using System;
using Tweetinvi.Streaming.Events;

namespace Tweetinvi.Events
{
    public class WarningTooManyFollowersEventArgs : EventArgs
    {
        public WarningTooManyFollowersEventArgs(IWarningMessageTooManyFollowers warningMessage)
        {
            WarningMessage = warningMessage;
        }

        public IWarningMessageTooManyFollowers WarningMessage { get; private set; }
    }

    public class WarningFallingBehindEventArgs : EventArgs
    {
        public WarningFallingBehindEventArgs(IWarningMessageFallingBehind warningMessage)
        {
            WarningMessage = warningMessage;
        }

        public IWarningMessageFallingBehind WarningMessage { get; private set; }
    }
}