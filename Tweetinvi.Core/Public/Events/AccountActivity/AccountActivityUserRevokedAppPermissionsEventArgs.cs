using System;

namespace Tweetinvi.Events
{
    public class AccountActivityUserRevokedAppPermissionsEventArgs : EventArgs
    {
        public long UserId { get; set; }
        public long AppId { get; set; }
    }
}
