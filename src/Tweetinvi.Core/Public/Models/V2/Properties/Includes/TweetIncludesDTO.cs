using Newtonsoft.Json;
using Tweetinvi.Models.V2.Responses;

namespace Tweetinvi.Models.V2
{
    public class TweetIncludesDTO
    {
        [JsonProperty("media")] public MediaDTO[] Media { get; set; }
        [JsonProperty("places")] public PlaceDTO[] Places { get; set; }
        [JsonProperty("polls")] public PollDTO[] Polls { get; set; }
        [JsonProperty("tweets")] public TweetDTO[] Tweets { get; set; }
        [JsonProperty("users")] public UserDTO[] Users { get; set; }
    }
}