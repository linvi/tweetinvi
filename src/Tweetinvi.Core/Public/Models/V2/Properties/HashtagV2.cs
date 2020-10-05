using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class HashtagV2
    {
        /// <summary>
        /// Index of the first letter of the tag
        /// </summary>
        [JsonProperty("start")] public int Start { get; set; }

        /// <summary>
        /// Index of the last letter of the tag
        /// </summary>
        [JsonProperty("end")] public int End { get; set; }

        /// <summary>
        /// The text of the Hashtag.
        /// </summary>
        [JsonProperty("tag")] public string Tag { get; set; }

        [JsonProperty("hashtag")] public string Hashtag
        {
            set => Tag = value;
        }
    }
}