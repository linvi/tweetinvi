namespace Tweetinvi.Core.Interfaces.Models
{
    /// <summary>
    /// Object containing information to uniquely identify a user.
    /// </summary>
    public interface IUserIdentifier
    {
        /// <summary>
        /// User Id
        /// </summary>
        long Id { get; set; }

        /// <summary>
        /// User Id as a string
        /// </summary>
        string IdStr { get; set; }

        /// <summary>
        /// User screen name
        /// </summary>
        string ScreenName { get; set; }
    }
}