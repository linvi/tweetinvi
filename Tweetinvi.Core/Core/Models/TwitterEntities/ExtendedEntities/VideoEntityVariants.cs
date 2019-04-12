using Newtonsoft.Json;
using Tweetinvi.Models.Entities.ExtendedEntities;

namespace Tweetinvi.Logic.TwitterEntities.ExtendedEntities
{
    public class VideoEntityVariant : IVideoEntityVariant
    {
        [JsonProperty("bitrate")]
        public int Bitrate { get; set; }
        [JsonProperty("content_type")]
        public string ContentType { get; set; }
        [JsonProperty("url")]
        public string URL { get; set; }
    }
}