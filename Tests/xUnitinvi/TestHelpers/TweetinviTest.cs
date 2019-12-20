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
            TweetinviEvents.QueryBeforeExecute += TweetinviEventsOnQueryBeforeExecute;
        }

        private void TweetinviEventsOnQueryBeforeExecute(object sender, QueryBeforeExecuteEventArgs args)
        {
            _logger.WriteLine(args.Url);
        }

        public void Dispose()
        {
            TweetinviEvents.QueryBeforeExecute -= TweetinviEventsOnQueryBeforeExecute;
            GC.SuppressFinalize(this);
        }
    }
}