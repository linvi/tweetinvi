using System;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Events;
using Tweetinvi.Injectinvi;

namespace Tweetinvi
{
    /// <summary>
    /// For super users only. Change Tweetinvi internal mechanisms.
    /// </summary>
    public static class TweetinviContainer
    {
        private static readonly ITweetinviContainer _container;

        /// <summary>
        /// Event raised before the registration completes so that you can override registered classes.
        /// Doing so allow you to force Tweetinvi to use your own set of class instead of the one designed by the application.
        /// </summary>
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

        /// <summary>
        /// Allow you to retrieve any class used by Tweetinvi by specifying its interface.
        /// </summary>
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