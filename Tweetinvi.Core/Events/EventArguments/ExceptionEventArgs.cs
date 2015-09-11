using System;
using Tweetinvi.Core.Interfaces.Models.StreamMessages;

namespace Tweetinvi.Core.Events.EventArguments
{
    public class ExceptionEventArgs : EventArgs
    {
        public ExceptionEventArgs(Exception ex)
        {
            Exception = ex;
        }

        public Exception Exception { get; private set; }
    }

    public class StreamExceptionEventArgs : EventArgs
    {
        public StreamExceptionEventArgs(Exception ex, IDisconnectMessage disconnectMessage = null)
        {
            Exception = ex;
            DisconnectMessage = disconnectMessage;
        }

        public Exception Exception { get; private set; }
        public IDisconnectMessage DisconnectMessage { get; private set; }
    }
}