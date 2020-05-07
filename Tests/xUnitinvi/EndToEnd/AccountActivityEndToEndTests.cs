using System;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.AspNet;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Xunit;
using Xunit.Abstractions;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.EndToEnd
{
    public class RunAccountActivityTestConfig
    {
        public bool ShouldRespondToRequest { get; set; }
        public ITwitterClient AccountActivityClient { get; set; }
        public NgrokRunner Ngrok { get; set; }
        public IAccountActivityRequestHandler AccountActivityRequestHandler { get; set; }
    }

    [Collection("EndToEndTests")]
    public class AccountActivityEndToEndTests : TweetinviTest
    {
        public AccountActivityEndToEndTests(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public async Task RegistrationsAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            await RunAccountActivityTestAsync(async config =>
            {
                var client = config.AccountActivityClient;
                var environment = "sandbox";

                // act
                var webhookUrl = $"{await config.Ngrok.GetUrlAsync()}";
                var newWebhook = await client.AccountActivity.CreateAccountActivityWebhookAsync(environment, webhookUrl);

                try
                {
                    config.ShouldRespondToRequest = false;
                    await client.AccountActivity.TriggerAccountActivityWebhookCRCAsync(environment, newWebhook.Id);
                    throw new Exception("Should have failed");
                }
                catch (TwitterException)
                {
                }

                var envWithDisabledWebhook = await client.AccountActivity.GetAccountActivityWebhookEnvironmentsAsync();
                var disabledWebhooks = envWithDisabledWebhook.SelectMany(x => x.Webhooks).ToArray();

                config.ShouldRespondToRequest = true;
                await client.AccountActivity.TriggerAccountActivityWebhookCRCAsync(environment, newWebhook.Id);

                var newEnvironments = await client.AccountActivity.GetAccountActivityWebhookEnvironmentsAsync();
                var newWebhooks = newEnvironments.SelectMany(x => x.Webhooks).ToArray();
                var environmentWebhooks = await client.AccountActivity.GetAccountActivityEnvironmentWebhooksAsync(environment);

                // cleanup
                await CleanAllEnvironmentsAsync(client);

                // assert
                Assert.False(disabledWebhooks[0].Valid);
                Assert.True(newWebhooks[0].Valid);
                Assert.Contains(newWebhooks, webhook => webhook.Id == newWebhook.Id);
                Assert.Contains(environmentWebhooks, webhook => webhook.Id == newWebhook.Id);

            }, _tweetinviClient, _logger);
        }

        [Fact]
        public async Task SubscriptionsAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var environment = "sandbox";

            await RunAccountActivityTestAsync(async config =>
            {
                // arrange
                var client = config.AccountActivityClient;

                var webhookUrl = $"{await config.Ngrok.GetUrlAsync()}?accountActivityEnvironment={environment}";
                await client.AccountActivity.CreateAccountActivityWebhookAsync(environment, webhookUrl);

                var userClient = new TwitterClient(EndToEndTestConfig.ProtectedUserAuthenticatedToTweetinviApi.Credentials);

                // act
                await userClient.AccountActivity.SubscribeToAccountActivityAsync(environment);

                var countAfterSubscription = await client.AccountActivity.CountAccountActivitySubscriptionsAsync();
                var subscriptions = await client.AccountActivity.GetAccountActivitySubscriptionsAsync(environment);

                var isSubscriber1 = await userClient.AccountActivity.IsAccountSubscribedToAccountActivityAsync(environment);
                await client.AccountActivity.UnsubscribeFromAccountActivityAsync(environment, EndToEndTestConfig.ProtectedUserAuthenticatedToTweetinviApi.UserId);
                var isSubscriber2 = await userClient.AccountActivity.IsAccountSubscribedToAccountActivityAsync(environment);

                // cleanup
                await CleanAllEnvironmentsAsync(client);

                // assert
                Assert.Equal(countAfterSubscription.ProvisionedCount, "15");
                Assert.Equal(countAfterSubscription.SubscriptionsCount, "1");
                Assert.Equal(subscriptions.Subscriptions[0].UserId, EndToEndTestConfig.ProtectedUserAuthenticatedToTweetinviApi.UserId.ToString());
                Assert.True(isSubscriber1);
                Assert.False(isSubscriber2);
            }, _tweetinviClient, _logger);
        }

        public static async Task RunAccountActivityTestAsync(
            Func<RunAccountActivityTestConfig, Task> runTests,
            ITwitterClient tweetinviClient,
            ITestOutputHelper logger)
        {
            if (tweetinviClient.Credentials.BearerToken == null)
            {
                await tweetinviClient.Auth.InitializeClientBearerTokenAsync();
            }

            Plugins.Add<AspNetPlugin>();

            try
            {
                var client = new TwitterClient(EndToEndTestConfig.TweetinviApi.Credentials);
                await CleanAllEnvironmentsAsync(client);

                using (var ngrok = new NgrokRunner())
                using (var server = new TestingWebhookServer(8042))
                {
                    ngrok.Start(8042);

                    var accountActivityHandler = client.AccountActivity.CreateRequestHandler();

                    var runTestConfig = new RunAccountActivityTestConfig
                    {
                        AccountActivityClient = client,
                        ShouldRespondToRequest = true,
                        Ngrok = ngrok,
                        AccountActivityRequestHandler = accountActivityHandler
                    };

                    server.OnRequest += async (sender, context) =>
                    {
                        // ReSharper disable once AccessToModifiedClosure
                        if (!runTestConfig.ShouldRespondToRequest)
                        {
                            return;
                        }

                        var webhookRequest = WebhookRequestFactory.Create(context);
                        if (await accountActivityHandler.IsRequestManagedByTweetinviAsync(webhookRequest))
                        {
                            await accountActivityHandler.TryRouteRequestAsync(webhookRequest);
                        }
                    };

                    server.Start();

                    if (runTests != null)
                    {
                        await runTests(runTestConfig);
                    }
                }
            }
            catch (TwitterException e)
            {
                logger.WriteLine(e.ToString());
                throw;
            }
        }

        public static async Task CleanAllEnvironmentsAsync(ITwitterClient client)
        {
            var environments = await client.AccountActivity.GetAccountActivityWebhookEnvironmentsAsync();
            await RemoveWebhooksFromEnvironmentAsync(environments, client);
        }

        private static async Task RemoveWebhooksFromEnvironmentAsync(IWebhookEnvironment[] environments, ITwitterClient client)
        {
            foreach (var environment in environments)
            {
                foreach (var webhook in environment.Webhooks)
                {
                    await client.AccountActivity.DeleteAccountActivityWebhookAsync(environment.Name, webhook.Id);
                }
            }
        }
    }
}