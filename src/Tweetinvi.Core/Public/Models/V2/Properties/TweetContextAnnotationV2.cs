using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class TweetContextAnnotationV2
    {
        /// <summary>
        /// Contains elements which identify detailed information regarding the domain classification based on Tweet text.
        /// </summary>
        [JsonProperty("domain")] public TweetContextAnnotationDomainV2 Domain { get; set; }

        /// <summary>
        /// Contains elements which identify detailed information regarding the domain classification bases on Tweet text.
        /// </summary>
        [JsonProperty("entity")] public TweetContextAnnotationEntityV2 Entity { get; set; }
    }
}