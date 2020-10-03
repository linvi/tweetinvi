using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class TweetEntitiesV2
    {
        [JsonProperty("annotations")] public TweetAnnotationV2[] Annotations { get; set; }
        [JsonProperty("cashtags")] public CashtagV2[] Cashtags { get; set; }
        [JsonProperty("hashtags")] public HashtagV2[] Hashtags { get; set; }
        [JsonProperty("mentions")] public UserMentionV2[] Mentions { get; set; }
        [JsonProperty("urls")] public UrlV2[] Urls { get; set; }
    }
}