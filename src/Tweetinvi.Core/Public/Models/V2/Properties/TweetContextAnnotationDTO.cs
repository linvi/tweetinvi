using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class TweetContextAnnotationDTO
    {
        [JsonProperty("domain")] public TweetContextAnnotationDomainDTO Domain { get; set; }
        [JsonProperty("entity")] public TweetContextAnnotationEntityDTO Entity { get; set; }
    }
}