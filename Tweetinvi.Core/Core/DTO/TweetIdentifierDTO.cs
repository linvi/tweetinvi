using Newtonsoft.Json;
using Tweetinvi.Logic.JsonConverters;
using Tweetinvi.Models;

namespace Tweetinvi.Logic.DTO
{
    public class TweetIdentifierDTO : ITweetIdentifier
    {
        private long _id;

        public TweetIdentifierDTO()
        {
            _id = TweetinviSettings.DEFAULT_ID;
        }

        public bool IsTweetPublished { get; set; }

        [JsonProperty("id")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public long Id
        {
            get { return _id; }
            set
            {
                _id = value;
                if (_id != TweetinviSettings.DEFAULT_ID)
                {
                    IsTweetPublished = true;
                }
            }
        }

        [JsonProperty("id_str")]
        public string IdStr { get; set; }
    }
}