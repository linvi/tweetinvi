using System;
using Newtonsoft.Json;
using Tweetinvi.Logic.JsonConverters;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Logic.DTO
{
    public class UploadedMediaInfo : IUploadedMediaInfo
    {
        public UploadedMediaInfo()
        {
            CreatedDate = DateTime.Now;
        }

        [JsonIgnore]
        public DateTime CreatedDate { get; private set; }

        [JsonProperty("media_id")]
        public long MediaId { get; set; }

        [JsonProperty("media_id_string")]
        public string MediaIdStr { get; set; }

        [JsonProperty("size")]
        public int MediaSize { get; set; }

        [JsonProperty("image")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public IUploadedImageDetails ImageDetails { get; set; }

        [JsonProperty("video")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public IUploadedVideoDetails VideoDetails { get; set; }

        [JsonProperty("processing_info")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public IUploadProcessingInfo ProcessingInfo { get; set; }
    }
}