using Newtonsoft.Json;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.DTO
{
    public class MessageCreateTargetDTO : IMessageCreateTargetDTO
    {
        [JsonProperty("recipient_id")]
        public long RecipientId { get; set; }
    }
}
