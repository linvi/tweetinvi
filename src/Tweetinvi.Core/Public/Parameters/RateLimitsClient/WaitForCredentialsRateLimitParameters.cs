using Tweetinvi.Core.Client;
using Tweetinvi.Models;

namespace Tweetinvi.Parameters.RateLimitsClient
{
    public interface IWaitForCredentialsRateLimitParameters
    {
        string Url { get; set; }
        IReadOnlyTwitterCredentials Credentials { get; set; }
        ITwitterExecutionContext ExecutionContext { get; set; }
        RateLimitsSource From { get; set; }
    }

    public class WaitForCredentialsRateLimitParameters : IWaitForCredentialsRateLimitParameters
    {
        public WaitForCredentialsRateLimitParameters(string url)
        {
            Url = url;
        }

        public string Url { get; set; }
        public IReadOnlyTwitterCredentials Credentials { get; set; }
        public ITwitterExecutionContext ExecutionContext { get; set; }
        public RateLimitsSource From { get; set; }
    }
}