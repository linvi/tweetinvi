using Newtonsoft.Json;
using Tweetinvi.Core.Public.Models.Interfaces.DTO.Webhooks;

namespace Tweetinvi.Logic.DTO
{
    public class GetWebhookSubscriptionsCountResultDTO : IGetWebhookSubscriptionsCountResultDTO
    {
        [JsonProperty("account_name")]
        public string AccountName { get; set; }
        [JsonProperty("subscriptions_count_all")]
        public string SubscriptionsCountAll { get; set; }
        [JsonProperty("subscriptions_count_direct_messages")]
        public string SubscriptionsCountDirectMessages { get; set; }
    }
}
