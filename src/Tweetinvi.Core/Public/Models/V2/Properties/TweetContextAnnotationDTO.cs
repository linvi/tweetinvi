using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class TweetContextAnnotationDTO
    {
        [JsonProperty("domain")] public TweetContextAnnotationDomainDTO domain { get; set; }

        [JsonProperty("entity")] public TweetContextAnnotationEntityDTO entity { get; set; }
    }
}