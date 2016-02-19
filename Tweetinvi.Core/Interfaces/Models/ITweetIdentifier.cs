namespace Tweetinvi.Core.Interfaces.Models
{
    /// <summary>
    /// Identifier allowing to identify a unique tweet.
    /// </summary>
    public interface ITweetIdentifier
    {
        /// <summary>
        /// Id of the Tweet.
        /// </summary>
        long Id { get; }

        /// <summary>
        /// Id of the tweet as a string.
        /// </summary>
        string IdStr { get; }
    }
}