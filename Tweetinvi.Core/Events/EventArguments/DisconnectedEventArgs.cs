using System;
using Tweetinvi.Core.Interfaces.Models.StreamMessages;

namespace Tweetinvi.Core.Events.EventArguments
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