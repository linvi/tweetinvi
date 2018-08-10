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

    public class AuthenticatedUserUpdatedEventArgs : EventArgs
    {
        public AuthenticatedUserUpdatedEventArgs(IAuthenticatedUser authenticatedUser)
        {
            AuthenticatedUser = authenticatedUser;
        }

        public IAuthenticatedUser AuthenticatedUser { get; private set; }
    }

    public class UserFollowedEventArgs : UserEventArgs
    {
        public UserFollowedEventArgs(IUser target, long sourceId) : base(target, sourceId)
        {
        }
    }

    public class UserBlockedEventArgs : UserEventArgs
    {
        public UserBlockedEventArgs(IUser target, long sourceId) : base(target, sourceId)
        {
        }
    }

    public class UserMutedEventArgs : UserEventArgs
    {
        public UserMutedEventArgs(IUser target, long sourceId) : base(target, sourceId)
        {

        }
    }

    public class UserWitheldEventArgs : EventArgs
    {
        public UserWitheldEventArgs(IUserWitheldInfo userWitheldInfo)
        {
            UserWitheldInfo = userWitheldInfo;
        }

        public IUserWitheldInfo UserWitheldInfo { get; private set; }
    }

    public class UserRevokedAppPermissionsEventArgs : EventArgs
    {
        public long UserId { get; set; }
        public long AppId { get; set; }
    }
}