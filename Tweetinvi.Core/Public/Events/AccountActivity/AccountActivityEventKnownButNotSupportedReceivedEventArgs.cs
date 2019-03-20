using System;

namespace Tweetinvi.Events
{
    /// <summary>
    /// The json message was understood, but Tweetinvi could not properly analyze it
    /// as it the reason why it was raised are not yet supported
    /// </summary>
    public class EventKnownButNotSupportedReceivedEventArgs : EventArgs
    {
        public EventKnownButNotSupportedReceivedEventArgs(string fullJson, BaseAccountActivityEventArgs accountActivityEventArgs)
        {
            FullJson = fullJson;
            EventArgs = accountActivityEventArgs;
        }

        /// <summary>
        /// The json object that Tweetinvi could not fully understand
        /// </summary>
        public string FullJson { get; }

        public BaseAccountActivityEventArgs EventArgs { get; }
    }
}
