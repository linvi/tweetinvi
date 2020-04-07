using System;
using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    public enum UserUnblockedRaisedInResultOf
    {
        /// <summary>
        /// This case should not happen and is here in case Twitter changes when they trigger the Unblocked event.
        /// If you happen to receive this mode, please report to Tweetinvi your case ideally with the associated json.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The account user has unblocked another user
        /// </summary>
        AccountUserUnblockingAnotherUser,
    }

    /// <summary>
    /// Event information when a user is unblocked.
    /// </summary>
    public class UserUnblockedEvent : BaseAccountActivityEventArgs<UserUnblockedRaisedInResultOf>
    {
        public UserUnblockedEvent(AccountActivityEvent<Tuple<IUser, IUser>> eventInfo) : base(eventInfo)
        {
            UnblockedBy = eventInfo.Args.Item1;
            UnblockedUser = eventInfo.Args.Item2;

            InResultOf = GetInResultOf();
        }

        /// <summary>
        /// THe user who got unblocked
        /// </summary>
        public IUser UnblockedUser { get; }

        /// <summary>
        /// The user who performed the action of unblocking another user
        /// </summary>
        public IUser UnblockedBy { get; }

        private UserUnblockedRaisedInResultOf GetInResultOf()
        {
            if (UnblockedBy.Id == AccountUserId)
            {
                return UserUnblockedRaisedInResultOf.AccountUserUnblockingAnotherUser;
            }

            return UserUnblockedRaisedInResultOf.Unknown;
        }
    }
}
