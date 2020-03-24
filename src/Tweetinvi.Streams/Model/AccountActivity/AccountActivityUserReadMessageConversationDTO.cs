using Newtonsoft.Json;

namespace Tweetinvi.Streams.Model.AccountActivity
{
    public class AccountActivityUserReadMessageConversationDTO : BaseAccountActivityMessageEventDTO
    {
        [JsonProperty("direct_message_mark_read_events")]
        public ActivityStreamDirectMessageConversationEventDTO[] MessageConversationReadEvents { get; set; }
    }
}
