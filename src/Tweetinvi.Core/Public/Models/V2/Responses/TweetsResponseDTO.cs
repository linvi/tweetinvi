using Newtonsoft.Json;

namespace Tweetinvi.Models.V2.Responses
{
    public class TweetsResponseDTO
    {
        [JsonProperty("data")] public TweetDTO[] data { get; set; } = new TweetDTO[0];

        [JsonProperty("includes")] public TweetIncludesDTO includes { get; set; }

        [JsonProperty("errors")] public ErrorDTO[] errors { get; set; }
    }
}