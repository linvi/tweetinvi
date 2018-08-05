using Newtonsoft.Json;

namespace Tweetinvi.Logic.DTO.ActivityStream
{
    public class ActivityStreamUserIdentifierDTO
    {
        [JsonProperty("user_id")]
        public long UserId { get; set; }
    }
}
