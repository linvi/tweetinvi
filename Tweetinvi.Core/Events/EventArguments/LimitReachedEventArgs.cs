using System;

namespace Tweetinvi.Core.Events.EventArguments
{
    public class LimitReachedEventArgs : EventArgs
    {
        public LimitReachedEventArgs(int numberOfTweetsNotReceived)
        {
            NumberOfTweetsNotReceived = numberOfTweetsNotReceived;
        }

        public int NumberOfTweetsNotReceived { get; private set; }
    }
}