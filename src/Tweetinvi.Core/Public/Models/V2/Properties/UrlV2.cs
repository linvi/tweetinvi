using Newtonsoft.Json;

namespace Tweetinvi.Models
{
    public class UrlV2
    {
        [JsonProperty("display_url")] public string DisplayUrl { get; set; }
        [JsonProperty("end")] public int End { get; set; }
        [JsonProperty("expanded_url")] public string ExpandedUrl { get; set; }
        [JsonProperty("start")] public int Start { get; set; }
        [JsonProperty("url")] public string Url { get; set; }
        [JsonProperty("unwound_url")] public string UnwoundUrl { get; set; }
    }

    public class UrlsV2
    {
        [JsonProperty("urls")] public UrlV2[] Urls { get; set; }
    }
}