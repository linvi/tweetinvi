using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class UserMentionV2
    {
        /// <summary>
        /// The end position (zero-based) of the recognized user mention within the Tweet.
        /// </summary>
        [JsonProperty("end")] public int End { get; set; }

        /// <summary>
        /// The start position (zero-based) of the recognized user mention within the Tweet.
        /// </summary>
        [JsonProperty("start")] public int Start { get; set; }

        /// <summary>
        /// The part of text recognized as a user mention.
        /// </summary>
        [JsonProperty("username")] public string Username { get; set; }
    }
}