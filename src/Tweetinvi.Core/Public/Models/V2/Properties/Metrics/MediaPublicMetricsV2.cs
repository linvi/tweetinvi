using Newtonsoft.Json;

namespace Tweetinvi.Models
{
    public class MediaPublicMetricsV2
    {
        [JsonProperty("view_count")] public int ViewCount { get; set; }
    }
}