using System;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.RateLimit;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Client
{
    public interface ITwitterExecutionContext : ITweetinviSettings
    {
        Func<ITwitterRequest> RequestFactory { get; set; }
        IRateLimitCacheManager RateLimitCacheManager { get; set; }
        ITweetinviContainer Container { get; set; }
        new ITwitterExecutionContext Clone();
    }

    public class TwitterExecutionContext : TweetinviSettings, ITwitterExecutionContext
    {
        public TwitterExecutionContext()
        {
            RequestFactory = () => throw new InvalidOperationException($"You cannot run contextual operations without configuring the {nameof(RequestFactory)} of the ExecutionContext");
        }

        public Func<ITwitterRequest> RequestFactory { get; set; }
        public IRateLimitCacheManager RateLimitCacheManager { get; set; }
        public ITweetinviContainer Container { get; set; }
        public new ITwitterExecutionContext Clone()
        {
            var clone = new TwitterExecutionContext
            {
                RequestFactory = RequestFactory
            };

            clone.InitialiseFrom(this);

            return clone;
        }
    }
}
