using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class UserIncludesDTO
    {
        [JsonProperty("tweets")] public TweetDTO[] Tweets { get; set; }
    }
}