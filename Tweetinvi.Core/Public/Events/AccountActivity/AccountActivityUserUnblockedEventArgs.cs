using System;
using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    public enum UserUnblockedRaisedInResultOf
    {
        /// <summary>
        /// The account user has unblocked another user 
        /// </summary>
        AccountUserUnblockingAnotherUser,

        /// <summary>
        /// This case should not happen and is here in case Twitter changes when they trigger the UnBlocked event.
        /// If you happen to receive this mode, please report to Tweetinvi your case ideally with the associated json.
        /// </summary>
        Unknown
    }

    public class AccountActivityUserUnblockedEventArgs : BaseAccountActivityEventArgs<UserUnblockedRaisedInResultOf>
    {
        public AccountActivityUserUnblockedEventArgs(AccountActivityEvent<Tuple<IUser, IUser>> eventInfo) : base(eventInfo)
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
