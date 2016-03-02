using System;
using Tweetinvi.Core.Attributes;

namespace Tweetinvi.Core.Interfaces.Credentials
{
    /// <summary>
    /// Lists the state of all the twitter api endpoints rate limits.
    /// https://dev.twitter.com/docs/rate-limiting/1.1/limits
    /// </summary>
    public interface ICredentialsRateLimits
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
        IEndpointRateLimit AccountLoginVerificationEnrollmentLimit { get; }
        
        [TwitterEndpoint("https://api.twitter.com/1.1/account/settings.json")]
        IEndpointRateLimit AccountSettingsLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/account/update_profile.json")]
        IEndpointRateLimit AccountUpdateProfileLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/account/verify_credentials.json")]
        IEndpointRateLimit AccountVerifyCredentialsLimit { get; }

        // APPLICATION
        [TwitterEndpoint("https://api.twitter.com/1.1/application/rate_limit_status.json")]
        IEndpointRateLimit ApplicationRateLimitStatusLimit { get; }

        // BLOCK
        [TwitterEndpoint("https://api.twitter.com/1.1/blocks/ids.json")]
        IEndpointRateLimit BlocksIdsLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/blocks/list.json")]
        IEndpointRateLimit BlocksListLimit { get; }

        // OTHER
        IEndpointRateLimit DeviceTokenLimit { get; }

        // DIRECT MESSAGES
        [TwitterEndpoint("https://api.twitter.com/1.1/direct_messages.json")]
        IEndpointRateLimit DirectMessagesLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/direct_messages/sent.json")]
        IEndpointRateLimit DirectMessagesSentLimit { get; }

        // TODO (LINVI) : FIND OUT THE QUERY IT IS RELATED WITH
        IEndpointRateLimit DirectMessagesSentAndReceivedLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/direct_messages/show.json")]
        IEndpointRateLimit DirectMessagesShowLimit { get; }

        // FAVOURITES
        [TwitterEndpoint("https://api.twitter.com/1.1/favorites/list.json")]
        IEndpointRateLimit FavoritesListLimit { get; }

        // FOLLOWERS
        [TwitterEndpoint("https://api.twitter.com/1.1/followers/ids.json")]
        IEndpointRateLimit FollowersIdsLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/followers/list.json")]
        IEndpointRateLimit FollowersListLimit { get; }

        // FRIENDS
        [TwitterEndpoint("https://api.twitter.com/1.1/friends/ids.json")]
        IEndpointRateLimit FriendsIdsLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/friends/list.json")]
        IEndpointRateLimit FriendsListLimit { get; }
        IEndpointRateLimit FriendsFollowingIdsLimit { get; }
        IEndpointRateLimit FriendsFollowingListLimit { get; }

        // FRIENDSHIP
        [TwitterEndpoint("https://api.twitter.com/1.1/friendships/incoming.json")]
        IEndpointRateLimit FriendshipsIncomingLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/friendships/lookup.json")]
        IEndpointRateLimit FriendshipsLookupLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/friendships/no_retweets/ids.json")]
        IEndpointRateLimit FriendshipsNoRetweetsIdsLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/friendships/outgoing.json")]
        IEndpointRateLimit FriendshipsOutgoingLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/friendships/show.json")]
        IEndpointRateLimit FriendshipsShowLimit { get; }

        // GEO
        [TwitterEndpoint("https://api.twitter.com/1.1/geo/id/[a-zA-Z0-9]+\\.json", true)]
        IEndpointRateLimit GeoGetPlaceFromIdLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/geo/reverse_geocode.json")]
        IEndpointRateLimit GeoReverseGeoCodeLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/geo/search.json")]
        IEndpointRateLimit GeoSearchLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/geo/similar_places.json")]
        IEndpointRateLimit GeoSimilarPlacesLimit { get; }

        // HELP
        [TwitterEndpoint("https://api.twitter.com/1.1/help/configuration.json")]
        IEndpointRateLimit HelpConfigurationLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/help/languages.json")]
        IEndpointRateLimit HelpLanguagesLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/help/privacy.json")]
        IEndpointRateLimit HelpPrivacyLimit { get; }

