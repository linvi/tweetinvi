using System;
using Tweetinvi.Streaming.Events;

namespace Tweetinvi.Events
{
    /// <summary>
    /// Event informing that the processing of messages is too slow.
    /// <para>
    /// If you receive such message, please process the data received by the stream in another thread,
    /// or send the objects to a queue.
    /// </para>
    /// </summary>
    public class WarningFallingBehindEventArgs : EventArgs
    {
        public WarningFallingBehindEventArgs(IWarningMessageFallingBehind warningMessage)
        {
            WarningMessage = warningMessage;
        }

        public IWarningMessageFallingBehind WarningMessage { get; }
    }
}