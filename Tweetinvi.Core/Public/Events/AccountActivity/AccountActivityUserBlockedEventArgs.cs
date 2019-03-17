using System;
using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    public enum UserBlockedRaisedInResultOf
    {
        /// <summary>
        /// The account user has blocked another user 
        /// </summary>
        AccountUserBlockingAnotherUser,

        /// <summary>
        /// This case should not happen and is here in case Twitter changes when they trigger the Blocked event.
        /// If you happen to receive this mode, please report to Tweetinvi your case ideally with the associated json.
        /// </summary>
        Unknown
    }

    public class AccountActivityUserBlockedEventArgs : BaseAccountActivityEventArgs
    {
        public AccountActivityUserBlockedEventArgs(AccountActivityEvent<Tuple<IUser, IUser>> eventInfo) : base(eventInfo)
        {
            BlockedBy = eventInfo.Args.Item1;
            UserBlocked = eventInfo.Args.Item2;

            InResultOf = GetInResultOf();
        }

        public IUser UserBlocked { get; }
        public IUser BlockedBy { get; }

        public UserBlockedRaisedInResultOf InResultOf { get; }

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
