using System;
using Tweetinvi.Streaming.Events;

namespace Tweetinvi.Events
{
    /// <summary>
    /// The stream was disconnected
    /// </summary>
    public class DisconnectedEventArgs : EventArgs
    {
        public DisconnectedEventArgs(IDisconnectMessage disconnectMessage)
        {
            DisconnectMessage = disconnectMessage;
        }

        public IDisconnectMessage DisconnectMessage { get; }
    }
}