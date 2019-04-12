using Newtonsoft.Json;
using Tweetinvi.Logic.JsonConverters;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.Entities;

namespace Tweetinvi.Logic.DTO
{
    public class MessageDataDTO : IMessageDataDTO
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("entities")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public IMessageEntities Entities { get; set; }

        [JsonProperty("quick_reply")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public IQuickReplyDTO QuickReply { get; set; }

        [JsonProperty("quick_reply_response")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public IQuickReplyResponse QuickReplyResponse { get; set; }

        [JsonProperty("attachment")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public IAttachmentDTO Attachment { get; set; }
    }
}
