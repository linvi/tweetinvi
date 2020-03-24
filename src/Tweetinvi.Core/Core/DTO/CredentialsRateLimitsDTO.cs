using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tweetinvi.Models;

namespace Tweetinvi.Core.DTO
{
    public class CredentialsRateLimitsDTO
    {
        public class RateLimitResources
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
            [JsonProperty("feedback")] public Dictionary<string, IEndpointRateLimit> FeedbackRateLimits { get; set; }
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
        public JObject RateLimitContext { get; set; }

        [JsonProperty("resources")]
        public RateLimitResources Resources { get; set; }
    }
}