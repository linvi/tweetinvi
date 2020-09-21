using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class UserEntitiesDTO
    {
        [JsonProperty("description")] public UserDescriptionEntitiesDTO description { get; set; }

        [JsonProperty("url")] public UrlsDTO url { get; set; }
    }
}