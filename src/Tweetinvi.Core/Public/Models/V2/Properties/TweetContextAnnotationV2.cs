using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class TweetContextAnnotationV2
    {
        [JsonProperty("domain")] public TweetContextAnnotationDomainV2 Domain { get; set; }
        [JsonProperty("entity")] public TweetContextAnnotationEntityV2 Entity { get; set; }
    }
}