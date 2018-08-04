using Newtonsoft.Json;
using Tweetinvi.Core.Public.Streaming.Events;

namespace Tweetinvi.Logic.DTO.ActivityStream
{
    public class ActivityStreamAppIdentifierDTO : IActivityStreamAppIdentifierDTO
    {
        [JsonProperty("app_id")]
        public long AppId { get; set; }
    }
}
