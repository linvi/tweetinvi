using Tweetinvi.Client;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Core.Injectinvi;

namespace Tweetinvi
{
    public class TweetinviModule : ITweetinviModule
    {
        public void Initialize(ITweetinviContainer container)
        {
            // Register a singleton of the container, do not use InstancePerApplication
            container.RegisterInstance(typeof(ITweetinviContainer), container);

            container.RegisterType<IInternalAccountRequester, AccountRequester>();
            container.RegisterType<IInternalTweetsRequester, TweetsRequester>();
            container.RegisterType<IInternalUsersRequester, UsersRequester>();

            container.RegisterType<IInternalRequestExecutor, RequestExecutor>();
        }
    }
}