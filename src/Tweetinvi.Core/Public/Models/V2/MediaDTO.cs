using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class MediaDTO
    {
        [JsonProperty("duration_ms")] public int duration_ms { get; set; }

        [JsonProperty("height")] public int height { get; set; }

        [JsonProperty("media_key")] public string media_key { get; set; }

        [JsonProperty("non_public_metrics")] public MediaNonPublicMetricsDTO non_public_metrics { get; set; }

        [JsonProperty("organic_metrics")] public MediaOrganicMetricsDTO organic_metrics { get; set; }

        [JsonProperty("preview_image_url")] public string preview_image_url { get; set; }

        [JsonProperty("promoted_metrics")] public MediaPromotedMetricsDTO promoted_metrics { get; set; }

        [JsonProperty("public_metrics")] public MediaPublicMetricsDTO public_metrics { get; set; }

        [JsonProperty("type")] public string type { get; set; }

        [JsonProperty("width")] public int width { get; set; }
    }
}