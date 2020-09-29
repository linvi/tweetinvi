using Newtonsoft.Json;

namespace Tweetinvi.Models.V2.Responses
{
    public class SearchTweetsResponseDTO : TweetsResponseDTO
    {
        [JsonProperty("meta")] public SearchTweetsMetadataDTO meta { get; set; }
    }
}