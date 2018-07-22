using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tweetinvi;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Public.Models.Interfaces.DTO.Webhooks;
using Tweetinvi.Models.DTO;

namespace Testinvi.json.net
{
    [TestClass]
    public class WebhookModelsDeserializerTests
    {
        [TestMethod]
        public void RegisterWebhookFormat()
        {
            var converter = TweetinviContainer.Resolve<IJsonObjectConverter>();

            var json = @"{
                     ""id"": ""1234567890"",
                     ""url"": ""https://your_domain.com/webhook/twitter"",
                     ""valid"": true,
                     ""created_at"": ""2017-06-02T23:54:02Z""
            }";

            var dto = converter.DeserializeObject<IWebhookDTO>(json);

            Console.WriteLine(dto);
        }

        [TestMethod]
        public void GetAllWebhookFormat()
        {
            var converter = TweetinviContainer.Resolve<IJsonObjectConverter>();

            var json = @"{
              ""environments"": [
                {
                  ""environment_name"": ""env-beta"",
                  ""webhooks"":  [
                   {
                     ""id"": ""1234567890"",
                     ""url"": ""https://your_domain.com/webhook/twitter"",
                     ""valid"": true,
                     ""created_at"": ""2017-06-02T23:54:02Z""
                   }
                  ]
                }
              ]
            }";

            var dto = converter.DeserializeObject<IGetAllWebhooksResultDTO>(json);

            Console.WriteLine(dto);
        }

        [TestMethod]
        public void GetWebhookSubscriptionsCount()
        {
            var converter = TweetinviContainer.Resolve<IJsonObjectConverter>();

            var json = @"{
                ""account_name"": ""test-account"",
                ""subscriptions_count_all"": ""2"",
                ""subscriptions_count_direct_messages"": ""1""
            }";

            var dto = converter.DeserializeObject<IGetWebhookSubscriptionsCountResultDTO>(json);

            Console.WriteLine(dto);
        }


        [TestMethod]
        public void GetWebhookSubscriptionList()
        {
            var converter = TweetinviContainer.Resolve<IJsonObjectConverter>();

            var json = @"{
                ""environment"": ""appname"",
                ""application_id"": ""13090192"",
                ""subscriptions"": [
                  {
                    ""user_id"": ""3001969357""
                  }
                ]
            }";

            var dto = converter.DeserializeObject<IWebhookSubcriptionListDTO>(json);

            Console.WriteLine(dto);
        }
    }
}
