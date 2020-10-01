using Newtonsoft.Json;

namespace Tweetinvi.Models.V2.Responses
{
    public class SearchTweetsResponseDTO : TweetsResponseDTO
    {
        [JsonProperty("meta")] public SearchTweetsMetadataDTO SearchMetadata { get; set; }
    }
}