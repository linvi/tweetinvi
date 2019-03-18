namespace Tweetinvi.Events
{
    public enum UserRevokedAppPermissionsInResultOf
    {
        /// <summary>
        /// The account user removed permission for a specific app
        /// </summary>
        AccountUserRemovingAppPermissions,

        /// <summary>
        /// This case should not happen and is here in case Twitter changes when they trigger the PermissionsRevoked event.
        /// If you happen to receive this mode, please report to Tweetinvi your case ideally with the associated json.
        /// </summary>
        Unknown,
    }

    public class AccountActivityUserRevokedAppPermissionsEventArgs : BaseAccountActivityEventArgs
    {
        public AccountActivityUserRevokedAppPermissionsEventArgs(
            AccountActivityEvent activityEvent,
            long userId,
            long appId) : base(activityEvent)
        {
            UserId = userId;
            AppId = appId;

            InResultOf = GetInResultOf();
        }

        public long UserId { get; }
        public long AppId { get; }
        public UserRevokedAppPermissionsInResultOf InResultOf { get; }

        private UserRevokedAppPermissionsInResultOf GetInResultOf()
        {
            if (AccountUserId == UserId)
            {
                return UserRevokedAppPermissionsInResultOf.AccountUserRemovingAppPermissions;
            }

            return UserRevokedAppPermissionsInResultOf.Unknown;
        }
    }
}
