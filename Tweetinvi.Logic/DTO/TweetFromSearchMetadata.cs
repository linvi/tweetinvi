using Newtonsoft.Json;
using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi.Logic.DTO
{
    public class TweetFromSearchMetadata : ITweetFromSearchMetadata
    {
        [JsonProperty("result_type")]
        public string ResultType { get; private set; }

        [JsonProperty("iso_language_code")]
        public string ISOLanguageCode { get; private set; }
    }
}