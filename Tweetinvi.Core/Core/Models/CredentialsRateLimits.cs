using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core.Attributes;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Models
{
    public class CredentialsRateLimits : ICredentialsRateLimits
    {
        public CredentialsRateLimits()
        {
            CreatedAt = DateTime.Now;
            OtherEndpointRateLimits = new Dictionary<TwitterEndpointAttribute, IEndpointRateLimit>();
        }

        public DateTime CreatedAt { get; }

        public string RateLimitContext { get; set; }
        public bool IsApplicationOnlyCredentials { get; set; }

        public Dictionary<TwitterEndpointAttribute, IEndpointRateLimit> OtherEndpointRateLimits { get; }

        // Account
        public IEndpointRateLimit AccountLoginVerificationEnrollmentLimit => _resources.AccountRateLimits["/account/login_verification_enrollment"];
        public IEndpointRateLimit AccountSettingsLimit => _resources.AccountRateLimits["/account/settings"];
        public IEndpointRateLimit AccountUpdateProfileLimit => _resources.AccountRateLimits["/account/update_profile"];
        public IEndpointRateLimit AccountVerifyCredentialsLimit => _resources.AccountRateLimits["/account/verify_credentials"];

        // Application
        public IEndpointRateLimit ApplicationRateLimitStatusLimit => _resources.ApplicationRateLimits["/application/rate_limit_status"];

        // Auth
        public IEndpointRateLimit AuthCrossSiteRequestForgeryLimit => _resources.AuthRateLimits["/auth/csrf_token"];

        // Block
        public IEndpointRateLimit BlocksIdsLimit => _resources.BlocksRateLimits["/blocks/ids"];
        public IEndpointRateLimit BlocksListLimit => _resources.BlocksRateLimits["/blocks/list"];

        // Business Experience
        public IEndpointRateLimit BusinessExperienceKeywordLimit => _resources.BusinessExperienceRateLimits["/business_experience/keywords"];

        // Collections
        public IEndpointRateLimit CollectionsListLimit => _resources.CollectionsRateLimits["/collections/list"];
        public IEndpointRateLimit CollectionsEntriesLimit => _resources.CollectionsRateLimits["/collections/entries"];
        public IEndpointRateLimit CollectionsShowLimit => _resources.CollectionsRateLimits["/collections/show"];

        // Contacts
        public IEndpointRateLimit ContactsUpdatedByLimit => _resources.ContactsRateLimits["/contacts/uploaded_by"];
        public IEndpointRateLimit ContactsUsersLimit => _resources.ContactsRateLimits["/contacts/users"];
        public IEndpointRateLimit ContactsAddressBookLimit => _resources.ContactsRateLimits["/contacts/addressbook"];
        public IEndpointRateLimit ContactsUsersAndUploadedByLimit => _resources.ContactsRateLimits["/contacts/users_and_uploaded_by"];
        public IEndpointRateLimit ContactsDeleteStatusLimit => _resources.ContactsRateLimits["/contacts/delete/status"];

        // Device
        public IEndpointRateLimit DeviceTokenLimit => _resources.DeviceRateLimits["/device/token"];

        // DirectMessages
        public IEndpointRateLimit DirectMessagesShowLimit => _resources.DirectMessagesRateLimits["/direct_messages/events/show"];

        public IEndpointRateLimit DirectMessagesListLimit => _resources.DirectMessagesRateLimits["/direct_messages/events/list"];

        // Favourites
        public IEndpointRateLimit FavoritesListLimit => _resources.FavoritesRateLimits["/favorites/list"];

        // Feedback
        public IEndpointRateLimit FeedbackShowLimit => _resources.FeedbacksRateLimits["/feedback/show/:id"];
        public IEndpointRateLimit FeedbackEventsLimit => _resources.FeedbacksRateLimits["/feedback/events"];


        // Followers
        public IEndpointRateLimit FollowersIdsLimit => _resources.FollowersRateLimits["/followers/ids"];
        public IEndpointRateLimit FollowersListLimit => _resources.FollowersRateLimits["/followers/list"];

        // Friends
        public IEndpointRateLimit FriendsIdsLimit => _resources.FriendsRateLimits["/friends/ids"];
        public IEndpointRateLimit FriendsListLimit => _resources.FriendsRateLimits["/friends/list"];
        public IEndpointRateLimit FriendsFollowingIdsLimit => _resources.FriendsRateLimits["/friends/following/ids"];
        public IEndpointRateLimit FriendsFollowingListLimit => _resources.FriendsRateLimits["/friends/following/list"];

        // Friendships
        public IEndpointRateLimit FriendshipsIncomingLimit => _resources.FriendshipsRateLimits["/friendships/incoming"];
        public IEndpointRateLimit FriendshipsLookupLimit => _resources.FriendshipsRateLimits["/friendships/lookup"];
        public IEndpointRateLimit FriendshipsNoRetweetsIdsLimit => _resources.FriendshipsRateLimits["/friendships/no_retweets/ids"];
        public IEndpointRateLimit FriendshipsOutgoingLimit => _resources.FriendshipsRateLimits["/friendships/outgoing"];
        public IEndpointRateLimit FriendshipsShowLimit => _resources.FriendshipsRateLimits["/friendships/show"];
        public IEndpointRateLimit FriendshipsListLimit => _resources.FriendshipsRateLimits["/friendships/list"];


        // Geo
        public IEndpointRateLimit GeoGetPlaceFromIdLimit => _resources.GeoRateLimits["/geo/id/:place_id"];
        public IEndpointRateLimit GeoReverseGeoCodeLimit => _resources.GeoRateLimits["/geo/reverse_geocode"];
        public IEndpointRateLimit GeoSearchLimit => _resources.GeoRateLimits["/geo/search"];
        public IEndpointRateLimit GeoSimilarPlacesLimit => _resources.GeoRateLimits["/geo/similar_places"];

        // Help
        public IEndpointRateLimit HelpConfigurationLimit => _resources.HelpRateLimits["/help/configuration"];
        public IEndpointRateLimit HelpLanguagesLimit => _resources.HelpRateLimits["/help/languages"];
        public IEndpointRateLimit HelpPrivacyLimit => _resources.HelpRateLimits["/help/privacy"];
        public IEndpointRateLimit HelpSettingsLimit => _resources.HelpRateLimits["/help/settings"];
        public IEndpointRateLimit HelpTosLimit => _resources.HelpRateLimits["/help/tos"];

        // Lists
        public IEndpointRateLimit ListsListLimit => _resources.ListsRateLimits["/lists/list"];
        public IEndpointRateLimit ListsMembersLimit => _resources.ListsRateLimits["/lists/members"];
        public IEndpointRateLimit ListsMembersShowLimit => _resources.ListsRateLimits["/lists/members/show"];
        public IEndpointRateLimit ListsMembershipsLimit => _resources.ListsRateLimits["/lists/memberships"];
        public IEndpointRateLimit ListsOwnershipsLimit => _resources.ListsRateLimits["/lists/ownerships"];
        public IEndpointRateLimit ListsShowLimit => _resources.ListsRateLimits["/lists/show"];
        public IEndpointRateLimit ListsStatusesLimit => _resources.ListsRateLimits["/lists/statuses"];
        public IEndpointRateLimit ListsSubscribersLimit => _resources.ListsRateLimits["/lists/subscribers"];
        public IEndpointRateLimit ListsSubscribersShowLimit => _resources.ListsRateLimits["/lists/subscribers/show"];
        public IEndpointRateLimit ListsSubscriptionsLimit => _resources.ListsRateLimits["/lists/subscriptions"];

        // Media
        public IEndpointRateLimit MediaUploadLimit => _resources.MediaRateLimits["/media/upload"];

        // Moments
        public IEndpointRateLimit MomentsPermissions => _resources.MomentsRateLimits["/moments/permissions"];

        // Mutes
        public IEndpointRateLimit MutesUserList => _resources.MutesRateLimits["/mutes/users/list"];
        public IEndpointRateLimit MutesUserIds => _resources.MutesRateLimits["/mutes/users/ids"];

        // SavedSearches
        public IEndpointRateLimit SavedSearchDestroyLimit => _resources.SavedSearchesRateLimits["/saved_searches/destroy/:id"];
        public IEndpointRateLimit SavedSearchesListLimit => _resources.SavedSearchesRateLimits["/saved_searches/list"];
        public IEndpointRateLimit SavedSearchesShowIdLimit => _resources.SavedSearchesRateLimits["/saved_searches/show/:id"];

        // Search
        public IEndpointRateLimit SearchTweetsLimit => _resources.SearchRateLimits["/search/tweets"];

        // Statuses
        public IEndpointRateLimit StatusesFriendsLimit => _resources.StatusesRateLimits["/statuses/friends"];
        public IEndpointRateLimit StatusesHomeTimelineLimit => _resources.StatusesRateLimits["/statuses/home_timeline"];
        public IEndpointRateLimit StatusesLookupLimit => _resources.StatusesRateLimits["/statuses/lookup"];
        public IEndpointRateLimit StatusesMentionsTimelineLimit => _resources.StatusesRateLimits["/statuses/mentions_timeline"];
        public IEndpointRateLimit StatusesOembedLimit => _resources.StatusesRateLimits["/statuses/oembed"];
        public IEndpointRateLimit StatusesRetweetersIdsLimit => _resources.StatusesRateLimits["/statuses/retweeters/ids"];
        public IEndpointRateLimit StatusesRetweetsIdLimit => _resources.StatusesRateLimits["/statuses/retweets/:id"];
        public IEndpointRateLimit StatusesRetweetsOfMeLimit => _resources.StatusesRateLimits["/statuses/retweets_of_me"];
        public IEndpointRateLimit StatusesShowIdLimit => _resources.StatusesRateLimits["/statuses/show/:id"];
        public IEndpointRateLimit StatusesUserTimelineLimit => _resources.StatusesRateLimits["/statuses/user_timeline"];

        // Trends
        public IEndpointRateLimit TrendsAvailableLimit => _resources.TrendsRateLimits["/trends/available"];
        public IEndpointRateLimit TrendsClosestLimit => _resources.TrendsRateLimits["/trends/closest"];
        public IEndpointRateLimit TrendsPlaceLimit => _resources.TrendsRateLimits["/trends/place"];

        // Twitter Prompts

        public IEndpointRateLimit TweetPromptsReportInteractionLimit => _resources.TweetPromptsRateLimits["/tweet_prompts/report_interaction"];
        public IEndpointRateLimit TweetPromptsShowLimit => _resources.TweetPromptsRateLimits["/tweet_prompts/show"];

        // Users
        public IEndpointRateLimit UsersDerivedInfoLimit => _resources.UsersRateLimits["/users/derived_info"];
        public IEndpointRateLimit UsersLookupLimit => _resources.UsersRateLimits["/users/lookup"];
        public IEndpointRateLimit UsersProfileBannerLimit => _resources.UsersRateLimits["/users/profile_banner"];
        public IEndpointRateLimit UsersReportSpamLimit => _resources.UsersRateLimits["/users/report_spam"];
        public IEndpointRateLimit UsersSearchLimit => _resources.UsersRateLimits["/users/search"];
        public IEndpointRateLimit UsersShowIdLimit => _resources.UsersRateLimits["/users/show/:id"];
        public IEndpointRateLimit UsersSuggestionsLimit => _resources.UsersRateLimits["/users/suggestions"];
        public IEndpointRateLimit UsersSuggestionsSlugLimit => _resources.UsersRateLimits["/users/suggestions/:slug"];
        public IEndpointRateLimit UsersSuggestionsSlugMembersLimit => _resources.UsersRateLimits["/users/suggestions/:slug/members"];

        [SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
        private class RateLimitResources
        {
            [JsonProperty("account")] public Dictionary<string, IEndpointRateLimit> AccountRateLimits { get; set; }
            [JsonProperty("application")] public Dictionary<string, IEndpointRateLimit> ApplicationRateLimits { get; set; }
            [JsonProperty("auth")] public Dictionary<string, IEndpointRateLimit> AuthRateLimits { get; set; }
            [JsonProperty("blocks")] public Dictionary<string, IEndpointRateLimit> BlocksRateLimits { get; set; }
            [JsonProperty("business_experience")] public Dictionary<string, IEndpointRateLimit> BusinessExperienceRateLimits { get; set; }
            [JsonProperty("collections")] public Dictionary<string, IEndpointRateLimit> CollectionsRateLimits { get; set; }
            [JsonProperty("contacts")] public Dictionary<string, IEndpointRateLimit> ContactsRateLimits { get; set; }
            [JsonProperty("device")] public Dictionary<string, IEndpointRateLimit> DeviceRateLimits { get; set; }
            [JsonProperty("direct_messages")] public Dictionary<string, IEndpointRateLimit> DirectMessagesRateLimits { get; set; }
            [JsonProperty("favorites")] public Dictionary<string, IEndpointRateLimit> FavoritesRateLimits { get; set; }
            [JsonProperty("feedback")] public Dictionary<string, IEndpointRateLimit> FeedbacksRateLimits { get; set; }
            [JsonProperty("followers")] public Dictionary<string, IEndpointRateLimit> FollowersRateLimits { get; set; }
            [JsonProperty("friends")] public Dictionary<string, IEndpointRateLimit> FriendsRateLimits { get; set; }
            [JsonProperty("friendships")] public Dictionary<string, IEndpointRateLimit> FriendshipsRateLimits { get; set; }
            [JsonProperty("geo")] public Dictionary<string, IEndpointRateLimit> GeoRateLimits { get; set; }
            [JsonProperty("help")] public Dictionary<string, IEndpointRateLimit> HelpRateLimits { get; set; }
            [JsonProperty("lists")] public Dictionary<string, IEndpointRateLimit> ListsRateLimits { get; set; }
            [JsonProperty("media")] public Dictionary<string, IEndpointRateLimit> MediaRateLimits { get; set; }
            [JsonProperty("moments")] public Dictionary<string, IEndpointRateLimit> MomentsRateLimits { get; set; }
            [JsonProperty("mutes")] public Dictionary<string, IEndpointRateLimit> MutesRateLimits { get; set; }
            [JsonProperty("saved_searches")] public Dictionary<string, IEndpointRateLimit> SavedSearchesRateLimits { get; set; }
            [JsonProperty("search")] public Dictionary<string, IEndpointRateLimit> SearchRateLimits { get; set; }
            [JsonProperty("statuses")] public Dictionary<string, IEndpointRateLimit> StatusesRateLimits { get; set; }
            [JsonProperty("tweet_prompts")] public Dictionary<string, IEndpointRateLimit> TweetPromptsRateLimits { get; set; }
            [JsonProperty("trends")] public Dictionary<string, IEndpointRateLimit> TrendsRateLimits { get; set; }
            [JsonProperty("users")] public Dictionary<string, IEndpointRateLimit> UsersRateLimits { get; set; }
        }

        [JsonProperty("rate_limit_context")]
        private JObject _rateLimitContext
        {
            set
            {
                if (value.TryGetValue("access_token", out var jsonObj))
                {
                    RateLimitContext = jsonObj.ToObject<string>();
                }
                else if (value.TryGetValue("application", out jsonObj))
                {
                    RateLimitContext = jsonObj.ToObject<string>();
                    IsApplicationOnlyCredentials = true;
                }
            }
        }

        [JsonProperty("resources")] private RateLimitResources _resources { get; set; }
    }
}