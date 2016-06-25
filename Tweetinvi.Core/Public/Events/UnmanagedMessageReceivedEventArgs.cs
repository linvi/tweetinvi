using System;

namespace Tweetinvi.Events
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