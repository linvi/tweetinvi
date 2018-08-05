using Newtonsoft.Json;

namespace Tweetinvi.Streams.Model.AccountActivity
{
    public class AccountActivityTweetDeletedEventStatusDTO
    {
        [JsonProperty("id")]
        public long TweetId { get; set; }

        [JsonProperty("user_id")]
        public long UserId { get; set; }
    }
}
