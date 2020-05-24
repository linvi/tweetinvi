using System;
using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    public enum UserUnmutedRaisedInResultOf
    {
        /// <summary>
        /// This case should not happen and is here in case Twitter changes when they trigger the Unmuted event.
        /// If you happen to receive this mode, please report to Tweetinvi your case ideally with the associated json.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The account user has Unmuted another user
        /// </summary>
        AccountUserUnmutingAnotherUser,
    }

    /// <summary>
    /// Event information when a user is unmuted.
    /// </summary>
    public class UserUnmutedEvent : BaseAccountActivityEventArgs<UserUnmutedRaisedInResultOf>
    {
        public UserUnmutedEvent(AccountActivityEvent<Tuple<IUser, IUser>> eventInfo) : base(eventInfo)
        {
            UnmutedBy = eventInfo.Args.Item1;
            UnmutedUser = eventInfo.Args.Item2;

            InResultOf = GetInResultOf();
        }

        /// <summary>
        /// User who stopped being muted
        /// </summary>
        public IUser UnmutedUser { get; }

        /// <summary>
        /// User who performed the action of muting another user
        /// </summary>
        public IUser UnmutedBy { get; }

        private UserUnmutedRaisedInResultOf GetInResultOf()
        {
            if (UnmutedBy.Id == AccountUserId)
            {
                return UserUnmutedRaisedInResultOf.AccountUserUnmutingAnotherUser;
            }

            return UserUnmutedRaisedInResultOf.Unknown;
        }
    }
}
