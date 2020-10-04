using Tweetinvi.Core.Client;
using Tweetinvi.Models;

namespace Tweetinvi.Parameters.RateLimitsClient
{
    public interface IWaitForCredentialsRateLimitParameters
    {
        string Url { get; set; }
        ITwitterRequest Request { get; set; }
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

        public WaitForCredentialsRateLimitParameters(ITwitterRequest request)
        {
            Request = request;
            Credentials = request.Query.TwitterCredentials;
            ExecutionContext = request.ExecutionContext;
            Url = request.Query.Url;
        }

        public string Url { get; set; }
        public ITwitterRequest Request { get; set; }
        public IReadOnlyTwitterCredentials Credentials { get; set; }
        public ITwitterExecutionContext ExecutionContext { get; set; }
        public RateLimitsSource From { get; set; }
    }
}