using Tweetinvi.Models;

namespace Tweetinvi.Controllers.Friendship
{
    public class FriendshipAuthorizations : IFriendshipAuthorizations
    {
        public bool RetweetsEnabled { get; set; }
        public bool DeviceNotificationEnabled { get; set; }
    }
}