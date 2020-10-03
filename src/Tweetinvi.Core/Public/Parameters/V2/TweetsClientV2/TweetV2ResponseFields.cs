using System.Collections.Generic;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Parameters.V2
{
    public static class TweetV2ResponseFields
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

        public static readonly TweetFields Tweet = new TweetFields();
        public static readonly UserFields User = new UserFields();
    }
}