using Newtonsoft.Json;

namespace Tweetinvi.Models.V2.Responses
{
    public class SearchTweetsResponseDTO : TweetsResponseDTO
    {
        /// <summary>
        /// Search response metadata
        /// </summary>
        [JsonProperty("meta")] public SearchTweetsMetadataDTO SearchMetadata { get; set; }
    }
}