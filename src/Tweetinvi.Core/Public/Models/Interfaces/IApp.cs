namespace Tweetinvi.Models
{
    public interface IApp
    {
        /// <summary>
        /// Application id
        /// </summary>
        long Id { get; }

        /// <summary>
        /// Application name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Application url
        /// </summary>
        string Url { get; }
    }
}
