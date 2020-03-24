using System;
using Tweetinvi.Streaming.Events;

namespace Tweetinvi.Events
{
    public class StreamStoppedEventArgs : EventArgs
    {
        public StreamStoppedEventArgs()
        {
        }

        public StreamStoppedEventArgs(Exception ex, IDisconnectMessage disconnectMessage = null)
        {
            Exception = ex;
            DisconnectMessage = disconnectMessage;
        }

        public Exception Exception { get; }
        public IDisconnectMessage DisconnectMessage { get; }
    }
}