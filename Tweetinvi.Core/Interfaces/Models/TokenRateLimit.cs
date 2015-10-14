using System;
using Newtonsoft.Json;
using Tweetinvi.Core.Interfaces.Credentials;

namespace Tweetinvi.Core.Interfaces.Models
{
    public class TokenRateLimit : ITokenRateLimit
    {
        private long _reset;

        [JsonProperty("remaining")]
        public int Remaining { get; set; }

        [JsonProperty("reset")]
        public long Reset
        {
            get { return _reset; }
            set
            {
                _reset = value;
                ResetDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                ResetDateTime = ResetDateTime.AddSeconds(_reset).ToLocalTime();
            }
        }

        [JsonProperty("limit")]
        public int Limit { get; private set; }

        [JsonIgnore]
        public double ResetDateTimeInSeconds
        {
            get
            {
                if (ResetDateTime <= DateTime.Now)
                {
                    return 0;
                }

                return (ResetDateTime - DateTime.Now).TotalSeconds;
            }
        }

        [JsonIgnore]
        public double ResetDateTimeInMilliseconds
        {
            get { return ResetDateTimeInSeconds * 1000; }
        }

        [JsonIgnore]
        public DateTime ResetDateTime { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}/{1} (Reset in  {2} seconds)", Remaining, Limit, ResetDateTimeInSeconds);
        }
    }
}