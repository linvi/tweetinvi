using Newtonsoft.Json;
using Tweetinvi.Core;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Logic.JsonConverters;

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