using System;

namespace Tweetinvi.Core.Interfaces.Credentials
{
    public interface IRateLimit
    {
        string ResourceName { get; }
        int Remaining { get; }
        int Limit { get; }
        double ResetTimeInSecond { get; }
        DateTime ResetDateTime { get; }
    }
}