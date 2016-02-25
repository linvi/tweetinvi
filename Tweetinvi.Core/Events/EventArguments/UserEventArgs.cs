using System;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Models.StreamMessages;

namespace Tweetinvi.Core.Events.EventArguments
{
    public class UserEventArgs : EventArgs
    {
        public UserEventArgs(IUser user)
        {
            User = user;
        }

        public IUser User { get; private set; }
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
        public UserFollowedEventArgs(IUser user) : base(user)
        {
        }
    }

    public class UserBlockedEventArgs : UserEventArgs
    {
        public UserBlockedEventArgs(IUser user) : base(user)
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
}