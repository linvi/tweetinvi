using Newtonsoft.Json;

namespace Tweetinvi.Models.V2.Responses
{
    public class TweetResponseDTO
    {
        [JsonProperty("data")] public TweetDTO Tweet { get; set; }
        [JsonProperty("includes")] public TweetIncludesDTO Includes { get; set; }
        [JsonProperty("errors")] public ErrorDTO[] Errors { get; set; }
    }
}