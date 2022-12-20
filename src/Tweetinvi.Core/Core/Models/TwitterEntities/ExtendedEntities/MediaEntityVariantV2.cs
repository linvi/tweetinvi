using Newtonsoft.Json;

using System.Diagnostics;

using Tweetinvi.Models.Entities.ExtendedEntities;

namespace Tweetinvi.Core.Models.TwitterEntities.ExtendedEntities;

[DebuggerDisplay("ContentType: {ContentType} Url: {URL}")]
public class MediaEntityVariantV2 : IVideoEntityVariant
{
    [JsonProperty("bit_rate")]
    public int Bitrate { get; set; }
    [JsonProperty("content_type")]
    public string ContentType { get; set; }
    [JsonProperty("url")]
    public string URL { get; set; }
}
