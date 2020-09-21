using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class UserPublicMetricsDTO
    {
        [JsonProperty("followers_count")] public int followers_count { get; set; }

        [JsonProperty("following_count")] public int following_count { get; set; }

        [JsonProperty("listed_count")] public int listed_count { get; set; }

        [JsonProperty("tweet_count")] public int tweet_count { get; set; }
    }
}