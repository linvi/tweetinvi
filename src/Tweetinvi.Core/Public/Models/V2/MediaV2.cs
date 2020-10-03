using Newtonsoft.Json;

namespace Tweetinvi.Models
{
    public class MediaV2
    {
        [JsonProperty("duration_ms")] public int DurationMs { get; set; }
        [JsonProperty("height")] public int Height { get; set; }
        [JsonProperty("media_key")] public string MediaKey { get; set; }
        [JsonProperty("preview_image_url")] public string PreviewImageUrl { get; set; }
        [JsonProperty("type")] public string Type { get; set; }
        [JsonProperty("width")] public int Width { get; set; }

        [JsonProperty("non_public_metrics")] public MediaNonPublicMetricsV2 NonPublicMetrics { get; set; }
        [JsonProperty("organic_metrics")] public MediaOrganicMetricsV2 OrganicMetrics { get; set; }
        [JsonProperty("promoted_metrics")] public MediaPromotedMetricsV2 PromotedMetrics { get; set; }
        [JsonProperty("public_metrics")] public MediaPublicMetricsV2 PublicMetrics { get; set; }
    }
}