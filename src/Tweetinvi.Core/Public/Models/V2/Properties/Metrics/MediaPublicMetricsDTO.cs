using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class MediaPublicMetricsDTO
    {
        [JsonProperty("view_count")] public int ViewCount { get; set; }
    }
}