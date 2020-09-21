using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class UserDTO
    {
        [JsonProperty("created_at")] public DateTimeOffset created_at { get; set; }

        [JsonProperty("description")] public string description { get; set; }

        [JsonProperty("entities")] public UserEntitiesDTO entities { get; set; }

        [JsonProperty("id")] public string id { get; set; }

        [JsonProperty("protected")] public bool is_protected { get; set; }

        [JsonProperty("location")] public string location { get; set; }

        [JsonProperty("name")] public string name { get; set; }

        [JsonProperty("pinned_tweet_id")] public string pinned_tweet_id { get; set; }

        [JsonProperty("profile_image_url")] public string profile_image_url { get; set; }

        [JsonProperty("public_metrics")] public UserPublicMetricsDTO public_metrics { get; set; }

        [JsonProperty("url")] public string url { get; set; }

        [JsonProperty("username")] public string username { get; set; }

        [JsonProperty("verified")] public bool verified { get; set; }

        [JsonProperty("withheld")] public WithheldDTO withheld { get; set; }
    }
}