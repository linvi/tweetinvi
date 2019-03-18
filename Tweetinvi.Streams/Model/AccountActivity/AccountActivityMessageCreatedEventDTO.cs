using Newtonsoft.Json;
using Tweetinvi.Logic.DTO;

namespace Tweetinvi.Streams.Model.AccountActivity
{
    public class AccountActivityMessageCreatedEventDTO : BaseAccountActivityMessageEventDTO
    {
        [JsonProperty("direct_message_events")]
        public EventDTO[] MessageEvents { get; set; }
    }
}
