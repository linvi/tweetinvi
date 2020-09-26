using System.Collections.Generic;

namespace Tweetinvi.Parameters.V2
{
    public static class TweetFields
    {
        public static class Expansions
        {
            public static HashSet<string> ALL => new HashSet<string>
            {
                AttachmentsPollIds,
                AttachmentsMediaKeys,
                AuthorId,
                EntitiesMentionsUsername,
                GeoPlaceId,
                InReplyToUserId,
                ReferencedTweetsId,
                ReferencedTweetsIdAuthorId
            };

            public const string AttachmentsPollIds = "attachments.poll_ids";
            public const string AttachmentsMediaKeys = "attachments.media_keys";
            public const string AuthorId = "author_id";
            public const string GeoPlaceId = "geo.place_id";
            public const string EntitiesMentionsUsername = "entities.mentions.username";
            public const string InReplyToUserId = "in_reply_to_user_id";
            public const string ReferencedTweetsId = "referenced_tweets.id";
            public const string ReferencedTweetsIdAuthorId = "referenced_tweets.id.author_id";
        }

        public static class Media
        {
            public static HashSet<string> ALL => new HashSet<string>
            {
                DurationMs,
                Height,
                MediaKey,
                PreviewImageUrl,
                Type,
                Url,
                Width,
            };

            public static HashSet<string> ALL_METRICS => new HashSet<string>
            {
                PublicMetrics,
                NonPublicMetrics,
                OrganicMetrics,
                PromotedMetrics,
            };

            public const string DurationMs = "duration_ms";
            public const string Height = "height";
            public const string MediaKey = "media_key";
            public const string PreviewImageUrl = "preview_image_url";
            public const string Type = "type";
            public const string Url = "url";
            public const string Width = "width";
            public const string PublicMetrics = "public_metrics";
            public const string NonPublicMetrics = "non_public_metrics";
            public const string OrganicMetrics = "organic_metrics";
            public const string PromotedMetrics = "promoted_metrics";
        }

        public static class Place
        {
            public static HashSet<string> ALL => new HashSet<string>
            {
                ContainedWithin,
                Country,
                CountryCode,
                FullName,
                Geo,
                Id,
                Name,
                PlaceType,
            };

            public const string ContainedWithin = "contained_within";
            public const string Country = "country";
            public const string CountryCode = "country_code";
            public const string FullName = "full_name";
            public const string Geo = "geo";
            public const string Id = "id";
            public const string Name = "name";
            public const string PlaceType = "place_type";
        }

        public static class Polls
        {
            public static HashSet<string> ALL => new HashSet<string>
            {
                DurationMinutes,
                EndDatetime,
                Id,
                Options,
                VotingStatus,
            };

            public const string DurationMinutes = "duration_minutes";
            public const string EndDatetime = "end_datetime";
            public const string Id = "id";
            public const string Options = "options";
            public const string VotingStatus = "voting_status";
        }

        public static class Tweet
        {
            public static HashSet<string> ALL => new HashSet<string>
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
                Withheld
            };

            public static HashSet<string> ALL_METRICS => new HashSet<string>
            {
                NonPublicMetrics,
                PublicMetrics,
                OrganicMetrics,
                PromotedMetrics,
            };

            public const string Attachments = "attachments";
            public const string AuthorId = "author_id";
            public const string ContextAnnotations = "context_annotations";
            public const string ConversationId = "conversation_id";
            public const string CreatedAt = "created_at";
            public const string Entities = "entities";
            public const string Geo = "geo";
            public const string Id = "id";
            public const string InReplyToUserId = "in_reply_to_user_id";
            public const string Lang = "lang";
            public const string NonPublicMetrics = "non_public_metrics";
            public const string PublicMetrics = "public_metrics";
            public const string OrganicMetrics = "organic_metrics";
            public const string PromotedMetrics = "promoted_metrics";
            public const string PossiblySensitive = "possibly_sensitive";
            public const string ReferencedTweets = "referenced_tweets";
            public const string Source = "source";
            public const string Text = "text";
            public const string Withheld = "withheld";
        }

        public static class User
        {
            public static HashSet<string> ALL => new HashSet<string>
            {
                CreatedAt,
                Description,
                Entities,
                Id,
                Location,
                Name,
                PinnedTweetId,
                ProfileImageUrl,
                Protected,
                Url,
                Username,
                Verified,
                Withheld
            };

            public static HashSet<string> ALL_METRICS => new HashSet<string>
            {
                PublicMetrics,
            };

            public const string CreatedAt = "created_at";
            public const string Description = "description";
            public const string Entities = "entities";
            public const string Id = "id";
            public const string Location = "location";
            public const string Name = "name";
            public const string PinnedTweetId = "pinned_tweet_id";
            public const string ProfileImageUrl = "profile_image_url";
            public const string Protected = "protected";
            public const string PublicMetrics = "public_metrics";
            public const string Url = "url";
            public const string Username = "username";
            public const string Verified = "verified";
            public const string Withheld = "withheld";
        }
    }
}