using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class UserIncludesV2
    {
        [JsonProperty("tweets")] public TweetV2[] Tweets { get; set; }
    }
}