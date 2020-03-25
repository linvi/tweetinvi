using System;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.AspNet;
using Tweetinvi.AspNet.Public;
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
        public async Task Registrations()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            await RunAccountActivityTest(async config =>
            {
                var client = config.AccountActivityClient;
                var environment = "sandbox";

                // act
                var webhookUrl = $"{await config.Ngrok.GetUrl()}";
                var newWebhook = await client.AccountActivity.CreateAccountActivityWebhook(environment, webhookUrl);

                try
                {
                    config.ShouldRespondToRequest = false;
                    await client.AccountActivity.TriggerAccountActivityWebhookCRC(environment, newWebhook.Id);
                    throw new Exception("Should have failed");
                }
                catch (TwitterException)
                {
                }

                var envWithDisabledWebhook = await client.AccountActivity.GetAccountActivityWebhookEnvironments();
                var disabledWebhooks = envWithDisabledWebhook.SelectMany(x => x.Webhooks).ToArray();

                config.ShouldRespondToRequest = true;
                await client.AccountActivity.TriggerAccountActivityWebhookCRC(environment, newWebhook.Id);

                var newEnvironments = await client.AccountActivity.GetAccountActivityWebhookEnvironments();
                var newWebhooks = newEnvironments.SelectMany(x => x.Webhooks).ToArray();
                var environmentWebhooks = await client.AccountActivity.GetAccountActivityEnvironmentWebhooks(environment);

                // cleanup
                await CleanAllEnvironments(client);

                // assert
                Assert.False(disabledWebhooks[0].Valid);
                Assert.True(newWebhooks[0].Valid);
                Assert.Contains(newWebhooks, webhook => webhook.Id == newWebhook.Id);
                Assert.Contains(environmentWebhooks, webhook => webhook.Id == newWebhook.Id);

            }, _tweetinviClient, _logger);
        }

        [Fact]
        public async Task Subscriptions()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var environment = "sandbox";

            await RunAccountActivityTest(async config =>
            {
                // arrange
                var client = config.AccountActivityClient;

                var webhookUrl = $"{await config.Ngrok.GetUrl()}?accountActivityEnvironment={environment}";
                await client.AccountActivity.CreateAccountActivityWebhook(environment, webhookUrl);

                var userClient = new TwitterClient(EndToEndTestConfig.ProtectedUserAuthenticatedToTweetinviApi.Credentials);

                // act
                await userClient.AccountActivity.SubscribeToAccountActivity(environment);

                var countAfterSubscription = await client.AccountActivity.CountAccountActivitySubscriptions();
                var subscriptions = await client.AccountActivity.GetAccountActivitySubscriptions(environment);

                var isSubscriber1 = await userClient.AccountActivity.IsAccountSubscribedToAccountActivity(environment);
                await client.AccountActivity.UnsubscribeFromAccountActivity(environment, EndToEndTestConfig.ProtectedUserAuthenticatedToTweetinviApi.UserId);
                var isSubscriber2 = await userClient.AccountActivity.IsAccountSubscribedToAccountActivity(environment);

                // cleanup
                await CleanAllEnvironments(client);

                // assert
                Assert.Equal(countAfterSubscription.ProvisionedCount, "15");
                Assert.Equal(countAfterSubscription.SubscriptionsCount, "1");
                Assert.Equal(subscriptions.Subscriptions[0].UserId, EndToEndTestConfig.ProtectedUserAuthenticatedToTweetinviApi.UserId.ToString());
                Assert.True(isSubscriber1);
                Assert.False(isSubscriber2);
            }, _tweetinviClient, _logger);
        }

        public static async Task RunAccountActivityTest(
            Func<RunAccountActivityTestConfig, Task> runTests,
            ITwitterClient tweetinviClient,
            ITestOutputHelper logger)
        {
            if (tweetinviClient.Credentials.BearerToken == null)
            {
                await tweetinviClient.Auth.InitializeClientBearerToken();
            }

            Plugins.Add<WebhooksPlugin>();

            try
            {
                var client = new TwitterClient(EndToEndTestConfig.TweetinviApi.Credentials);
                await CleanAllEnvironments(client);

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
                        if (await accountActivityHandler.IsRequestManagedByTweetinvi(webhookRequest))
                        {
                            await accountActivityHandler.TryRouteRequest(webhookRequest);
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

        public static async Task CleanAllEnvironments(ITwitterClient client)
        {
            var environments = await client.AccountActivity.GetAccountActivityWebhookEnvironments();
            await RemoveWebhooksFromEnvironment(environments, client);
        }

        private static async Task RemoveWebhooksFromEnvironment(IWebhookEnvironment[] environments, ITwitterClient client)
        {
            foreach (var environment in environments)
            {
                foreach (var webhook in environment.Webhooks)
                {
                    await client.AccountActivity.DeleteAccountActivityWebhook(environment.Name, webhook.Id);
                }
            }
        }
    }
}