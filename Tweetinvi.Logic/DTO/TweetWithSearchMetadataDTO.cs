using Newtonsoft.Json;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Logic.JsonConverters;

namespace Tweetinvi.Logic.DTO
{
    public class TweetWithSearchMetadataDTO : TweetDTO, ITweetWithSearchMetadataDTO
    {
        [JsonProperty("metadata")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public ITweetFromSearchMetadata TweetFromSearchMetadata { get; private set; }
    }
}
