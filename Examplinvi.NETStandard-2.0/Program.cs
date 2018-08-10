using System;
using Tweetinvi;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Public.Models.Authentication;
using Tweetinvi.Core.Public.Models.Interfaces.DTO.Webhooks;

namespace Examplinvi.NETStandard_2._0
{
    class Program
    {
        static void Main(string[] args)
        {
            Sync.ExecuteTaskAsync(() =>
            {
                var consumerOnlyCredentials = new ConsumerOnlyCredentials("CONSUMER_TOKEN", "CONSUMER_SECRET")
                {
                    ApplicationOnlyBearerToken = "BEARER_TOKEN"
                };

                IWebhookEnvironmentDTO[] webhookEnvironments = Webhooks.GetAllWebhookEnvironmentsAsync(consumerOnlyCredentials).Result;
                
                webhookEnvironments.ForEach(env =>
                {
                    Console.WriteLine(env.Name);
                });
            }).Wait();

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
