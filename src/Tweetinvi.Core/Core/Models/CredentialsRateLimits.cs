using System;
using System.Collections.Generic;
using Tweetinvi.Core.Attributes;
using Tweetinvi.Core.DTO;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Models
{
    public class CredentialsRateLimits : ICredentialsRateLimits
    {
        public CredentialsRateLimitsDTO CredentialsRateLimitsDTO { get; }

        public CredentialsRateLimits(CredentialsRateLimitsDTO credentialsRateLimitsDTO)
        {
            CreatedAt = DateTime.Now;
            OtherEndpointRateLimits = new Dictionary<TwitterEndpointAttribute, IEndpointRateLimit>();
            CredentialsRateLimitsDTO = credentialsRateLimitsDTO;
        }

        public DateTime CreatedAt { get; }

        public string RateLimitContext
        {
            get
            {
                if (CredentialsRateLimitsDTO.RateLimitContext.TryGetValue("access_token", out var jsonObj))
                {
                    return jsonObj.ToObject<string>();
                }

                if (CredentialsRateLimitsDTO.RateLimitContext.TryGetValue("application", out jsonObj))
                {
                    return jsonObj.ToObject<string>();
                }

                return null;
            }
        }

        public bool IsApplicationOnlyCredentials => CredentialsRateLimitsDTO.RateLimitContext.ContainsKey("application");

        public Dictionary<TwitterEndpointAttribute, IEndpointRateLimit> OtherEndpointRateLimits { get; }

        // Account
        public IEndpointRateLimit AccountLoginVerificationEnrollmentLimit => CredentialsRateLimitsDTO?.Resources?.AccountRateLimits["/account/login_verification_enrollment"];
        public IEndpointRateLimit AccountSettingsLimit => CredentialsRateLimitsDTO?.Resources?.AccountRateLimits["/account/settings"];
        public IEndpointRateLimit AccountUpdateProfileLimit => CredentialsRateLimitsDTO?.Resources?.AccountRateLimits["/account/update_profile"];
        public IEndpointRateLimit AccountVerifyCredentialsLimit => CredentialsRateLimitsDTO?.Resources?.AccountRateLimits["/account/verify_credentials"];

        // Application
        public IEndpointRateLimit ApplicationRateLimitStatusLimit => CredentialsRateLimitsDTO?.Resources?.ApplicationRateLimits["/application/rate_limit_status"];

        // Auth
        public IEndpointRateLimit AuthCrossSiteRequestForgeryLimit => CredentialsRateLimitsDTO?.Resources?.AuthRateLimits["/auth/csrf_token"];

        // Block

        public IEndpointRateLimit BlocksIdsLimit => CredentialsRateLimitsDTO?.Resources?.BlocksRateLimits["/blocks/ids"];
        public IEndpointRateLimit BlocksListLimit => CredentialsRateLimitsDTO?.Resources?.BlocksRateLimits["/blocks/list"];

        // Business Experience
        public IEndpointRateLimit BusinessExperienceKeywordLimit => CredentialsRateLimitsDTO?.Resources?.BusinessExperienceRateLimits["/business_experience/keywords"];

        // Collections
        public IEndpointRateLimit CollectionsListLimit => CredentialsRateLimitsDTO?.Resources?.CollectionsRateLimits["/collections/list"];
        public IEndpointRateLimit CollectionsEntriesLimit => CredentialsRateLimitsDTO?.Resources?.CollectionsRateLimits["/collections/entries"];
        public IEndpointRateLimit CollectionsShowLimit => CredentialsRateLimitsDTO?.Resources?.CollectionsRateLimits["/collections/show"];

        // Contacts
        public IEndpointRateLimit ContactsUpdatedByLimit => CredentialsRateLimitsDTO?.Resources?.ContactsRateLimits["/contacts/uploaded_by"];
        public IEndpointRateLimit ContactsUsersLimit => CredentialsRateLimitsDTO?.Resources?.ContactsRateLimits["/contacts/users"];
        public IEndpointRateLimit ContactsAddressBookLimit => CredentialsRateLimitsDTO?.Resources?.ContactsRateLimits["/contacts/addressbook"];
        public IEndpointRateLimit ContactsUsersAndUploadedByLimit => CredentialsRateLimitsDTO?.Resources?.ContactsRateLimits["/contacts/users_and_uploaded_by"];
        public IEndpointRateLimit ContactsDeleteStatusLimit => CredentialsRateLimitsDTO?.Resources?.ContactsRateLimits["/contacts/delete/status"];

        // Device
        public IEndpointRateLimit DeviceTokenLimit => CredentialsRateLimitsDTO?.Resources?.DeviceRateLimits["/device/token"];

        // DirectMessages
        public IEndpointRateLimit DirectMessagesShowLimit => CredentialsRateLimitsDTO?.Resources?.DirectMessagesRateLimits["/direct_messages/events/show"];
        public IEndpointRateLimit DirectMessagesListLimit => CredentialsRateLimitsDTO?.Resources?.DirectMessagesRateLimits["/direct_messages/events/list"];

        // Favourites
        public IEndpointRateLimit FavoritesListLimit => CredentialsRateLimitsDTO?.Resources?.FavoritesRateLimits["/favorites/list"];

        // Feedback
        public IEndpointRateLimit FeedbackShowLimit => CredentialsRateLimitsDTO?.Resources?.FeedbackRateLimits["/feedback/show/:id"];
        public IEndpointRateLimit FeedbackEventsLimit => CredentialsRateLimitsDTO?.Resources?.FeedbackRateLimits["/feedback/events"];

        // Followers
        public IEndpointRateLimit FollowersIdsLimit => CredentialsRateLimitsDTO?.Resources?.FollowersRateLimits["/followers/ids"];
        public IEndpointRateLimit FollowersListLimit => CredentialsRateLimitsDTO?.Resources?.FollowersRateLimits["/followers/list"];

        // Friends
        public IEndpointRateLimit FriendsIdsLimit => CredentialsRateLimitsDTO?.Resources?.FriendsRateLimits["/friends/ids"];
        public IEndpointRateLimit FriendsListLimit => CredentialsRateLimitsDTO?.Resources?.FriendsRateLimits["/friends/list"];
        public IEndpointRateLimit FriendsFollowingIdsLimit => CredentialsRateLimitsDTO?.Resources?.FriendsRateLimits["/friends/following/ids"];
        public IEndpointRateLimit FriendsFollowingListLimit => CredentialsRateLimitsDTO?.Resources?.FriendsRateLimits["/friends/following/list"];

        // Friendships
        public IEndpointRateLimit FriendshipsIncomingLimit => CredentialsRateLimitsDTO?.Resources?.FriendshipsRateLimits["/friendships/incoming"];
        public IEndpointRateLimit FriendshipsLookupLimit => CredentialsRateLimitsDTO?.Resources?.FriendshipsRateLimits["/friendships/lookup"];
        public IEndpointRateLimit FriendshipsNoRetweetsIdsLimit => CredentialsRateLimitsDTO?.Resources?.FriendshipsRateLimits["/friendships/no_retweets/ids"];
        public IEndpointRateLimit FriendshipsOutgoingLimit => CredentialsRateLimitsDTO?.Resources?.FriendshipsRateLimits["/friendships/outgoing"];
        public IEndpointRateLimit FriendshipsShowLimit => CredentialsRateLimitsDTO?.Resources?.FriendshipsRateLimits["/friendships/show"];
        public IEndpointRateLimit FriendshipsListLimit => CredentialsRateLimitsDTO?.Resources?.FriendshipsRateLimits["/friendships/list"];

        // Geo
        public IEndpointRateLimit GeoGetPlaceFromIdLimit => CredentialsRateLimitsDTO?.Resources?.GeoRateLimits["/geo/id/:place_id"];
        public IEndpointRateLimit GeoReverseGeoCodeLimit => CredentialsRateLimitsDTO?.Resources?.GeoRateLimits["/geo/reverse_geocode"];
        public IEndpointRateLimit GeoSearchLimit => CredentialsRateLimitsDTO?.Resources?.GeoRateLimits["/geo/search"];
        public IEndpointRateLimit GeoSimilarPlacesLimit => CredentialsRateLimitsDTO?.Resources?.GeoRateLimits["/geo/similar_places"];

        // Help
        public IEndpointRateLimit HelpConfigurationLimit => CredentialsRateLimitsDTO?.Resources?.HelpRateLimits["/help/configuration"];
        public IEndpointRateLimit HelpLanguagesLimit => CredentialsRateLimitsDTO?.Resources?.HelpRateLimits["/help/languages"];
        public IEndpointRateLimit HelpPrivacyLimit => CredentialsRateLimitsDTO?.Resources?.HelpRateLimits["/help/privacy"];
        public IEndpointRateLimit HelpSettingsLimit => CredentialsRateLimitsDTO?.Resources?.HelpRateLimits["/help/settings"];
        public IEndpointRateLimit HelpTosLimit => CredentialsRateLimitsDTO?.Resources?.HelpRateLimits["/help/tos"];

        // Lists
        public IEndpointRateLimit ListsListLimit => CredentialsRateLimitsDTO?.Resources?.ListsRateLimits["/lists/list"];
        public IEndpointRateLimit ListsMembersLimit => CredentialsRateLimitsDTO?.Resources?.ListsRateLimits["/lists/members"];
        public IEndpointRateLimit ListsMembersShowLimit => CredentialsRateLimitsDTO?.Resources?.ListsRateLimits["/lists/members/show"];
        public IEndpointRateLimit ListsMembershipsLimit => CredentialsRateLimitsDTO?.Resources?.ListsRateLimits["/lists/memberships"];
        public IEndpointRateLimit ListsOwnershipsLimit => CredentialsRateLimitsDTO?.Resources?.ListsRateLimits["/lists/ownerships"];
        public IEndpointRateLimit ListsShowLimit => CredentialsRateLimitsDTO?.Resources?.ListsRateLimits["/lists/show"];
        public IEndpointRateLimit ListsStatusesLimit => CredentialsRateLimitsDTO?.Resources?.ListsRateLimits["/lists/statuses"];
        public IEndpointRateLimit ListsSubscribersLimit => CredentialsRateLimitsDTO?.Resources?.ListsRateLimits["/lists/subscribers"];
        public IEndpointRateLimit ListsSubscribersShowLimit => CredentialsRateLimitsDTO?.Resources?.ListsRateLimits["/lists/subscribers/show"];
        public IEndpointRateLimit ListsSubscriptionsLimit => CredentialsRateLimitsDTO?.Resources?.ListsRateLimits["/lists/subscriptions"];

        // Media
        public IEndpointRateLimit MediaUploadLimit => CredentialsRateLimitsDTO?.Resources?.MediaRateLimits["/media/upload"];

        // Moments
        public IEndpointRateLimit MomentsPermissions => CredentialsRateLimitsDTO?.Resources?.MomentsRateLimits["/moments/permissions"];

        // Mutes
        public IEndpointRateLimit MutesUserList => CredentialsRateLimitsDTO?.Resources?.MutesRateLimits["/mutes/users/list"];
        public IEndpointRateLimit MutesUserIds => CredentialsRateLimitsDTO?.Resources?.MutesRateLimits["/mutes/users/ids"];

        // SavedSearches
        public IEndpointRateLimit SavedSearchDestroyLimit => CredentialsRateLimitsDTO?.Resources?.SavedSearchesRateLimits["/saved_searches/destroy/:id"];
        public IEndpointRateLimit SavedSearchesListLimit => CredentialsRateLimitsDTO?.Resources?.SavedSearchesRateLimits["/saved_searches/list"];
        public IEndpointRateLimit SavedSearchesShowIdLimit => CredentialsRateLimitsDTO?.Resources?.SavedSearchesRateLimits["/saved_searches/show/:id"];

        // Search
        public IEndpointRateLimit SearchTweetsLimit => CredentialsRateLimitsDTO?.Resources?.SearchRateLimits["/search/tweets"];

        // Statuses
        public IEndpointRateLimit StatusesFriendsLimit => CredentialsRateLimitsDTO?.Resources?.StatusesRateLimits["/statuses/friends"];
        public IEndpointRateLimit StatusesHomeTimelineLimit => CredentialsRateLimitsDTO?.Resources?.StatusesRateLimits["/statuses/home_timeline"];
        public IEndpointRateLimit StatusesLookupLimit => CredentialsRateLimitsDTO?.Resources?.StatusesRateLimits["/statuses/lookup"];
        public IEndpointRateLimit StatusesMentionsTimelineLimit => CredentialsRateLimitsDTO?.Resources?.StatusesRateLimits["/statuses/mentions_timeline"];
        public IEndpointRateLimit StatusesOembedLimit => CredentialsRateLimitsDTO?.Resources?.StatusesRateLimits["/statuses/oembed"];
        public IEndpointRateLimit StatusesRetweetersIdsLimit => CredentialsRateLimitsDTO?.Resources?.StatusesRateLimits["/statuses/retweeters/ids"];
        public IEndpointRateLimit StatusesRetweetsIdLimit => CredentialsRateLimitsDTO?.Resources?.StatusesRateLimits["/statuses/retweets/:id"];
        public IEndpointRateLimit StatusesRetweetsOfMeLimit => CredentialsRateLimitsDTO?.Resources?.StatusesRateLimits["/statuses/retweets_of_me"];
        public IEndpointRateLimit StatusesShowIdLimit => CredentialsRateLimitsDTO?.Resources?.StatusesRateLimits["/statuses/show/:id"];
        public IEndpointRateLimit StatusesUserTimelineLimit => CredentialsRateLimitsDTO?.Resources?.StatusesRateLimits["/statuses/user_timeline"];

        // Trends
        public IEndpointRateLimit TrendsAvailableLimit => CredentialsRateLimitsDTO?.Resources?.TrendsRateLimits["/trends/available"];
        public IEndpointRateLimit TrendsClosestLimit => CredentialsRateLimitsDTO?.Resources?.TrendsRateLimits["/trends/closest"];
        public IEndpointRateLimit TrendsPlaceLimit => CredentialsRateLimitsDTO?.Resources?.TrendsRateLimits["/trends/place"];

        // Twitter Prompts
        public IEndpointRateLimit TweetPromptsReportInteractionLimit => CredentialsRateLimitsDTO?.Resources?.TweetPromptsRateLimits["/tweet_prompts/report_interaction"];
        public IEndpointRateLimit TweetPromptsShowLimit => CredentialsRateLimitsDTO?.Resources?.TweetPromptsRateLimits["/tweet_prompts/show"];

        // Users
        public IEndpointRateLimit UsersDerivedInfoLimit => CredentialsRateLimitsDTO?.Resources?.UsersRateLimits["/users/derived_info"];
        public IEndpointRateLimit UsersLookupLimit => CredentialsRateLimitsDTO?.Resources?.UsersRateLimits["/users/lookup"];
        public IEndpointRateLimit UsersProfileBannerLimit => CredentialsRateLimitsDTO?.Resources?.UsersRateLimits["/users/profile_banner"];
        public IEndpointRateLimit UsersReportSpamLimit => CredentialsRateLimitsDTO?.Resources?.UsersRateLimits["/users/report_spam"];
        public IEndpointRateLimit UsersSearchLimit => CredentialsRateLimitsDTO?.Resources?.UsersRateLimits["/users/search"];
        public IEndpointRateLimit UsersShowIdLimit => CredentialsRateLimitsDTO?.Resources?.UsersRateLimits["/users/show/:id"];
    }
}