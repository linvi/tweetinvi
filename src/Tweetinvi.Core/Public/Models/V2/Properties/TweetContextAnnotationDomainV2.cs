using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class TweetContextAnnotationDomainV2
    {
        /// <summary>
        /// Long form description of domain classification.
        /// </summary>
        [JsonProperty("description")] public string Description { get; set; }

        /// <summary>
        /// Contains the numeric value of the domain.
        /// </summary>
        [JsonProperty("id")] public string Id { get; set; }

        /// <summary>
        /// Domain name based on the Tweet text.
        /// </summary>
        [JsonProperty("name")] public string Name { get; set; }
    }
}