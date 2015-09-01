using System;
using Tweetinvi.Core.Attributes;

namespace Tweetinvi.Core.Interfaces.Credentials
{
    /// <summary>
    /// Lists of Rate Limits provided by Twitter API 1.1
    /// https://dev.twitter.com/docs/rate-limiting/1.1/limits
    /// </summary>
    public interface ITokenRateLimits
    {
        // TODO LINVI : 
        // ADD https://dev.twitter.com/rest/reference/post/statuses/destroy/%3Aid
        // ADD https://api.twitter.com/1.1/statuses/update.json
        // ADD https://api.twitter.com/1.1/direct_messages/destroy.json
        // ADD https://api.twitter.com/1.1/direct_messages/new.json
        // ADD https://api.twitter.com/1.1/friendships/create.json
        // ADD https://api.twitter.com/1.1/friendships/destroy.json
        // ADD https://api.twitter.com/1.1/friendships/update.json

        DateTime CreatedAt { get; }
        string RateLimitContext { get; }
        bool IsApplicationOnlyCredentials { get; set; }

        // ACCOUNT
        ITokenRateLimit AccountLoginVerificationEnrollmentLimit { get; }
        
        [TwitterEndpoint("https://api.twitter.com/1.1/account/settings.json")]
        ITokenRateLimit AccountSettingsLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/account/update_profile.json")]
        ITokenRateLimit AccountUpdateProfileLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/account/verify_credentials.json")]
        ITokenRateLimit AccountVerifyCredentialsLimit { get; }

        // APPLICATION
        [TwitterEndpoint("https://api.twitter.com/1.1/application/rate_limit_status.json")]
        ITokenRateLimit ApplicationRateLimitStatusLimit { get; }

        // BLOCK
        [TwitterEndpoint("https://api.twitter.com/1.1/blocks/ids.json")]
        ITokenRateLimit BlocksIdsLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/blocks/list.json")]
        ITokenRateLimit BlocksListLimit { get; }

        // OTHER
        ITokenRateLimit DeviceTokenLimit { get; }

        // DIRECT MESSAGES
        [TwitterEndpoint("https://api.twitter.com/1.1/direct_messages.json")]
        ITokenRateLimit DirectMessagesLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/direct_messages/sent.json")]
        ITokenRateLimit DirectMessagesSentLimit { get; }

        // TODO (LINVI) : FIND OUT THE QUERY IT IS RELATED WITH
        ITokenRateLimit DirectMessagesSentAndReceivedLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/direct_messages/show.json")]
        ITokenRateLimit DirectMessagesShowLimit { get; }

        // FAVOURITES
        [TwitterEndpoint("https://api.twitter.com/1.1/favorites/list.json")]
        ITokenRateLimit FavoritesListLimit { get; }

        // FOLLOWERS
        [TwitterEndpoint("https://api.twitter.com/1.1/followers/ids.json")]
        ITokenRateLimit FollowersIdsLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/followers/list.json")]
        ITokenRateLimit FollowersListLimit { get; }

        // FRIENDS
        [TwitterEndpoint("https://api.twitter.com/1.1/friends/ids.json")]
        ITokenRateLimit FriendsIdsLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/friends/list.json")]
        ITokenRateLimit FriendsListLimit { get; }
        ITokenRateLimit FriendsFollowingIdsLimit { get; }
        ITokenRateLimit FriendsFollowingListLimit { get; }

        // FRIENDSHIP
        [TwitterEndpoint("https://api.twitter.com/1.1/friendships/incoming.json")]
        ITokenRateLimit FriendshipsIncomingLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/friendships/lookup.json")]
        ITokenRateLimit FriendshipsLookupLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/friendships/no_retweets/ids.json")]
        ITokenRateLimit FriendshipsNoRetweetsIdsLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/friendships/outgoing.json")]
        ITokenRateLimit FriendshipsOutgoingLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/friendships/show.json")]
        ITokenRateLimit FriendshipsShowLimit { get; }

        // GEO
        [TwitterEndpoint("https://api.twitter.com/1.1/geo/id/[a-zA-Z0-9]+\\.json", true)]
        ITokenRateLimit GeoGetPlaceFromIdLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/geo/reverse_geocode.json")]
        ITokenRateLimit GeoReverseGeoCodeLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/geo/search.json")]
        ITokenRateLimit GeoSearchLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/geo/similar_places.json")]
        ITokenRateLimit GeoSimilarPlacesLimit { get; }

