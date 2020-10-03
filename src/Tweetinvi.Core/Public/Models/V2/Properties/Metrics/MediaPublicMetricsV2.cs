using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class MediaPublicMetricsV2
    {
        [JsonProperty("view_count")] public int ViewCount { get; set; }
    }
}