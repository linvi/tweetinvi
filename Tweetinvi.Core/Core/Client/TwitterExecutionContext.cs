using System;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.RateLimit;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Client
{
    public interface ITwitterExecutionContext : ITweetinviSettings
    {
        Func<ITwitterRequest> RequestFactory { get; set; }
        ITweetinviContainer Container { get; set; }
    }

    public class TwitterExecutionContext : TweetinviSettings, ITwitterExecutionContext
    {
        public TwitterExecutionContext()
        {
            RequestFactory = () => throw new InvalidOperationException($"You cannot run contextual operations without configuring the {nameof(RequestFactory)} of the ExecutionContext");
        }

        public TwitterExecutionContext(ITwitterExecutionContext context) : base(context)
        {
            RequestFactory = context.RequestFactory;
            Container = context.Container;
        }

        public Func<ITwitterRequest> RequestFactory { get; set; }
        public ITweetinviContainer Container { get; set; }
    }
}
