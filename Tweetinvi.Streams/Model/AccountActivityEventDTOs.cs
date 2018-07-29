using Newtonsoft.Json;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Streams.Model
{
    public class AccountActivityFavouriteEventDTO
    {
        public string Id { get; set; }

        [JsonProperty("user")]
        public IUserDTO User { get; set; }

        [JsonProperty("favorited_status")]
        public ITweetDTO FavouritedTweet { get; set; }
    }

    public class AccountActivityUserFollowedEventDTO
    {
        public string Type { get; set; }
        public string CreatedTimestamp { get; set; }

        [JsonProperty("source")]
        public IUserDTO Source { get; set; }

        [JsonProperty("target")]
        public IUserDTO Target { get; set; }
    }

    public class AccountActivityUserBlockedEventDTO
    {
        public string Type { get; set; }
        public string CreatedTimestamp { get; set; }

        [JsonProperty("source")]
        public IUserDTO Source { get; set; }

        [JsonProperty("target")]
        public IUserDTO Target { get; set; }
    }
}
