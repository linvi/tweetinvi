using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class UserIncludesV2
    {
        /// <summary>
        /// Tweets associated with the user, usually the pinned tweets
        /// </summary>
        [JsonProperty("tweets")] public TweetV2[] Tweets { get; set; }
    }
}