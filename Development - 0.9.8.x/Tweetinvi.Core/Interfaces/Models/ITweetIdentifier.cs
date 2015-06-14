namespace Tweetinvi.Core.Interfaces.Models
{
    public interface ITweetIdentifier
    {
        /// <summary>
        /// id of the Tweet
        /// </summary>
        long Id { get; }

        /// <summary>
        /// Id of tweet as a string
        /// </summary>
        string IdStr { get; }
    }
}