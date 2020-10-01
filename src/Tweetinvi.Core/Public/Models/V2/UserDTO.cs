using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class UserDTO
    {
        [JsonProperty("created_at")] public DateTimeOffset CreatedAt { get; set; }
        [JsonProperty("description")] public string Description { get; set; }
        [JsonProperty("entities")] public UserEntitiesDTO Entities { get; set; }
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("protected")] public bool IsProtected { get; set; }
        [JsonProperty("location")] public string Location { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("pinned_tweet_id")] public string PinnedTweetId { get; set; }
        [JsonProperty("profile_image_url")] public string ProfileImageUrl { get; set; }
        [JsonProperty("url")] public string Url { get; set; }
        [JsonProperty("username")] public string Username { get; set; }
        [JsonProperty("verified")] public bool Verified { get; set; }
        [JsonProperty("withheld")] public WithheldDTO Withheld { get; set; }

        [JsonProperty("public_metrics")] public UserPublicMetricsDTO PublicMetrics { get; set; }
    }
}