        // HELP
        [TwitterEndpoint("https://api.twitter.com/1.1/help/configuration.json")]
        ITokenRateLimit HelpConfigurationLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/help/languages.json")]
        ITokenRateLimit HelpLanguagesLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/help/privacy.json")]
        ITokenRateLimit HelpPrivacyLimit { get; }

        ITokenRateLimit HelpSettingsLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/help/tos.json")]
        ITokenRateLimit HelpTosLimit { get; }

        // LIST
        [TwitterEndpoint("https://api.twitter.com/1.1/lists/list.json")]
        ITokenRateLimit ListsListLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/lists/members.json")]
        ITokenRateLimit ListsMembersLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/lists/members/show.json")]
        ITokenRateLimit ListsMembersShowLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/lists/memberships.json")]
        ITokenRateLimit ListsMembershipsLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/lists/ownerships.json")]
        ITokenRateLimit ListsOwnershipsLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/lists/show.json")]
        ITokenRateLimit ListsShowLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/lists/statuses.json")]
        ITokenRateLimit ListsStatusesLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/lists/subscribers.json")]
        ITokenRateLimit ListsSubscribersLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/lists/subscribers/show.json")]
        ITokenRateLimit ListsSubscribersShowLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/lists/subscriptions.json")]
        ITokenRateLimit ListsSubscriptionsLimit { get; }

        // MUTES
        [TwitterEndpoint("https://api.twitter.com/1.1/mutes/users/list.json")]
        ITokenRateLimit MutesUserList { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/mutes/users/ids.json")]
        ITokenRateLimit MutesUserIds { get; }

        // SAVED SEARCHES
        [TwitterEndpoint("https://api.twitter.com/1.1/saved_searches/destroy/[a-zA-Z0-9]+\\.json", true)]
        ITokenRateLimit SavedSearchDestroyList { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/saved_searches/list.json")]
        ITokenRateLimit SavedSearchesListLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/saved_searches/show/[a-zA-Z0-9]+\\.json", true)]
        ITokenRateLimit SavedSearchesShowIdLimit { get; }

        // SEARCH
        [TwitterEndpoint("https://api.twitter.com/1.1/search/tweets.json")]
        ITokenRateLimit SearchTweetsLimit { get; }

        // STATUSES
        // TODO LINVI
        ITokenRateLimit StatusesFriendsLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/statuses/home_timeline.json")]
        ITokenRateLimit StatusesHomeTimelineLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/statuses/lookup.json")]
        ITokenRateLimit StatusesLookupLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/statuses/mentions_timeline.json")]
        ITokenRateLimit StatusesMentionsTimelineLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/statuses/oembed.json")]
        ITokenRateLimit StatusesOembedLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/statuses/retweeters/ids.json")]
        ITokenRateLimit StatusesRetweetersIdsLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/statuses/retweets/[0-9]+\\.json", true)]
        ITokenRateLimit StatusesRetweetsIdLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/statuses/retweets_of_me.json")]
        ITokenRateLimit StatusesRetweetsOfMeLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/statuses/show.json")]
        ITokenRateLimit StatusesShowIdLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/statuses/user_timeline.json")]
        ITokenRateLimit StatusesUserTimelineLimit { get; }

        // TRENDS
        [TwitterEndpoint("https://api.twitter.com/1.1/trends/available.json")]
        ITokenRateLimit TrendsAvailableLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/trends/closest.json")]
        ITokenRateLimit TrendsClosestLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/trends/place.json")]
        ITokenRateLimit TrendsPlaceLimit { get; }

        // USER
        ITokenRateLimit UsersDerivedInfoLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/users/lookup.json")]
        ITokenRateLimit UsersLookupLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/users/profile_banner.json")]
        ITokenRateLimit UsersProfileBannerLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/users/report_spam.json")]
        ITokenRateLimit UsersReportSpamLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/users/search.json")]
        ITokenRateLimit UsersSearchLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/users/show.json")]
        ITokenRateLimit UsersShowIdLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/users/suggestions.json")]
        ITokenRateLimit UsersSuggestionsLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/users/suggestions/[a-zA-Z0-9]+\\.json", true)]
        ITokenRateLimit UsersSuggestionsSlugLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/users/suggestions/[a-zA-Z0-9]+/members.json", true)]
        ITokenRateLimit UsersSuggestionsSlugMembersLimit { get; }
    }
}