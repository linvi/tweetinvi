using System;
using Newtonsoft.Json;

namespace Tweetinvi.Streams.Model.AccountActivity
{
    public class ActivityStreamUserRevokedAppPermissionsDTO
    {
        [JsonProperty("date_time")]
        public DateTimeOffset DateTime { get; set; }

        [JsonProperty("target")]
        public ActivityStreamAppIdentifierDTO Target { get; set; }

        [JsonProperty("source")]
        public ActivityStreamUserIdentifierDTO Source { get; set; }
    }
}
