using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class TweetIncludesV2
    {
        /// <summary>
        /// Medias associated with the tweets, like the images attached.
        /// </summary>
        [JsonProperty("media")] public MediaV2[] Medias { get; set; }

        /// <summary>
        /// Places associated with the tweets
        /// </summary>
        [JsonProperty("places")] public PlaceV2[] Places { get; set; }

        /// <summary>
        /// Polls associated with the tweets
        /// </summary>
        [JsonProperty("polls")] public PollV2[] Polls { get; set; }

        /// <summary>
        /// Tweets associated with the tweets, like retweets, replies...
        /// </summary>
        [JsonProperty("tweets")] public TweetV2[] Tweets { get; set; }

        /// <summary>
        /// Users associated with the tweets, like the owner.
        /// </summary>
        [JsonProperty("users")] public UserV2[] Users { get; set; }
    }
}