namespace Tweetinvi.Core.Interfaces.Models
{
    public interface IFriendshipAuthorizations
    {
        bool RetweetsEnabled { get; set; }
        bool DeviceNotificationEnabled { get; set; }
    }
}