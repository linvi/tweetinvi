using Newtonsoft.Json;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Streams.Model.AccountActivity
{
    public class AccountActivityFavouriteEventDTO
    {
        public string Id { get; set; }

        [JsonProperty("user")]
        public IUserDTO User { get; set; }

        [JsonProperty("favorited_status")]
        public ITweetDTO FavouritedTweet { get; set; }
    }
}
