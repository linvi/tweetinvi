using Newtonsoft.Json;

namespace Tweetinvi.Models
{
    public class TweetPublicMetricsV2
    {
        [JsonProperty("like_count")] public int LikeCount { get; set; }
        [JsonProperty("quote_count")] public int QuoteCount { get; set; }
        [JsonProperty("reply_count")] public int ReplyCount { get; set; }
        [JsonProperty("retweet_count")] public int RetweetCount { get; set; }
    }
}