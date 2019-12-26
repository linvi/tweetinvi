using System;
using Tweetinvi.Core.Injectinvi;

namespace Tweetinvi.Events
{
    public class TweetinviContainerEventArgs : EventArgs
    {
        public TweetinviContainerEventArgs(ITweetinviContainer tweetinviContainer)
        {
            TweetinviContainer = tweetinviContainer;
        }

        public ITweetinviContainer TweetinviContainer { get; }
    }
}