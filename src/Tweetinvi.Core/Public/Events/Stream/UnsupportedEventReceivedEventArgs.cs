using System;

namespace Tweetinvi.Events
{
    public class UnsupportedEventReceivedEventArgs : EventArgs
    {
        public UnsupportedEventReceivedEventArgs(string json)
        {
            JsonMessageReceived = json;
        }

        public string JsonMessageReceived { get; }
    }
}