        IEndpointRateLimit HelpSettingsLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/help/tos.json")]
        IEndpointRateLimit HelpTosLimit { get; }

        // LIST
        [TwitterEndpoint("https://api.twitter.com/1.1/lists/list.json")]
        IEndpointRateLimit ListsListLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/lists/members.json")]
        IEndpointRateLimit ListsMembersLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/lists/members/show.json")]
        IEndpointRateLimit ListsMembersShowLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/lists/memberships.json")]
        IEndpointRateLimit ListsMembershipsLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/lists/ownerships.json")]
        IEndpointRateLimit ListsOwnershipsLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/lists/show.json")]
        IEndpointRateLimit ListsShowLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/lists/statuses.json")]
        IEndpointRateLimit ListsStatusesLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/lists/subscribers.json")]
        IEndpointRateLimit ListsSubscribersLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/lists/subscribers/show.json")]
        IEndpointRateLimit ListsSubscribersShowLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/lists/subscriptions.json")]
        IEndpointRateLimit ListsSubscriptionsLimit { get; }

        // MUTES
        [TwitterEndpoint("https://api.twitter.com/1.1/mutes/users/list.json")]
        IEndpointRateLimit MutesUserList { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/mutes/users/ids.json")]
        IEndpointRateLimit MutesUserIds { get; }

        // SAVED SEARCHES
        [TwitterEndpoint("https://api.twitter.com/1.1/saved_searches/destroy/[a-zA-Z0-9]+\\.json", true)]
        IEndpointRateLimit SavedSearchDestroyList { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/saved_searches/list.json")]
        IEndpointRateLimit SavedSearchesListLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/saved_searches/show/[a-zA-Z0-9]+\\.json", true)]
        IEndpointRateLimit SavedSearchesShowIdLimit { get; }

        // SEARCH
        [TwitterEndpoint("https://api.twitter.com/1.1/search/tweets.json")]
        IEndpointRateLimit SearchTweetsLimit { get; }

        // STATUSES
        // TODO LINVI
        IEndpointRateLimit StatusesFriendsLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/statuses/home_timeline.json")]
        IEndpointRateLimit StatusesHomeTimelineLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/statuses/lookup.json")]
        IEndpointRateLimit StatusesLookupLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/statuses/mentions_timeline.json")]
        IEndpointRateLimit StatusesMentionsTimelineLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/statuses/oembed.json")]
        IEndpointRateLimit StatusesOembedLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/statuses/retweeters/ids.json")]
        IEndpointRateLimit StatusesRetweetersIdsLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/statuses/retweets/[0-9]+\\.json", true)]
        IEndpointRateLimit StatusesRetweetsIdLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/statuses/retweets_of_me.json")]
        IEndpointRateLimit StatusesRetweetsOfMeLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/statuses/show.json")]
        IEndpointRateLimit StatusesShowIdLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/statuses/user_timeline.json")]
        IEndpointRateLimit StatusesUserTimelineLimit { get; }

        // TRENDS
        [TwitterEndpoint("https://api.twitter.com/1.1/trends/available.json")]
        IEndpointRateLimit TrendsAvailableLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/trends/closest.json")]
        IEndpointRateLimit TrendsClosestLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/trends/place.json")]
        IEndpointRateLimit TrendsPlaceLimit { get; }

        // USER
        IEndpointRateLimit UsersDerivedInfoLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/users/lookup.json")]
        IEndpointRateLimit UsersLookupLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/users/profile_banner.json")]
        IEndpointRateLimit UsersProfileBannerLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/users/report_spam.json")]
        IEndpointRateLimit UsersReportSpamLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/users/search.json")]
        IEndpointRateLimit UsersSearchLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/users/show.json")]
        IEndpointRateLimit UsersShowIdLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/users/suggestions.json")]
        IEndpointRateLimit UsersSuggestionsLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/users/suggestions/[a-zA-Z0-9]+\\.json", true)]
        IEndpointRateLimit UsersSuggestionsSlugLimit { get; }

        [TwitterEndpoint("https://api.twitter.com/1.1/users/suggestions/[a-zA-Z0-9]+/members.json", true)]
        IEndpointRateLimit UsersSuggestionsSlugMembersLimit { get; }
    }
}