using System;
using Tweetinvi.Core.Interfaces.Models.StreamMessages;

namespace Tweetinvi.Core.Events.EventArguments
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