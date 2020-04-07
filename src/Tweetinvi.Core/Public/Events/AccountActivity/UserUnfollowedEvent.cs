using System;
using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    public enum UserUnfollowedRaisedInResultOf
    {
        /// <summary>
        /// This case should not happen and is here in case Twitter changes when they trigger the Unfollowed event.
        /// If you happen to receive this mode, please report to Tweetinvi your case ideally with the associated json.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The account user is no longer following another user
        /// </summary>
        AccountUserUnfollowingAnotherUser,
    }

    /// <summary>
    /// Event information when a user is unfollowed.
    /// </summary>
    public class UserUnfollowedEvent : BaseAccountActivityEventArgs<UserUnfollowedRaisedInResultOf>
    {
        public UserUnfollowedEvent(AccountActivityEvent<Tuple<IUser, IUser>> eventInfo) : base(eventInfo)
        {
            UnfollowedBy = eventInfo.Args.Item1;
            UnfollowedUser = eventInfo.Args.Item2;

            InResultOf = GetInResultOf();
        }

        /// <summary>
        /// User who just got unfollowed
        /// </summary>
        public IUser UnfollowedUser { get; }

        /// <summary>
        /// User who performed the action of unfollowing another user
        /// </summary>
        public IUser UnfollowedBy { get; }

        private UserUnfollowedRaisedInResultOf GetInResultOf()
        {
            if (UnfollowedBy.Id == AccountUserId)
            {
                return UserUnfollowedRaisedInResultOf.AccountUserUnfollowingAnotherUser;
            }

            return UserUnfollowedRaisedInResultOf.Unknown;
        }
    }
}
