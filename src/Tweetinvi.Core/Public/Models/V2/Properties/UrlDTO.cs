using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class UrlDTO
    {
        [JsonProperty("display_url")] public string DisplayUrl { get; set; }
        [JsonProperty("end")] public int End { get; set; }
        [JsonProperty("expanded_url")] public string ExpandedUrl { get; set; }
        [JsonProperty("start")] public int Start { get; set; }
        [JsonProperty("url")] public string Url { get; set; }
        [JsonProperty("unwound_url")] public string UnwoundUrl { get; set; }
    }

    public class UrlsDTO
    {
        [JsonProperty("urls")] public UrlDTO[] Urls { get; set; }
    }
}