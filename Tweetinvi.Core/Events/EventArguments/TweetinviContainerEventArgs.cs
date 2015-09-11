using System;
using Tweetinvi.Core.Injectinvi;

namespace Tweetinvi.Core.Events.EventArguments
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