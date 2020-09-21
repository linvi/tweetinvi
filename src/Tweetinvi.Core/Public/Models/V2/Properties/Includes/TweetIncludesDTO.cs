using Newtonsoft.Json;
using Tweetinvi.Models.V2.Responses;

namespace Tweetinvi.Models.V2
{
    public class TweetIncludesDTO
    {
        [JsonProperty("media")] public MediaDTO[] media { get; set; }

        [JsonProperty("places")] public PlaceDTO[] places { get; set; }

        [JsonProperty("polls")] public PollDTO[] polls { get; set; }

        [JsonProperty("tweets")] public TweetDTO[] tweets { get; set; }

        [JsonProperty("users")] public UserDTO[] users { get; set; }
    }
}