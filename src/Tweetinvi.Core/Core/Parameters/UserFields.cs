using System.Collections.Generic;

namespace Tweetinvi.Core.Parameters
{
    public class UserFields
    {
        public HashSet<string> ALL => new HashSet<string>
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
            Withheld,

            PublicMetrics,
        };

        public readonly string CreatedAt = "created_at";
        public readonly string Description = "description";
        public readonly string Entities = "entities";
        public readonly string Id = "id";
        public readonly string Location = "location";
        public readonly string Name = "name";
        public readonly string PinnedTweetId = "pinned_tweet_id";
        public readonly string ProfileImageUrl = "profile_image_url";
        public readonly string Protected = "protected";
        public readonly string PublicMetrics = "public_metrics";
        public readonly string Url = "url";
        public readonly string Username = "username";
        public readonly string Verified = "verified";
        public readonly string Withheld = "withheld";
    }
}