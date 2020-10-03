using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class UserEntitiesV2
    {
        [JsonProperty("description")] public UserDescriptionEntitiesV2 Description { get; set; }
        [JsonProperty("url")] public UrlsV2 Url { get; set; }
    }
}