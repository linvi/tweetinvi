using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class UserDescriptionEntitiesDTO
    {
        [JsonProperty("cashtags")] public CashtagDTO[] Cashtags { get; set; }
        [JsonProperty("hashtags")] public HashtagDTO[] Hashtags { get; set; }
        [JsonProperty("mentions")] public UserMentionDTO[] Mentions { get; set; }
        [JsonProperty("urls")] public UrlDTO[] Urls { get; set; }
    }
}