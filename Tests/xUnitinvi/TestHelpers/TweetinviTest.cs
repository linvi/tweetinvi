using System;
using Tweetinvi;
using Tweetinvi.Events;
using Xunit.Abstractions;

namespace xUnitinvi.TestHelpers
{
    public class TweetinviTest : IDisposable
    {
        protected readonly ITestOutputHelper _logger;

        public TweetinviTest(ITestOutputHelper logger)
        {
            _logger = logger;
            _logger.WriteLine(DateTime.Now.ToLongTimeString());
            TweetinviEvents.BeforeWaitingForRequestRateLimits += TweetinviEventsOnBeforeWaitingForRequestRateLimits;
        }

        private void TweetinviEventsOnBeforeWaitingForRequestRateLimits(object sender, BeforeExecutingRequestEventArgs args)
        {
            _logger.WriteLine(args.Url);
        }

        public void Dispose()
        {
            TweetinviEvents.BeforeWaitingForRequestRateLimits -= TweetinviEventsOnBeforeWaitingForRequestRateLimits;
            GC.SuppressFinalize(this);
        }
    }
}