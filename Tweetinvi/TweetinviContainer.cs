using System;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Injectinvi;

namespace Tweetinvi
{
    public static class TweetinviContainer
    {
        private static readonly ITweetinviContainer _container;

        public static event EventHandler<TweetinviContainerEventArgs> BeforeRegistrationComplete;

        static TweetinviContainer()
        {
            _container = new AutofacContainer();
            _container.BeforeRegistrationCompletes += ContainerOnBeforeRegistrationCompletes;
        }

        private static void ContainerOnBeforeRegistrationCompletes(object sender, TweetinviContainerEventArgs args)
        {
            var handlers = BeforeRegistrationComplete;
            if (handlers != null)
            {
                handlers.Invoke(sender, args);
            }

            _container.BeforeRegistrationCompletes -= ContainerOnBeforeRegistrationCompletes;
        }

        public static T Resolve<T>()
        {
            if (!_container.IsInitialized)
            {
                _container.Initialize();
            }

            return _container.Resolve<T>();
        }
    }
}