using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class TweetAttachmentsV2
    {
        /// <summary>
        /// List of unique identifiers of media attached to this Tweet.
        /// These identifiers use the same media key format as those returned by the Media Library.
        /// </summary>
        [JsonProperty("media_keys")] public string[] MediaKeys { get; set; }

        /// <summary>
        /// List of unique identifiers of polls present in the Tweets returned.
        /// These are returned as a string in order to avoid complications with languages and tools that cannot handle large integers.
        /// </summary>
        [JsonProperty("poll_ids")] public string[] PollIds { get; set; }
    }
}