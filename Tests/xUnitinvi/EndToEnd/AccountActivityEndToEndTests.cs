using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.AspNet;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Logic;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.Webhooks;
using Xunit;
using Xunit.Abstractions;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.EndToEnd
{
    public class TestingWebhookServer : IDisposable
    {
        private readonly HttpListener _server;

        public TestingWebhookServer(int port)
        {
            _server = new HttpListener();
            _server.Prefixes.Add($"http://*:{port}/");
        }

        public void Start()
        {
            _server.Start();

#pragma warning disable 4014
            RunServer();
#pragma warning restore 4014

            Task.Delay(1000).Wait();
        }

        private async Task RunServer()
        {
            while (_server.IsListening)
            {
                var context = await _server.GetContextAsync();
                this.Raise(OnRequest, context);
            }
        }

        public EventHandler<HttpListenerContext> OnRequest;

        public void Dispose()
        {
            ((IDisposable)_server)?.Dispose();
            GC.SuppressFinalize(this);
        }
    }

    [Collection("EndToEndTests")]
    public class WebhooksEndToEndTests : TweetinviTest
    {
        public WebhooksEndToEndTests(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public async Task Registration()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            if (_tweetinviClient.Credentials.BearerToken == null)
            {
                await _tweetinviClient.Auth.InitializeClientBearerToken();
            }

            Plugins.Add<WebhooksPlugin>();

            // cleanup
            var client = new TwitterClient(EndToEndTestConfig.TweetinviApi.Credentials);
            var environments = await client.AccountActivity.GetAccountActivityWebhookEnvironments();
            await RemoveAllExistingWebhooks(environments, client);

            using (var ngrok = new NgrokRunner())
            using (var server = new TestingWebhookServer(8042))
            {
                ngrok.Start(8042);

                var webhookUrl = await ngrok.GetUrl().ConfigureAwait(false);

                var router = TweetinviContainer.Resolve<IWebhookRouter>();
                var consumerCreds = new ConsumerOnlyCredentials(EndToEndTestConfig.TweetinviApi.Credentials);
                var configuration = new WebhookConfiguration(consumerCreds);

                server.OnRequest += async (sender, context) =>
                {
                    var request = new WebhooksRequestHandlerForHttpServer(context);

                    if (router.IsRequestManagedByTweetinvi(request, configuration))
                    {
                        await router.TryRouteRequest(request, configuration).ConfigureAwait(false);
                    }
                };

                server.Start();

                var newWebhook = await client.AccountActivity.RegisterAccountActivityWebhook("sandbox", webhookUrl);

                var newEnvironments = await client.AccountActivity.GetAccountActivityWebhookEnvironments();
                var newWebhooks = newEnvironments.SelectMany(x => x.Webhooks);

                await RemoveAllExistingWebhooks(newEnvironments, client);

                Assert.Contains(newWebhooks, webhook => webhook.Id == newWebhook.Id);
            }
        }

        private static async Task RemoveAllExistingWebhooks(IWebhookEnvironmentDTO[] environments, TwitterClient client)
        {
            foreach (var environment in environments)
            {
                foreach (var webhook in environment.Webhooks)
                {
                    await client.AccountActivity.RemoveAccountActivityWebhook(environment.Name, webhook.Id);
                }
            }
        }
    }
}