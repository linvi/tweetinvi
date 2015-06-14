using System;

namespace Tweetinvi.Core.Interfaces.Credentials
{
    public interface ITokenRateLimit
    {
        /// <summary>
        /// Remaining operation authorized on this Token
        /// </summary>
        int Remaining { get; set; }
        long Reset { get; }
        int Limit { get; }

        double ResetDateTimeInSeconds { get; }
        double ResetDateTimeInMilliseconds { get; }
        DateTime ResetDateTime { get; }
    }
}