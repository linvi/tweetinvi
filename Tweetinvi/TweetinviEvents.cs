using System;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Events;

namespace Tweetinvi
{
    /// <summary>
    /// Take power and control the execution of requests based on the state of your environment!
    /// </summary>
    public static class TweetinviEvents
    {
        private static readonly ITweetinviEvents _tweetinviEvents;

        static TweetinviEvents()
        {
            _tweetinviEvents = TweetinviContainer.Resolve<ITweetinviEvents>();
        }

        public static void SubscribeToClientEvents(ITwitterClient client)
        {
            _tweetinviEvents.SubscribeToClientEvents(client);
        }

        public static void UnsubscribeFromClientEvents(ITwitterClient client)
        {
            _tweetinviEvents.UnsubscribeFromClientEvents(client);
        }

        /// <inheritdoc cref="IExternalClientEvents.BeforeWaitingForRequestRateLimits" />
        public static event EventHandler<BeforeExecutingRequestEventArgs> BeforeWaitingForRequestRateLimits
        {
            add => _tweetinviEvents.BeforeWaitingForRequestRateLimits += value;
            remove => _tweetinviEvents.BeforeWaitingForRequestRateLimits -= value;
        }


        /// <inheritdoc cref="IExternalClientEvents.BeforeExecutingRequest" />
        public static event EventHandler<BeforeExecutingRequestEventArgs> BeforeExecutingRequest
        {
            add => _tweetinviEvents.BeforeExecutingRequest += value;
            remove => _tweetinviEvents.BeforeExecutingRequest -= value;
        }

        /// <inheritdoc cref="IExternalClientEvents.AfterExecutingRequest" />
        public static event EventHandler<AfterExecutingQueryEventArgs> AfterExecutingRequest
        {
            add => _tweetinviEvents.AfterExecutingRequest += value;
            remove => _tweetinviEvents.AfterExecutingRequest -= value;
        }

        /// <inheritdoc cref="IExternalClientEvents.OnTwitterException" />
        public static event EventHandler<ITwitterException> OnTwitterException
        {
            add => _tweetinviEvents.OnTwitterException += value;
            remove => _tweetinviEvents.OnTwitterException -= value;
        }
    }
}