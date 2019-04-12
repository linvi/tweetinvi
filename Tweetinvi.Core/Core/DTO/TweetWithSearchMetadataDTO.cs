using Newtonsoft.Json;
using Tweetinvi.Logic.JsonConverters;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Logic.DTO
{
    public class TweetWithSearchMetadataDTO : TweetDTO, ITweetWithSearchMetadataDTO
    {
        [JsonProperty("metadata")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public ITweetFromSearchMetadata TweetFromSearchMetadata { get; private set; }
    }
}
