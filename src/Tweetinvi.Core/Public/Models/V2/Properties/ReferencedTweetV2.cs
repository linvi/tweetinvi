using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class ReferencedTweetV2
    {
        /// <summary>
        /// The unique identifier of the referenced Tweet.
        /// </summary>
        [JsonProperty("id")] public string Id { get; set; }

        /// <summary>
        /// Indicates the type of relationship between this Tweet and the Tweet returned in the response:
        /// * retweeted (this Tweet is a Retweet),
        /// * quoted (a Retweet with comment, also known as Quoted Tweet),
        /// * or replied_to (this Tweet is a reply).
        /// </summary>
        [JsonProperty("type")] public string Type { get; set; }
    }
}