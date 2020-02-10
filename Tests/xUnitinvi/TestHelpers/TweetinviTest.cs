using System;
using Tweetinvi;
using Tweetinvi.Events;
using Xunit.Abstractions;
using xUnitinvi.EndToEnd;

namespace xUnitinvi.TestHelpers
{
    public class TweetinviTest : IDisposable
    {
        protected readonly ITestOutputHelper _logger;
        protected readonly TwitterClient _tweetinviClient;
        protected readonly TwitterClient _tweetinviTestClient;
        protected readonly TwitterClient _protectedClient;

        protected TweetinviTest(ITestOutputHelper logger)
        {
            _logger = logger;
            _logger.WriteLine(DateTime.Now.ToLongTimeString());
            TweetinviEvents.BeforeWaitingForRequestRateLimits += TweetinviEventsOnBeforeWaitingForRequestRateLimits;

            _tweetinviClient = new TwitterClient(EndToEndTestConfig.TweetinviApi.Credentials);
            _tweetinviTestClient = new TwitterClient(EndToEndTestConfig.TweetinviTest.Credentials);
            _protectedClient = new TwitterClient(EndToEndTestConfig.ProtectedUser.Credentials);
        }

        private void TweetinviEventsOnBeforeWaitingForRequestRateLimits(object sender, BeforeExecutingRequestEventArgs args)
        {
            _logger.WriteLine($"{args.TwitterQuery.HttpMethod.ToString()}  {args.Url}");
        }

        public void Dispose()
        {
            TweetinviEvents.BeforeWaitingForRequestRateLimits -= TweetinviEventsOnBeforeWaitingForRequestRateLimits;
            GC.SuppressFinalize(this);
        }
    }
}