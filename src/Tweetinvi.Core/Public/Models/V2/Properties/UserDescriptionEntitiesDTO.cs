using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class UserDescriptionEntitiesDTO
    {
        [JsonProperty("cashtags")] public CashtagDTO[] cashtags { get; set; }

        [JsonProperty("hashtags")] public HashtagDTO[] hashtags { get; set; }

        [JsonProperty("mentions")] public UserMentionDTO[] mentions { get; set; }

        [JsonProperty("urls")] public UrlDTO[] urls { get; set; }
    }
}