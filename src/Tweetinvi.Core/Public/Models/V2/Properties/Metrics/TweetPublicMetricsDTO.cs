using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class TweetPublicMetricsDTO
    {
        [JsonProperty("like_count")] public int like_count { get; set; }

        [JsonProperty("quote_count")] public int quote_count { get; set; }

        [JsonProperty("reply_count")] public int reply_count { get; set; }

        [JsonProperty("retweet_count")] public int retweet_count { get; set; }
    }
}