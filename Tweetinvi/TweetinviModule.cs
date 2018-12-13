using Tweetinvi.Core.ExecutionContext;
using Tweetinvi.Core.Injectinvi;

namespace Tweetinvi
{
    public class TweetinviModule : ITweetinviModule
    {
        public void Initialize(ITweetinviContainer container)
        {
            // Register a singleton of the container, do not use InstancePerApplication
            container.RegisterInstance(typeof(ITweetinviContainer), container);
            container.RegisterType<ICrossExecutionContextPreparer, CrossExecutionContextPreparer>(RegistrationLifetime
                .InstancePerApplication);
        }
    }
}