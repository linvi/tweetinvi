using Newtonsoft.Json;
using Tweetinvi.Core.JsonConverters;
using Tweetinvi.Models;

namespace Tweetinvi.Core.DTO
{
    public class TweetIdentifierDTO : ITweetIdentifier
    {
        [JsonProperty("id")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public long Id { get; set; }

        [JsonProperty("id_str")]
        public string IdStr { get; set; }
    }
}