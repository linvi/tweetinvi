using Newtonsoft.Json;
using Tweetinvi.Core.DTO.Events;

namespace Tweetinvi.Streams.Model.AccountActivity
{
    public class AccountActivityMessageCreatedEventDTO : BaseAccountActivityMessageEventDTO
    {
        [JsonProperty("direct_message_events")]
        public MessageEventDTO[] MessageEvents { get; set; }
    }
}
