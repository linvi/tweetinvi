using System;
using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class TweetDTO
    {
        [JsonProperty("attachments")] public AttachmentsDTO attachments { get; set; }

        [JsonProperty("author_id")] public string author_id { get; set; }

        [JsonProperty("created_at")] public DateTimeOffset created_at { get; set; }

        [JsonProperty("context_annotations")] public TweetContextAnnotationDTO[] context_annotations { get; set; }

        [JsonProperty("conversation_id")] public string conversation_id { get; set; }

        [JsonProperty("geo")] public GeoDTO geo { get; set; }

        [JsonProperty("id")] public string id { get; set; }

        [JsonProperty("in_reply_to_user_id")] public string in_reply_to_user_id { get; set; }

        [JsonProperty("lang")] public string lang { get; set; }

        [JsonProperty("non_public_metrics")] public TweetNonPublicMetricsDTO non_public_metrics { get; set; }

        [JsonProperty("organic_metrics")] public TweetOrganicMetricsDTO organic_metrics { get; set; }

        [JsonProperty("possibly_sensitive")] public bool possibly_sensitive { get; set; }

        [JsonProperty("promoted_metrics")] public TweetPromotedMetricsDTO promoted_metrics { get; set; }

        [JsonProperty("public_metrics")] public TweetPublicMetricsDTO public_metrics { get; set; }

        [JsonProperty("referenced_tweets")] public ReferencedTweetDTO referenced_tweets { get; set; }

        [JsonProperty("source")] public string source { get; set; }

        [JsonProperty("text")] public string text { get; set; }

        [JsonProperty("withheld")] public TweetWithheldDTO withheld { get; set; }
    }
}