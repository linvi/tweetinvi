using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class TweetNonPublicMetricsV2
    {
        [JsonProperty("impression_count")] public int ImpressionCount { get; set; }
        [JsonProperty("url_link_clicks")] public int UrlLinkClicks { get; set; }
        [JsonProperty("user_profile_clicks")] public int UserProfileClicks { get; set; }
    }
}