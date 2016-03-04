namespace Tweetinvi.Core.Interfaces.Models
{
    public interface IFriendshipAuthorizations
    {
        /// <summary>
        /// Can the user retweet you.
        /// </summary>
        bool RetweetsEnabled { get; set; }

        /// <summary>
        /// Can you receive notification from this user.
        /// </summary>
        bool DeviceNotificationEnabled { get; set; }
    }
}