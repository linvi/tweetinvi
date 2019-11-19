using Newtonsoft.Json;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.DTO
{
    public class TweetFromSearchMetadata : ITweetFromSearchMetadata
    {
        [JsonProperty("result_type")]
        public string ResultType { get; private set; }

        [JsonProperty("iso_language_code")]
        public string IsoLanguageCode { get; private set; }
    }
}