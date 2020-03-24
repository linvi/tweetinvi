using Newtonsoft.Json;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.DTO
{
    public class UploadedVideoDetailsDTO : IUploadedVideoDetails
    {
        [JsonProperty("video_type")]
        public string VideoType { get; set; }
    }
}