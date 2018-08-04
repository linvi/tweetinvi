using Newtonsoft.Json;
using Tweetinvi.Core.Public.Streaming.Events;

namespace Tweetinvi.Logic.DTO.ActivityStream
{
    public class ActivityStreamUserIdentifierDTO : IActivityStreamUserIdentifierDTO
    {
        [JsonProperty("user_id")]
        public long UserId { get; set; }
    }
}
