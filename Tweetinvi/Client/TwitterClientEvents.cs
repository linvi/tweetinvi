using System;
using Tweetinvi.Events;

namespace Tweetinvi.Client
{
    public class TwitterClientEvents
    {
        public event EventHandler<TweetinviContainerEventArgs> BeforeRegistrationCompletes;
    }
}