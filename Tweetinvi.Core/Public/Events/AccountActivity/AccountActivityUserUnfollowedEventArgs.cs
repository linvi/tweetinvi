using System;
using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    public enum UserUnfollowedRaisedInResultOf
    {
        /// <summary>
        /// The account user is no longer following another user 
        /// </summary>
        AccountUserUnfollowingAnotherUser,

        /// <summary>
        /// This case should not happen and is here in case Twitter changes when they trigger the Unfollowed event.
        /// If you happen to receive this mode, please report to Tweetinvi your case ideally with the associated json.
        /// </summary>
        Unknown
    }

    public class AccountActivityUserUnfollowedEventArgs : BaseAccountActivityEventArgs
    {
        public AccountActivityUserUnfollowedEventArgs(AccountActivityEvent<Tuple<IUser, IUser>> eventInfo) : base(eventInfo)
        {
            UnfollowedBy = eventInfo.Args.Item1;
            UserUnfollowed = eventInfo.Args.Item2;

            InResultOf = GetInResultOf();
        }

        public IUser UserUnfollowed { get; }
        public IUser UnfollowedBy { get; }

        public UserUnfollowedRaisedInResultOf InResultOf { get; }

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
