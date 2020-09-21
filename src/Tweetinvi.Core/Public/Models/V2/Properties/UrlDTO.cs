using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class UrlDTO
    {
        [JsonProperty("display_url")] public string display_url { get; set; }

        [JsonProperty("end")] public int end { get; set; }

        [JsonProperty("expanded_url")] public string expanded_url { get; set; }

        [JsonProperty("start")] public int start { get; set; }

        [JsonProperty("url")] public string url { get; set; }

        [JsonProperty("unwound_url")] public string unwound_url { get; set; }
    }

    public class UrlsDTO
    {
        [JsonProperty("urls")] public UrlDTO[] urls { get; set; }
    }
}