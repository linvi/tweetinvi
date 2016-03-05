using System;
using System.Collections.Generic;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Entities;

namespace Tweetinvi.Core.Interfaces.DTO
{
    public interface ITweetDTO : ITweetIdentifier
    {
        bool IsTweetPublished { get; set; }

        bool IsTweetDestroyed { get; set; }

        string Text { get; set; }

        bool Favorited { get; set; }

        int FavoriteCount { get; set; }

        IUserDTO CreatedBy { get; set; }

        ITweetIdentifier CurrentUserRetweetIdentifier { get; set; }

        ICoordinates Coordinates { get; set; }

        ITweetEntities Entities { get; set; }

        ITweetEntities LegacyEntities { get; set; }

        DateTime CreatedAt { get; set; }

        bool Truncated { get; set; }

        long? InReplyToStatusId { get; set; }

        string InReplyToStatusIdStr { get; set; }

        long? InReplyToUserId { get; set; }

        string InReplyToUserIdStr { get; set; }

        string InReplyToScreenName { get; set; }

        int RetweetCount { get; set; }

        bool Retweeted { get; set; }

        ITweetDTO RetweetedTweetDTO { get; set; }

        long? QuotedStatusId { get; set; }

        string QuotedStatusIdStr { get; set; }

        ITweetDTO QuotedTweetDTO { get; set; }

        Language Language { get; set; }

        bool PossiblySensitive { get; set; }

        int[] ContributorsIds { get; set; }

        IEnumerable<long> Contributors { get; set; }

        string Source { get; set; }

        Dictionary<string, object> Scopes { get; set; }

        string FilterLevel { get; set; }

        bool WithheldCopyright { get; set; }

        IEnumerable<string> WithheldInCountries { get; set; }

        string WithheldScope { get; set; }

        IPlace Place { get; set; }
    }
}