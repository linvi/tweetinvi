using System;
using Tweetinvi.Models;
using Tweetinvi.Streaming.Events;

namespace Tweetinvi.Events
{
    public class UserEventArgs : EventArgs
    {
        public UserEventArgs(IUser target, long sourceId)
        {
            SourceId = sourceId;
            Target = target;
        }

        public long SourceId { get; }
        public IUser Target { get; }
    }

    public class UserWitheldEventArgs : EventArgs
    {
        public UserWitheldEventArgs(IUserWitheldInfo userWitheldInfo)
        {
            UserWitheldInfo = userWitheldInfo;
        }

        public IUserWitheldInfo UserWitheldInfo { get; private set; }
    }
}