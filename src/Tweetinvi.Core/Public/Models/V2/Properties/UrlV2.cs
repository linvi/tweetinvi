using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class UrlV2
    {
        /// <summary>
        /// The URL as displayed in the Twitter client.
        /// </summary>
        [JsonProperty("display_url")] public string DisplayUrl { get; set; }

        /// <summary>
        /// The end position (zero-based) of the recognized URL within the Tweet.
        /// </summary>
        [JsonProperty("end")] public int End { get; set; }

        /// <summary>
        /// The fully resolved URL.
        /// </summary>
        [JsonProperty("expanded_url")] public string ExpandedUrl { get; set; }

        /// <summary>
        /// The start position (zero-based) of the recognized URL within the Tweet.
        /// </summary>
        [JsonProperty("start")] public int Start { get; set; }

        /// <summary>
        /// The URL in the format tweeted by the user.
        /// </summary>
        [JsonProperty("url")] public string Url { get; set; }

        /// <summary>
        /// The full destination URL.
        /// </summary>
        [JsonProperty("unwound_url")] public string UnwoundUrl { get; set; }
        
        /// <summary>
        /// Title of the URL
        /// </summary>
        [JsonProperty("title")] public string Title { get; set; }

        /// <summary>
        /// Description of the URL
        /// </summary>
        [JsonProperty("description")] public string Description { get; set; }

    }

    public class UrlsV2
    {
        /// <summary>
        /// Contains details about text recognized as a URL.
        /// </summary>
        [JsonProperty("urls")] public UrlV2[] Urls { get; set; }
    }
}
