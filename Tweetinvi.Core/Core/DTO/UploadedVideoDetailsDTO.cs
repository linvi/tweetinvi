using Newtonsoft.Json;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Logic.DTO
{
    public class UploadedVideoDetailsDTO : IUploadedVideoDetails
    {
        [JsonProperty("video_type")]
        public string VideoType { get; set; }
    }
}