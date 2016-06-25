using System;
using Tweetinvi.Streaming.Events;

namespace Tweetinvi.Events
{
    public class DisconnectedEventArgs : EventArgs
    {
        public DisconnectedEventArgs(IDisconnectMessage disconnectMessage)
        {
            DisconnectMessage = disconnectMessage;
        }

        public IDisconnectMessage DisconnectMessage { get; private set; }
    }
}