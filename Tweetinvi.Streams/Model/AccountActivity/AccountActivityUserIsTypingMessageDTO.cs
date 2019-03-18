using Newtonsoft.Json;

namespace Tweetinvi.Streams.Model.AccountActivity
{
    public class AccountActivityUserIsTypingMessageDTO : BaseAccountActivityMessageEventDTO
    {
        [JsonProperty("direct_message_indicate_typing_events")]
        public ActivityStreamDirectMessageConversationEventDTO[] TypingEvents { get; set; }
    }
}
