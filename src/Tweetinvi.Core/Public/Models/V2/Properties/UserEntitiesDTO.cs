using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class UserEntitiesDTO
    {
        [JsonProperty("description")] public UserDescriptionEntitiesDTO Description { get; set; }
        [JsonProperty("url")] public UrlsDTO Url { get; set; }
    }
}