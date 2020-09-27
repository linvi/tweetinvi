using System.Collections.Generic;

namespace Tweetinvi.Core.Parameters
{
    public class TweetFields
    {
        /// <summary>
        /// All the fields available on all tweets.
        /// This only includes the `PublicMetrics`
        /// </summary>
        public HashSet<string> ALL => new HashSet<string>
        {
            Attachments,
            AuthorId,
            ContextAnnotations,
            ConversationId,
            CreatedAt,
            Entities,
            Geo,
            Id,
            InReplyToUserId,
            Lang,
            PossiblySensitive,
            ReferencedTweets,
            Source,
            Text,
            Withheld,

            PublicMetrics,
        };

        public readonly string Attachments = "attachments";
        public readonly string AuthorId = "author_id";
        public readonly string ContextAnnotations = "context_annotations";
        public readonly string ConversationId = "conversation_id";
        public readonly string CreatedAt = "created_at";
        public readonly string Entities = "entities";
        public readonly string Geo = "geo";
        public readonly string Id = "id";
        public readonly string InReplyToUserId = "in_reply_to_user_id";
        public readonly string Lang = "lang";
        public readonly string NonPublicMetrics = "non_public_metrics";
        public readonly string PublicMetrics = "public_metrics";
        public readonly string OrganicMetrics = "organic_metrics";
        public readonly string PromotedMetrics = "promoted_metrics";
        public readonly string PossiblySensitive = "possibly_sensitive";
        public readonly string ReferencedTweets = "referenced_tweets";
        public readonly string Source = "source";
        public readonly string Text = "text";
        public readonly string Withheld = "withheld";
    }
}