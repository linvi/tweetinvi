using System;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Public.Models.Authentication;
using Tweetinvi.Core.Public.Models.Interfaces.DTO.Webhooks;

namespace Examplinvi.NETStandard_2._0
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var consumerOnlyCredentials = new ConsumerOnlyCredentials("CONSUMER_TOKEN", "CONSUMER_SECRET")
            {
                ApplicationOnlyBearerToken = "BEARER_TOKEN"
            };

            IWebhookEnvironmentDTO[] webhookEnvironments = await Webhooks.GetAllWebhookEnvironmentsAsync(consumerOnlyCredentials);
            
            webhookEnvironments.ForEach(env =>
            {
                Console.WriteLine(env.Name);
            });
        }

        static void StartServer()
        {
            Plugins.Add<WebhooksPlugin>();

            //var server = Task.Run(() => Examplinvi.WebhooksServer.Program.Main(new string[] { }));

            //var client = TweetinviContainer.Resolve<IWebhookProtocolProcessClient>();

            //client.Start();

            //server.Wait();
        }
    }
}
