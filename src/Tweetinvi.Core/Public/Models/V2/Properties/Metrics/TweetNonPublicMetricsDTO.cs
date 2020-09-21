using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class TweetNonPublicMetricsDTO
    {
        [JsonProperty("impression_count")] public int impression_count { get; set; }

        [JsonProperty("url_link_clicks")] public int url_link_clicks { get; set; }

        [JsonProperty("user_profile_clicks")] public int user_profile_clicks { get; set; }
    }
}