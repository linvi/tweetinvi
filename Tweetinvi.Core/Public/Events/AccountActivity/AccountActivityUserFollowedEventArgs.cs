using System;
using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    public enum UserFollowedRaisedInResultOf
    {
        /// <summary>
        /// The account user is now following another user 
        /// </summary>
        AccountUserFollowingAnotherUser,

        /// <summary>
        /// The account user is being followed by another user
        /// </summary>
        AnotherUserFollowingAccountUser,

        /// <summary>
        /// This case should not happen and is here in case Twitter changes when they trigger the Followed event.
        /// If you happen to receive this mode, please report to Tweetinvi your case ideally with the associated json.
        /// </summary>
        Unknown
    }

    public class AccountActivityUserFollowedEventArgs : BaseAccountActivityEventArgs
    {
        public AccountActivityUserFollowedEventArgs(AccountActivityEvent<Tuple<IUser, IUser>> eventInfo) : base(eventInfo)
        {
            FollowedBy = eventInfo.Args.Item1;
            UserFollowed = eventInfo.Args.Item2;

            InResultOf = GetInResultOf();
        }

        public IUser UserFollowed { get; }
        public IUser FollowedBy { get; }

        public UserFollowedRaisedInResultOf InResultOf { get; }

        private UserFollowedRaisedInResultOf GetInResultOf()
        {
            if (UserFollowed.Id == AccountUserId)
            {
                return UserFollowedRaisedInResultOf.AnotherUserFollowingAccountUser;
            }

            if (UserFollowed.Id != AccountUserId && FollowedBy.Id == AccountUserId)
            {
                return UserFollowedRaisedInResultOf.AccountUserFollowingAnotherUser;
            }

            return UserFollowedRaisedInResultOf.Unknown;
        }
    }
}
