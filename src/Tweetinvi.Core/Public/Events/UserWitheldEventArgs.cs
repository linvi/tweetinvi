using System;
using Tweetinvi.Streaming.Events;

namespace Tweetinvi.Events
{
    /// <summary>
    /// Event informing that some user event happened by were blocked by the country
    /// </summary>
    public class UserWitheldEventArgs : EventArgs
    {
        public UserWitheldEventArgs(IUserWitheldInfo userWitheldInfo)
        {
            UserWitheldInfo = userWitheldInfo;
        }

        public IUserWitheldInfo UserWitheldInfo { get; }
    }
}