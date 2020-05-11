using System;

namespace Tweetinvi.Events
{
    /// <summary>
    /// Event to inform that the limit of tweets that can be received by the application has been reached.
    /// </summary>
    public class LimitReachedEventArgs : EventArgs
    {
        public LimitReachedEventArgs(int numberOfTweetsNotReceived)
        {
            NumberOfTweetsNotReceived = numberOfTweetsNotReceived;
        }

        public int NumberOfTweetsNotReceived { get; }
    }
}