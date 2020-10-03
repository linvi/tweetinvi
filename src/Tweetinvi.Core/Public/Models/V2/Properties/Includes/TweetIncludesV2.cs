using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class TweetIncludesV2
    {
        [JsonProperty("media")] public MediaV2[] Media { get; set; }
        [JsonProperty("places")] public PlaceV2[] Places { get; set; }
        [JsonProperty("polls")] public PollV2[] Polls { get; set; }
        [JsonProperty("tweets")] public TweetV2[] Tweets { get; set; }
        [JsonProperty("users")] public UserV2[] Users { get; set; }
    }
}