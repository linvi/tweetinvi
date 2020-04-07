using System;
using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    public enum UserFollowedRaisedInResultOf
    {
        /// <summary>
        /// This case should not happen and is here in case Twitter changes when they trigger the Followed event.
        /// If you happen to receive this mode, please report to Tweetinvi your case ideally with the associated json.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The account user is now following another user
        /// </summary>
        AccountUserFollowingAnotherUser,

        /// <summary>
        /// The account user is being followed by another user
        /// </summary>
        AnotherUserFollowingAccountUser,
    }

    /// <summary>
    /// Event information when a user is being followed.
    /// </summary>
    public class UserFollowedEvent : BaseAccountActivityEventArgs<UserFollowedRaisedInResultOf>
    {
        public UserFollowedEvent(AccountActivityEvent<Tuple<IUser, IUser>> eventInfo) : base(eventInfo)
        {
            FollowedBy = eventInfo.Args.Item1;
            FollowedUser = eventInfo.Args.Item2;

            InResultOf = GetInResultOf();
        }

        /// <summary>
        /// The user who got followed
        /// </summary>
        public IUser FollowedUser { get; }

        /// <summary>
        /// The user who performed the action of following another user
        /// </summary>
        public IUser FollowedBy { get; }


        private UserFollowedRaisedInResultOf GetInResultOf()
        {
            if (FollowedUser.Id == AccountUserId)
            {
                return UserFollowedRaisedInResultOf.AnotherUserFollowingAccountUser;
            }

            if (FollowedUser.Id != AccountUserId && FollowedBy.Id == AccountUserId)
            {
                return UserFollowedRaisedInResultOf.AccountUserFollowingAnotherUser;
            }

            return UserFollowedRaisedInResultOf.Unknown;
        }
    }
}
