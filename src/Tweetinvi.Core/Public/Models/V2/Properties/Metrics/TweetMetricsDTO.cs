using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class TweetMetricsDTO
    {
        [JsonProperty("impression_count")] public int ImpressionCount { get; set; }
        [JsonProperty("like_count")] public int LikeCount { get; set; }
        [JsonProperty("reply_count")] public int ReplyCount { get; set; }
        [JsonProperty("retweet_count")] public int RetweetCount { get; set; }
        [JsonProperty("url_link_clicks")] public int UrlLinkClicks { get; set; }
        [JsonProperty("user_profile_clicks")] public int UserProfileClicks { get; set; }
    }
}