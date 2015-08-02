using Tweetinvi.Core.Injectinvi;

namespace Tweetinvi
{
    public class TweetinviModule : ITweetinviModule
    {
        private readonly ITweetinviContainer _container;

        public TweetinviModule(ITweetinviContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            // Register a singleton of the container, do not use InstancePerApplication
            _container.RegisterInstance(typeof(ITweetinviContainer), _container);
        }
    }
}