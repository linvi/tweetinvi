using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models.Entities;
using Tweetinvi.Logic.JsonConverters;

namespace Tweetinvi.Logic.DTO
{
    public class UserDTO : UserIdentifierDTO, IUserDTO
    {
        // Verify : ProfileImageTile

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("status")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public ITweetDTO Status { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("created_at")]
        [JsonConverter(typeof(JsonTwitterDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("geo_enabled")]
        public bool GeoEnabled { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("lang")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public Language Language { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("statuses_count")]
        public int StatusesCount { get; set; }

        [JsonProperty("followers_count")]
        public int FollowersCount { get; set; }

        [JsonProperty("friends_count")]
        public int FriendsCount { get; set; }

        [JsonProperty("following")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public bool Following { get; set; }

        [JsonProperty("protected")]
        public bool Protected { get; set; }

        [JsonProperty("verified")]
        public bool Verified { get; set; }

        [JsonProperty("entities")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public IUserEntities Entities { get; set; }

        [JsonProperty("notifications")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public bool Notifications { get; set; }

        [JsonProperty("profile_image_url")]
        public string ProfileImageUrl { get; set; }

        [JsonProperty("profile_image_url_https")]
        public string ProfileImageUrlHttps { get; set; }

        [JsonProperty("follow_request_sent")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public bool FollowRequestSent { get; set; }

        [JsonProperty("default_profile")]
        public bool DefaultProfile { get; set; }

        [JsonProperty("default_profile_image")]
        public bool DefaultProfileImage { get; set; }

        [JsonProperty("favourites_count")]
        public int? FavoritesCount { get; set; }

        [JsonProperty("listed_count")]
        public int? ListedCount { get; set; }

        [JsonProperty("profile_sidebar_fill_color")]
        public string ProfileSidebarFillColor { get; set; }

        [JsonProperty("profile_sidebar_border_color")]
        public string ProfileSidebarBorderColor { get; set; }

        [JsonProperty("profile_background_tile")]
        public bool ProfileBackgroundTile { get; set; }

        [JsonProperty("profile_background_color")]
        public string ProfileBackgroundColor { get; set; }

        [JsonProperty("profile_background_image_url")]
        public string ProfileBackgroundImageUrl { get; set; }

        [JsonProperty("profile_background_image_url_https")]
        public string ProfileBackgroundImageUrlHttps { get; set; }

        [JsonProperty("profile_banner_url")]
        public string ProfileBannerURL { get; set; }

        [JsonProperty("profile_text_color")]
        public string ProfileTextColor { get; set; }

        [JsonProperty("profile_link_color")]
        public string ProfileLinkColor { get; set; }

        [JsonProperty("profile_use_background_image")]
        public bool ProfileUseBackgroundImage { get; set; }

        [JsonProperty("is_translator")]
        public bool IsTranslator { get; set; }

        [JsonProperty("show_all_inline_media")]
        public bool ShowAllInlineMedia { get; set; }

        [JsonProperty("contributors_enabled")]
        public bool ContributorsEnabled { get; set; }

        [JsonProperty("utc_offset")]
        public int? UtcOffset { get; set; }

        [JsonProperty("time_zone")]
        public string TimeZone { get; set; }

        [JsonProperty("withheld_in_countries")]
        public IEnumerable<string> WithheldInCountries { get; set; }

        [JsonProperty("withheld_scope")]
        public string WithheldScope { get; set; }
    }
}