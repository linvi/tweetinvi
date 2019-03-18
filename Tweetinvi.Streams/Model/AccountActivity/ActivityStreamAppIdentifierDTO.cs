using Newtonsoft.Json;

namespace Tweetinvi.Streams.Model.AccountActivity
{
    public class ActivityStreamAppIdentifierDTO
    {
        [JsonProperty("app_id")]
        public long AppId { get; set; }
    }
}
