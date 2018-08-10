using Newtonsoft.Json;

namespace Tweetinvi.Logic.DTO.ActivityStream
{
    public class ActivityStreamAppIdentifierDTO
    {
        [JsonProperty("app_id")]
        public long AppId { get; set; }
    }
}
