using Newtonsoft.Json;

namespace Tweetinvi.Models.V2.Responses
{
    public class TweetsResponseDTO
    {
        [JsonProperty("data")] public TweetDTO[] Tweets { get; set; } = new TweetDTO[0];
        [JsonProperty("includes")] public TweetIncludesDTO Includes { get; set; }
        [JsonProperty("errors")] public ErrorDTO[] Errors { get; set; }
    }
}