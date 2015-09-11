using System;

namespace Tweetinvi.Core.Events.EventArguments
{
    public class UnmanagedMessageReceivedEventArgs : EventArgs
    {
        public UnmanagedMessageReceivedEventArgs(string json)
        {
            JsonMessageReceived = json;
        }

        public string JsonMessageReceived { get; private set; }
    }
}