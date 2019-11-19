using System;
using Tweetinvi.Streaming.Events;

namespace Tweetinvi.Events
{
    public class StreamExceptionEventArgs : EventArgs
    {
        public StreamExceptionEventArgs(Exception ex, IDisconnectMessage disconnectMessage = null)
        {
            Exception = ex;
            DisconnectMessage = disconnectMessage;
        }

        public Exception Exception { get; }
        public IDisconnectMessage DisconnectMessage { get; }
    }
}