using System;
using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    /// <summary>
    /// A Tweet
    /// <para>Read more here : https://developer.twitter.com/en/docs/twitter-api/data-dictionary/object-model/tweet </para>
    /// </summary>
    public class TweetV2
    {
        /// <summary>
        /// Specifies the type of attachments (if any) present in this Tweet.
        /// </summary>
        [JsonProperty("attachments")] public TweetAttachmentsV2 Attachments { get; set; }

        /// <summary>
        /// The unique identifier of the User who posted this Tweet.
        /// </summary>
        [JsonProperty("author_id")] public string AuthorId { get; set; }

        /// <summary>
        /// Contains context annotations for the Tweet.
        /// <para>
        /// Derived from the analysis of a Tweet’s text and will include a domain and entity pairing
        /// which can be used to discover Tweets on topics that may have been previously difficult to surface.
        /// At present, we’re using a list of 50+ domains to categorize Tweets.
        /// </para>
        /// <para>Read more: https://developer.twitter.com/en/docs/twitter-api/annotations </para>
        /// </summary>
        [JsonProperty("context_annotations")] public TweetContextAnnotationV2[] ContextAnnotations { get; set; }

        /// <summary>
        /// The Tweet ID of the original Tweet of the conversation (which includes direct replies, replies of replies).
        /// </summary>
        [JsonProperty("conversation_id")] public string ConversationId { get; set; }

        /// <summary>
        /// Creation time of the Tweet.
        /// </summary>
        [JsonProperty("created_at")] public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Entities which have been parsed out of the text of the Tweet.
        /// </summary>
        [JsonProperty("entities")] public TweetEntitiesV2 Entities { get; set; }

        /// <summary>
        /// Contains details about the location tagged by the user in this Tweet, if they specified one.
        /// </summary>
        [JsonProperty("geo")] public GeoV2 Geo { get; set; }

        /// <summary>
        /// The unique identifier of the requested Tweet.
        /// </summary>
        [JsonProperty("id")] public string Id { get; set; }

        /// <summary>
        /// If the represented Tweet is a reply, this field will contain the original Tweet’s author ID.
        /// This will not necessarily always be the user directly mentioned in the Tweet.
        /// </summary>
        [JsonProperty("in_reply_to_user_id")] public string InReplyToUserId { get; set; }

        /// <summary>
        /// Language of the Tweet, if detected by Twitter. Returned as a BCP47 language tag.
        /// </summary>
        [JsonProperty("lang")] public string Lang { get; set; }

        /// <summary>
        /// This field only surfaces when a Tweet contains a link.
        /// The meaning of the field doesn’t pertain to the Tweet content itself, but instead it is an indicator
        /// that the URL contained in the Tweet may contain content or media identified as sensitive content.
        /// </summary>
        [JsonProperty("possibly_sensitive")] public bool PossiblySensitive { get; set; }

        /// <summary>
        /// A list of Tweets this Tweet refers to. For example, if the parent Tweet is a Retweet,
        /// a Retweet with comment (also known as Quoted Tweet) or a Reply,
        /// it will include the related Tweet referenced to by its parent.
        /// </summary>
        [JsonProperty("referenced_tweets")] public ReferencedTweetV2[] ReferencedTweets { get; set; }

        /// <summary>
        /// The name of the app the user Tweeted from.
        /// </summary>
        [JsonProperty("source")] public string Source { get; set; }

        /// <summary>
        /// The actual UTF-8 text of the Tweet.
        /// </summary>
        [JsonProperty("text")] public string Text { get; set; }

        /// <summary>
        /// When present, contains withholding details for withheld content.
        /// <para>Read more: https://help.twitter.com/en/rules-and-policies/tweet-withheld-by-country </para>
        /// </summary>
        [JsonProperty("withheld")] public TweetWithheldInfoV2 Withheld { get; set; }

        /// <summary>
        /// Non-public engagement metrics for the Tweet at the time of the request.
        /// </summary>
        [JsonProperty("non_public_metrics")] public TweetNonPublicMetricsV2 NonPublicMetrics { get; set; }

        /// <summary>
        /// Engagement metrics, tracked in an organic context, for the Tweet at the time of the request.
        /// </summary>
        [JsonProperty("organic_metrics")] public TweetOrganicMetricsV2 OrganicMetrics { get; set; }

        /// <summary>
        /// Engagement metrics, tracked in a promoted context, for the Tweet at the time of the request.
        /// </summary>
        [JsonProperty("promoted_metrics")] public TweetPromotedMetricsV2 PromotedMetrics { get; set; }

        /// <summary>
        /// Public engagement metrics for the Tweet at the time of the request.
        /// </summary>
        [JsonProperty("public_metrics")] public TweetPublicMetricsV2 PublicMetrics { get; set; }

        /// <summary>
        ///     Author of the Tweet
        /// </summary>
        [JsonIgnore] public UserV2 TweetAuthor { get; set; }
    }
}