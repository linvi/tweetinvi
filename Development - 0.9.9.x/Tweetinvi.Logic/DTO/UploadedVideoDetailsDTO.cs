using Newtonsoft.Json;
using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi.Logic.DTO
{
    public class UploadedVideoDetailsDTO : IUploadedVideoDetails
    {
        [JsonProperty("video_type")]
        public string VideoType { get; set; }
    }
}