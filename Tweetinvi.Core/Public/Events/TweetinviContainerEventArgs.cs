using System;
using Tweetinvi.Core.Injectinvi;

namespace Tweetinvi.Events
{
    public class TweetinviContainerEventArgs : EventArgs
    {
        public TweetinviContainerEventArgs(ITweetinviContainer container)
        {
            TweetinviContainer = container;
        }

        public ITweetinviContainer TweetinviContainer { get; private set; }
    }
}