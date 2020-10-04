using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    /// <summary>
    /// A media, video, image, gif...
    /// <para>Read more here : https://developer.twitter.com/en/docs/twitter-api/data-dictionary/object-model/media </para>
    /// </summary>
    public class MediaV2
    {
        /// <summary>
        /// Available when type is video. Duration in milliseconds of the video.
        /// </summary>
        [JsonProperty("duration_ms")] public int DurationMs { get; set; }

        /// <summary>
        /// Height of this content in pixels.
        /// </summary>
        [JsonProperty("height")] public int Height { get; set; }

        /// <summary>
        /// Unique identifier of the expanded media content.
        /// </summary>
        [JsonProperty("media_key")] public string MediaKey { get; set; }

        /// <summary>
        /// URL to the static placeholder preview of this content.
        /// </summary>
        [JsonProperty("preview_image_url")] public string PreviewImageUrl { get; set; }

        /// <summary>
        /// Type of content (animated_gif, photo, video).
        /// </summary>
        [JsonProperty("type")] public string Type { get; set; }

        /// <summary>
        /// Width of this content in pixels.
        /// </summary>
        [JsonProperty("width")] public int Width { get; set; }

        /************* METRICS ************/

        /// <summary>
        /// Non-public engagement metrics for the media content at the time of the request.
        /// </summary>
        [JsonProperty("non_public_metrics")] public MediaNonPublicMetricsV2 NonPublicMetrics { get; set; }

        /// <summary>
        /// Engagement metrics for the media content, tracked in an organic context, at the time of the request.
        /// </summary>
        [JsonProperty("organic_metrics")] public MediaOrganicMetricsV2 OrganicMetrics { get; set; }

        /// <summary>
        /// Engagement metrics for the media content, tracked in a promoted context, at the time of the request.
        /// </summary>
        [JsonProperty("promoted_metrics")] public MediaPromotedMetricsV2 PromotedMetrics { get; set; }

        /// <summary>
        /// Public engagement metrics for the media content at the time of the request.
        /// </summary>
        [JsonProperty("public_metrics")] public MediaPublicMetricsV2 PublicMetrics { get; set; }
    }
}