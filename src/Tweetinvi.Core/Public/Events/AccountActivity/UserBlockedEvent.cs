using System;
using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    public enum UserBlockedRaisedInResultOf
    {
        /// <summary>
        /// This case should not happen and is here in case Twitter changes when they trigger the Blocked event.
        /// If you happen to receive this mode, please report to Tweetinvi your case ideally with the associated json.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The account user has blocked another user
        /// </summary>
        AccountUserBlockingAnotherUser,
    }

    /// <summary>
    /// Event information when a user is blocked.
    /// </summary>
    public class UserBlockedEvent : BaseAccountActivityEventArgs<UserBlockedRaisedInResultOf>
    {
        public UserBlockedEvent(AccountActivityEvent<Tuple<IUser, IUser>> eventInfo) : base(eventInfo)
        {
            BlockedBy = eventInfo.Args.Item1;
            BlockedUser = eventInfo.Args.Item2;

            InResultOf = GetInResultOf();
        }

        /// <summary>
        /// The user who got blocked
        /// </summary>
        public IUser BlockedUser { get; }

        /// <summary>
        /// The user who blocked
        /// </summary>
        public IUser BlockedBy { get; }

        private UserBlockedRaisedInResultOf GetInResultOf()
        {
            if (BlockedBy.Id == AccountUserId)
            {
                return UserBlockedRaisedInResultOf.AccountUserBlockingAnotherUser;
            }

            return UserBlockedRaisedInResultOf.Unknown;
        }
    }
}
