using System;
using Newtonsoft.Json;

namespace Tweetinvi.Models
{
    public class TweetV2
    {
        [JsonProperty("attachments")] public TweetAttachmentsV2 Attachments { get; set; }
        [JsonProperty("author_id")] public string AuthorId { get; set; }
        [JsonProperty("created_at")] public DateTimeOffset CreatedAt { get; set; }
        [JsonProperty("context_annotations")] public TweetContextAnnotationV2[] ContextAnnotations { get; set; }
        [JsonProperty("conversation_id")] public string ConversationId { get; set; }
        [JsonProperty("geo")] public GeoV2 Geo { get; set; }
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("in_reply_to_user_id")] public string InReplyToUserId { get; set; }
        [JsonProperty("lang")] public string Lang { get; set; }
        [JsonProperty("possibly_sensitive")] public bool PossiblySensitive { get; set; }
        [JsonProperty("referenced_tweets")] public ReferencedTweetV2[] ReferencedTweets { get; set; }
        [JsonProperty("source")] public string Source { get; set; }
        [JsonProperty("text")] public string Text { get; set; }
        [JsonProperty("withheld")] public TweetWithheldInfoV2 Withheld { get; set; }

        [JsonProperty("non_public_metrics")] public TweetNonPublicMetricsV2 NonPublicMetrics { get; set; }
        [JsonProperty("organic_metrics")] public TweetOrganicMetricsV2 OrganicMetrics { get; set; }
        [JsonProperty("promoted_metrics")] public TweetPromotedMetricsV2 PromotedMetrics { get; set; }
        [JsonProperty("public_metrics")] public TweetPublicMetricsV2 PublicMetrics { get; set; }
    }
}