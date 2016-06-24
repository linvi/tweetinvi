using Newtonsoft.Json;
using Tweetinvi.Models.Entities.ExtendedEntities;

namespace Tweetinvi.Logic.TwitterEntities.ExtendedEntities
{
    public class VideoInformationEntity : IVideoInformationEntity
    {
        [JsonProperty("aspect_ratio")]
        public int[] AspectRatio { get; set; }

        [JsonProperty("duration_millis")]
        public int DurationInMilliseconds { get; set; }

        [JsonProperty("variants")]
        public IVideoEntityVariant[] Variants { get; set; }
    }
}