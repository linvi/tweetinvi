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

        public DateTimeOffset CreatedAt { get; }

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
        public IEndpointRateLimit AccountLoginVerificationEnrollmentLimit => GetRateLimits(r => r.AccountRateLimits, "/account/login_verification_enrollment");
        public IEndpointRateLimit AccountSettingsLimit => GetRateLimits(r => r.AccountRateLimits, "/account/settings");
        public IEndpointRateLimit AccountUpdateProfileLimit => GetRateLimits(r => r.AccountRateLimits, "/account/update_profile");
        public IEndpointRateLimit AccountVerifyCredentialsLimit => GetRateLimits(r => r.AccountRateLimits, "/account/verify_credentials");

        // Application
        public IEndpointRateLimit ApplicationRateLimitStatusLimit => GetRateLimits(r => r.ApplicationRateLimits, "/application/rate_limit_status");

        // Auth
        public IEndpointRateLimit AuthCrossSiteRequestForgeryLimit => GetRateLimits(r => r.AuthRateLimits, "/auth/csrf_token");

        // Block

        public IEndpointRateLimit BlocksIdsLimit => GetRateLimits(r => r.BlocksRateLimits, "/blocks/ids");
        public IEndpointRateLimit BlocksListLimit => GetRateLimits(r => r.BlocksRateLimits, "/blocks/list");

        // Business Experience
        public IEndpointRateLimit BusinessExperienceKeywordLimit => GetRateLimits(r => r.BusinessExperienceRateLimits, "/business_experience/keywords");

        // Collections
        public IEndpointRateLimit CollectionsListLimit => GetRateLimits(r => r.CollectionsRateLimits, "/collections/list");
        public IEndpointRateLimit CollectionsEntriesLimit => GetRateLimits(r => r.CollectionsRateLimits, "/collections/entries");
        public IEndpointRateLimit CollectionsShowLimit => GetRateLimits(r => r.CollectionsRateLimits, "/collections/show");

        // Contacts
        public IEndpointRateLimit ContactsUpdatedByLimit => GetRateLimits(r => r.ContactsRateLimits, "/contacts/uploaded_by");
        public IEndpointRateLimit ContactsUsersLimit => GetRateLimits(r => r.ContactsRateLimits, "/contacts/users");
        public IEndpointRateLimit ContactsAddressBookLimit => GetRateLimits(r => r.ContactsRateLimits, "/contacts/addressbook");
        public IEndpointRateLimit ContactsUsersAndUploadedByLimit => GetRateLimits(r => r.ContactsRateLimits, "/contacts/users_and_uploaded_by");
        public IEndpointRateLimit ContactsDeleteStatusLimit => GetRateLimits(r => r.ContactsRateLimits, "/contacts/delete/status");

        // Device
        public IEndpointRateLimit DeviceTokenLimit => GetRateLimits(r => r.DeviceRateLimits, "/device/token");

        // DirectMessages
        public IEndpointRateLimit DirectMessagesShowLimit => GetRateLimits(r => r.DirectMessagesRateLimits, "/direct_messages/events/show");
        public IEndpointRateLimit DirectMessagesListLimit => GetRateLimits(r => r.DirectMessagesRateLimits, "/direct_messages/events/list");

        // Favorites
        public IEndpointRateLimit FavoritesListLimit => GetRateLimits(r => r.FavoritesRateLimits, "/favorites/list");

        // Feedback
        public IEndpointRateLimit FeedbackShowLimit => GetRateLimits(r => r.FeedbackRateLimits, "/feedback/show/:id");
        public IEndpointRateLimit FeedbackEventsLimit => GetRateLimits(r => r.FeedbackRateLimits, "/feedback/events");

        // Followers
        public IEndpointRateLimit FollowersIdsLimit => GetRateLimits(r => r.FollowersRateLimits, "/followers/ids");
        public IEndpointRateLimit FollowersListLimit => GetRateLimits(r => r.FollowersRateLimits, "/followers/list");

        // Friends
        public IEndpointRateLimit FriendsIdsLimit => GetRateLimits(r => r.FriendsRateLimits, "/friends/ids");
        public IEndpointRateLimit FriendsListLimit => GetRateLimits(r => r.FriendsRateLimits, "/friends/list");
        public IEndpointRateLimit FriendsFollowingIdsLimit => GetRateLimits(r => r.FriendsRateLimits, "/friends/following/ids");
        public IEndpointRateLimit FriendsFollowingListLimit => GetRateLimits(r => r.FriendsRateLimits, "/friends/following/list");

        // Friendships
        public IEndpointRateLimit FriendshipsIncomingLimit => GetRateLimits(r => r.FriendshipsRateLimits, "/friendships/incoming");
        public IEndpointRateLimit FriendshipsLookupLimit => GetRateLimits(r => r.FriendshipsRateLimits, "/friendships/lookup");
        public IEndpointRateLimit FriendshipsNoRetweetsIdsLimit => GetRateLimits(r => r.FriendshipsRateLimits, "/friendships/no_retweets/ids");
        public IEndpointRateLimit FriendshipsOutgoingLimit => GetRateLimits(r => r.FriendshipsRateLimits, "/friendships/outgoing");
        public IEndpointRateLimit FriendshipsShowLimit => GetRateLimits(r => r.FriendshipsRateLimits, "/friendships/show");
        public IEndpointRateLimit FriendshipsListLimit => GetRateLimits(r => r.FriendshipsRateLimits, "/friendships/list");

        // Geo
        public IEndpointRateLimit GeoGetPlaceFromIdLimit => GetRateLimits(r => r.GeoRateLimits, "/geo/id/:place_id");
        public IEndpointRateLimit GeoReverseGeoCodeLimit => GetRateLimits(r => r.GeoRateLimits, "/geo/reverse_geocode");
        public IEndpointRateLimit GeoSearchLimit => GetRateLimits(r => r.GeoRateLimits, "/geo/search");
        public IEndpointRateLimit GeoSimilarPlacesLimit => GetRateLimits(r => r.GeoRateLimits, "/geo/similar_places");

        // Help
        public IEndpointRateLimit HelpConfigurationLimit => GetRateLimits(r => r.HelpRateLimits, "/help/configuration");
        public IEndpointRateLimit HelpLanguagesLimit => GetRateLimits(r => r.HelpRateLimits, "/help/languages");
        public IEndpointRateLimit HelpPrivacyLimit => GetRateLimits(r => r.HelpRateLimits, "/help/privacy");
        public IEndpointRateLimit HelpSettingsLimit => GetRateLimits(r => r.HelpRateLimits, "/help/settings");
        public IEndpointRateLimit HelpTosLimit => GetRateLimits(r => r.HelpRateLimits, "/help/tos");

        // Lists
        public IEndpointRateLimit ListsListLimit => GetRateLimits(r => r.ListsRateLimits, "/lists/list");
        public IEndpointRateLimit ListsMembersLimit => GetRateLimits(r => r.ListsRateLimits, "/lists/members");
        public IEndpointRateLimit ListsMembersShowLimit => GetRateLimits(r => r.ListsRateLimits, "/lists/members/show");
        public IEndpointRateLimit ListsMembershipsLimit => GetRateLimits(r => r.ListsRateLimits, "/lists/memberships");
        public IEndpointRateLimit ListsOwnershipsLimit => GetRateLimits(r => r.ListsRateLimits, "/lists/ownerships");
        public IEndpointRateLimit ListsShowLimit => GetRateLimits(r => r.ListsRateLimits, "/lists/show");
        public IEndpointRateLimit ListsStatusesLimit => GetRateLimits(r => r.ListsRateLimits, "/lists/statuses");
        public IEndpointRateLimit ListsSubscribersLimit => GetRateLimits(r => r.ListsRateLimits, "/lists/subscribers");
        public IEndpointRateLimit ListsSubscribersShowLimit => GetRateLimits(r => r.ListsRateLimits, "/lists/subscribers/show");
        public IEndpointRateLimit ListsSubscriptionsLimit => GetRateLimits(r => r.ListsRateLimits, "/lists/subscriptions");

        // Media
        public IEndpointRateLimit MediaUploadLimit => GetRateLimits(r => r.MediaRateLimits, "/media/upload");

        // Moments
        public IEndpointRateLimit MomentsPermissions => GetRateLimits(r => r.MomentsRateLimits, "/moments/permissions");

        // Mutes
        public IEndpointRateLimit MutesUserList => GetRateLimits(r => r.MutesRateLimits, "/mutes/users/list");
        public IEndpointRateLimit MutesUserIds => GetRateLimits(r => r.MutesRateLimits, "/mutes/users/ids");

        // SavedSearches
        public IEndpointRateLimit SavedSearchDestroyLimit => GetRateLimits(r => r.SavedSearchesRateLimits, "/saved_searches/destroy/:id");
        public IEndpointRateLimit SavedSearchesListLimit => GetRateLimits(r => r.SavedSearchesRateLimits, "/saved_searches/list");
        public IEndpointRateLimit SavedSearchesShowIdLimit => GetRateLimits(r => r.SavedSearchesRateLimits, "/saved_searches/show/:id");

        // Search
        public IEndpointRateLimit SearchTweetsLimit => GetRateLimits(r => r.SearchRateLimits, "/search/tweets");

        // Statuses
        public IEndpointRateLimit StatusesFriendsLimit => GetRateLimits(r => r.StatusesRateLimits, "/statuses/friends");
        public IEndpointRateLimit StatusesHomeTimelineLimit => GetRateLimits(r => r.StatusesRateLimits, "/statuses/home_timeline");
        public IEndpointRateLimit StatusesLookupLimit => GetRateLimits(r => r.StatusesRateLimits, "/statuses/lookup");
        public IEndpointRateLimit StatusesMentionsTimelineLimit => GetRateLimits(r => r.StatusesRateLimits, "/statuses/mentions_timeline");
        public IEndpointRateLimit StatusesOembedLimit => GetRateLimits(r => r.StatusesRateLimits, "/statuses/oembed");
        public IEndpointRateLimit StatusesRetweetersIdsLimit => GetRateLimits(r => r.StatusesRateLimits, "/statuses/retweeters/ids");
        public IEndpointRateLimit StatusesRetweetsIdLimit => GetRateLimits(r => r.StatusesRateLimits, "/statuses/retweets/:id");
        public IEndpointRateLimit StatusesRetweetsOfMeLimit => GetRateLimits(r => r.StatusesRateLimits, "/statuses/retweets_of_me");
        public IEndpointRateLimit StatusesShowIdLimit => GetRateLimits(r => r.StatusesRateLimits, "/statuses/show/:id");
        public IEndpointRateLimit StatusesUserTimelineLimit => GetRateLimits(r => r.StatusesRateLimits, "/statuses/user_timeline");

        // Trends
        public IEndpointRateLimit TrendsAvailableLimit => GetRateLimits(r => r.TrendsRateLimits, "/trends/available");
        public IEndpointRateLimit TrendsClosestLimit => GetRateLimits(r => r.TrendsRateLimits, "/trends/closest");
        public IEndpointRateLimit TrendsPlaceLimit => GetRateLimits(r => r.TrendsRateLimits, "/trends/place");

        // Twitter Prompts
        public IEndpointRateLimit TweetPromptsReportInteractionLimit => GetRateLimits(r => r.TweetPromptsRateLimits, "/tweet_prompts/report_interaction");
        public IEndpointRateLimit TweetPromptsShowLimit => GetRateLimits(r => r.TweetPromptsRateLimits, "/tweet_prompts/show");

        // Users
        public IEndpointRateLimit UsersDerivedInfoLimit => GetRateLimits(r => r.UsersRateLimits, "/users/derived_info");
        public IEndpointRateLimit UsersLookupLimit => GetRateLimits(r => r.UsersRateLimits, "/users/lookup");
        public IEndpointRateLimit UsersProfileBannerLimit => GetRateLimits(r => r.UsersRateLimits, "/users/profile_banner");
        public IEndpointRateLimit UsersReportSpamLimit => GetRateLimits(r => r.UsersRateLimits, "/users/report_spam");
        public IEndpointRateLimit UsersSearchLimit => GetRateLimits(r => r.UsersRateLimits, "/users/search");
        public IEndpointRateLimit UsersShowIdLimit => GetRateLimits(r => r.UsersRateLimits, "/users/show/:id");

        private IEndpointRateLimit GetRateLimits(Func<CredentialsRateLimitsDTO.RateLimitResources, Dictionary<string, IEndpointRateLimit>> getResources, string key)
        {
            var resource = getResources(CredentialsRateLimitsDTO?.Resources);
            if (resource == null)
            {
                return null;
            }

            if (!resource.TryGetValue(key, out var rateLimit))
            {
                return null;
            }

            return rateLimit;
        }
    }
}