using Newtonsoft.Json;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.DTO
{
    public class UploadProcessingError : IUploadProcessingError
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}