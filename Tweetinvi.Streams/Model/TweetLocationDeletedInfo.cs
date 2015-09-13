using Newtonsoft.Json;
using Tweetinvi.Core.Interfaces.Models.StreamMessages;

namespace Tweetinvi.Streams.Model
{
    public class TweetLocationDeletedInfo : ITweetLocationDeletedInfo
    {
        [JsonProperty("user_id")]
        public long UserId { get; set; }

        [JsonProperty("user_id_str")]
        public string UserIdStr { get; set; }

        [JsonProperty("up_to_status_id")]
        public long UpToStatusId { get; set; }

        [JsonProperty("up_to_status_id_str")]
        public string UpToStatusIdStr { get; set; }
    }
}