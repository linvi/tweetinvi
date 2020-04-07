namespace Tweetinvi.Events
{
    public enum UserRevokedAppPermissionsInResultOf
    {
        /// <summary>
        /// This case should not happen and is here in case Twitter changes when they trigger the PermissionsRevoked event.
        /// If you happen to receive this mode, please report to Tweetinvi your case ideally with the associated json.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The account user removed permission for a specific app
        /// </summary>
        AccountUserRemovingAppPermissions,
    }

    /// <summary>
    /// Event information when a user revokes application permissions.
    /// </summary>
    public class UserRevokedAppPermissionsEvent : BaseAccountActivityEventArgs<UserRevokedAppPermissionsInResultOf>
    {
        public UserRevokedAppPermissionsEvent(
            AccountActivityEvent activityEvent,
            long userId,
            long appId) : base(activityEvent)
        {
            UserId = userId;
            AppId = appId;

            InResultOf = GetInResultOf();
        }

        /// <summary>
        /// Id of the user who revoked the application permissions
        /// </summary>
        public long UserId { get; }

        /// <summary>
        /// Application that go its permissions revoked
        /// </summary>
        public long AppId { get; }

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
