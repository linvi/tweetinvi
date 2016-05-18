using Newtonsoft.Json;
using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi.Logic.DTO
{
    public class UploadProcessingInfoDTO : IUploadProcessingInfo
    {
        public UploadProcessingInfoDTO()
        {
            CheckAfterInSeconds = 0;
            ProgressPercentage = 0;
        }

        public string State { get; set; }

        [JsonProperty("check_after_secs")]
        public int CheckAfterInSeconds { get; set; }

        [JsonIgnore]
        public int CheckAfterInMilliseconds { get { return CheckAfterInSeconds*1000; } }

        [JsonProperty("progress_percent")]
        public int ProgressPercentage { get; set; }
    }
}