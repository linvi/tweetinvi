namespace Tweetinvi.Models
{
    /// <summary>
    /// Object containing information to uniquely identify a user.
    /// </summary>
    public interface IUserIdentifier : ITwitterIdentifier
    {
        /// <summary>
        /// User screen name
        /// </summary>
        string ScreenName { get; }
    }
}