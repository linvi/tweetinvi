using System;
using Newtonsoft.Json;

namespace Tweetinvi.Logic.DTO.ActivityStream
{
    public class ActivityStreamUserRevokedAppPermissionsDTO
    {
        [JsonProperty("date_time")]
        public DateTime DateTime { get; set; }

        [JsonProperty("target")]
        public ActivityStreamAppIdentifierDTO Target { get; set; }

        [JsonProperty("source")]
        public ActivityStreamUserIdentifierDTO Source { get; set; }
    }
}
