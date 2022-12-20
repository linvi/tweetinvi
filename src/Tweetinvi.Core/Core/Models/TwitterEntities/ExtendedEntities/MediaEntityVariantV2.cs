using Newtonsoft.Json;

using Tweetinvi.Models.Entities.ExtendedEntities;

namespace Tweetinvi.Core.Models.TwitterEntities.ExtendedEntities;
public class MediaEntityVariantV2 : IVideoEntityVariant
{
    [JsonProperty("bit_rate")]
    public int Bitrate { get; set; }
    [JsonProperty("content_type")]
    public string ContentType { get; set; }
    [JsonProperty("url")]
    public string URL { get; set; }
}
