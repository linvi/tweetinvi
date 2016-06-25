using System;

namespace Tweetinvi.Events
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