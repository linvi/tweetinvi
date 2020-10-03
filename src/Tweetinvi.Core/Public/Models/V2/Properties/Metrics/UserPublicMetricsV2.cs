using Newtonsoft.Json;

namespace Tweetinvi.Models
{
    public class UserPublicMetricsV2
    {
        [JsonProperty("followers_count")] public int FollowersCount { get; set; }
        [JsonProperty("following_count")] public int FollowingCount { get; set; }
        [JsonProperty("listed_count")] public int ListedCount { get; set; }
        [JsonProperty("tweet_count")] public int TweetCount { get; set; }
    }
}