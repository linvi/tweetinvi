using Newtonsoft.Json;

namespace Tweetinvi.Models.V2.Responses
{
    public class TweetResponseDTO
    {
        [JsonProperty("data")] public TweetDTO data { get; set; }

        [JsonProperty("includes")] public TweetIncludesDTO includes { get; set; }

        [JsonProperty("errors")] public ErrorDTO[] errors { get; set; }
    }
}