using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class UserEntitiesV2
    {
        /// <summary>
        /// Contains details about URLs, Hashtags, Cashtags, or mentions located within a user's description.
        /// </summary>
        [JsonProperty("description")] public UserDescriptionEntitiesV2 Description { get; set; }

        /// <summary>
        /// Contains details about the user's profile website.
        /// </summary>
        [JsonProperty("url")] public UrlsV2 Url { get; set; }
    }
}