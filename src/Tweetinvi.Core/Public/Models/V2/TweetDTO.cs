using System;
using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class TweetDTO
    {
        [JsonProperty("attachments")] public AttachmentsDTO Attachments { get; set; }
        [JsonProperty("author_id")] public string AuthorId { get; set; }
        [JsonProperty("created_at")] public DateTimeOffset CreatedAt { get; set; }
        [JsonProperty("context_annotations")] public TweetContextAnnotationDTO[] ContextAnnotations { get; set; }
        [JsonProperty("conversation_id")] public string ConversationId { get; set; }
        [JsonProperty("geo")] public GeoDTO Geo { get; set; }
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("in_reply_to_user_id")] public string InReplyToUserId { get; set; }
        [JsonProperty("lang")] public string Lang { get; set; }
        [JsonProperty("possibly_sensitive")] public bool PossiblySensitive { get; set; }
        [JsonProperty("referenced_tweets")] public ReferencedTweetDTO[] ReferencedTweets { get; set; }
        [JsonProperty("source")] public string Source { get; set; }
        [JsonProperty("text")] public string Text { get; set; }
        [JsonProperty("withheld")] public TweetWithheldDTO Withheld { get; set; }

        [JsonProperty("non_public_metrics")] public TweetNonPublicMetricsDTO NonPublicMetrics { get; set; }
        [JsonProperty("organic_metrics")] public TweetOrganicMetricsDTO OrganicMetrics { get; set; }
        [JsonProperty("promoted_metrics")] public TweetPromotedMetricsDTO PromotedMetrics { get; set; }
        [JsonProperty("public_metrics")] public TweetPublicMetricsDTO PublicMetrics { get; set; }
    }
}