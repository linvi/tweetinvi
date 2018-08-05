using Newtonsoft.Json;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Streams.Model.AccountActivity
{
    public class AccountActivityUserToUserEventDTO
    {
        public string Type { get; set; }

        [JsonProperty("created_timestamp")]
        public string CreatedTimestamp { get; set; }

        [JsonProperty("source")]
        public IUserDTO Source { get; set; }

        [JsonProperty("target")]
        public IUserDTO Target { get; set; }
    }
}
