using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class SearchTweetsV2Response : TweetsV2Response
    {
        /// <summary>
        /// Search response metadata
        /// </summary>
        [JsonProperty("meta")] public SearchTweetsMetadataV2 SearchMetadata { get; set; }
    }
}