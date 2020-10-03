using Newtonsoft.Json;

namespace Tweetinvi.Models
{
    public class MediaPromotedMetricsV2 : MediaMetricsV2
    {
        [JsonProperty("view_count")] public int ViewCount { get; set; }
    }
}