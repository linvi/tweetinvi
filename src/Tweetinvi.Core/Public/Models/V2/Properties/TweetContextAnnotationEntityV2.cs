using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class TweetContextAnnotationEntityV2
    {
        /// <summary>
        /// Additional information regarding referenced entity.
        /// </summary>
        [JsonProperty("description")] public string Description { get; set; }

        /// <summary>
        /// Unique value which correlates to an explicitly mentioned Person, Place, Product or Organization
        /// </summary>
        [JsonProperty("id")] public string Id { get; set; }

        /// <summary>
        /// Name or reference of entity referenced in the Tweet.
        /// </summary>
        [JsonProperty("name")] public string Name { get; set; }
    }
}