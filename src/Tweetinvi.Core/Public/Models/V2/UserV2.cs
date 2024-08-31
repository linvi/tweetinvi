using System;
using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    /// <summary>
    /// A user
    /// <para>Read more here : https://developer.twitter.com/en/docs/twitter-api/data-dictionary/object-model/user </para>
    /// </summary>
    public class UserV2
    {
        /// <summary>
        /// The UTC datetime that the user account was created on Twitter.
        /// </summary>
        [JsonProperty("created_at")] public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// The text of this user's profile description (also known as bio), if the user provided one.
        /// </summary>
        [JsonProperty("description")] public string Description { get; set; }

        /// <summary>
        /// Contains details about text that has a special meaning in the user's description.
        /// </summary>
        [JsonProperty("entities")] public UserEntitiesV2 Entities { get; set; }

        /// <summary>
        /// The unique identifier of this user.
        /// </summary>
        [JsonProperty("id")] public string Id { get; set; }

        /// <summary>
        /// The location specified in the user's profile, if the user provided one. As this is a freeform value,
        /// it may not indicate a valid location, but it may be fuzzily evaluated when performing searches with location queries.
        /// </summary>
        [JsonProperty("location")] public string Location { get; set; }

        /// <summary>
        /// The name of the user, as they’ve defined it on their profile. Not necessarily a person’s name.
        /// </summary>
        [JsonProperty("name")] public string Name { get; set; }

        /// <summary>
        /// Unique identifier of this user's pinned Tweet.
        /// </summary>
        [JsonProperty("pinned_tweet_id")] public string PinnedTweetId { get; set; }

        /// <summary>
        /// The URL to the profile image for this user, as shown on the user's profile.
        /// </summary>
        [JsonProperty("profile_image_url")] public string ProfileImageUrl { get; set; }

        /// <summary>
        ///     The URL to this user's profile banner.
        /// </summary>
        [JsonProperty("profile_banner_url")] public string ProfileBannerUrl { get; set; }

        /// <summary>
        /// Indicates if this user has chosen to protect their Tweets (in other words, if this user's Tweets are private).
        /// </summary>
        [JsonProperty("protected")] public bool IsProtected { get; set; }

        /// <summary>
        /// The URL specified in the user's profile, if present.
        /// </summary>
        [JsonProperty("url")] public string Url { get; set; }

        /// <summary>
        /// The Twitter screen name, handle, or alias that this user identifies themselves with.
        /// Usernames are unique but subject to change.
        /// </summary>
        [JsonProperty("username")] public string Username { get; set; }

        /// <summary>
        /// Indicates if this user is a verified Twitter User.
        /// </summary>
        [JsonProperty("verified")] public bool Verified { get; set; }

        /// <summary>
        /// Contains withholding details for withheld content, if applicable.
        /// <para>Read more: https://help.twitter.com/en/rules-and-policies/tweet-withheld-by-country </para>
        /// </summary>
        [JsonProperty("withheld")] public WithheldInfoV2 Withheld { get; set; }

        /// <summary>
        /// Contains details about activity for this user.
        /// </summary>
        [JsonProperty("public_metrics")] public UserPublicMetricsV2 PublicMetrics { get; set; }
    }
}