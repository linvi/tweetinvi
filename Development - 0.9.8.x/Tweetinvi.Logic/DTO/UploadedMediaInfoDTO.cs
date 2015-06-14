using Newtonsoft.Json;
using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi.Logic.DTO
{
    public class UploadedMediaInfoDTO : IUploadedMediaInfoDTO
    {
        [JsonProperty("w")]
        public int? Width { get; set; }

        [JsonProperty("h")]
        public int? Height { get; set; }

        [JsonProperty("image_type")]
        public string ImageType { get; set; }
    }
}