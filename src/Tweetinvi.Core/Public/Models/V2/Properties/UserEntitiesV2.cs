using Newtonsoft.Json;

namespace Tweetinvi.Models
{
    public class UserEntitiesV2
    {
        [JsonProperty("description")] public UserDescriptionEntitiesV2 Description { get; set; }
        [JsonProperty("url")] public UrlsV2 Url { get; set; }
    }
}