using Newtonsoft.Json;

namespace Tweetinvi.Streams.Model.AccountActivity
{
    public class ActivityStreamUserIdentifierDTO
    {
        [JsonProperty("user_id")]
        public long UserId { get; set; }
    }
}
