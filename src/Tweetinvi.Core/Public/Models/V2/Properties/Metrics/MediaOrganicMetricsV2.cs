using Newtonsoft.Json;

namespace Tweetinvi.Models
{
    public class MediaOrganicMetricsV2 : MediaMetricsV2
    {
        [JsonProperty("view_count")] public int ViewCount { get; set; }
    }
}