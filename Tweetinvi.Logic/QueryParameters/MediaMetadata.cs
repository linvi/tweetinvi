using Newtonsoft.Json;
using Tweetinvi.Logic.JsonConverters;

namespace Tweetinvi.Logic.QueryParameters
{
    public interface IMediaMetadata
    {
        long MediaId { get; set; }
        string AltText { get; set; }
    }

    public class MediaMetadata : IMediaMetadata
    {
        public MediaMetadata(long mediaId, string altText)
        {
            MediaId = mediaId;
            AltText = altText;
        }

        [JsonProperty("media_id")]
        public long MediaId { get; set; }

        [JsonProperty("alt_text")]
        [JsonConverter(typeof(JsonUploadMetadataAltTextConverter))]
        public string AltText { get; set; }
    }
}