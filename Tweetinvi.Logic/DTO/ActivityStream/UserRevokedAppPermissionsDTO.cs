using System;
using Newtonsoft.Json;
using Tweetinvi.Core.Public.Streaming.Events;

namespace Tweetinvi.Logic.DTO.ActivityStream
{
    public class UserRevokedAppPermissionsDTO : IUserRevokedAppPermissionsDTO
    {
        [JsonProperty("date_time")]
        public DateTime DateTime { get; set; }

        [JsonProperty("target")]
        public IActivityStreamAppIdentifierDTO Target { get; set; }

        [JsonProperty("source")]
        public IActivityStreamUserIdentifierDTO Source { get; set; }
    }
}
