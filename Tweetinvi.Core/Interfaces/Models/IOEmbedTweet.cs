namespace Tweetinvi.Core.Interfaces.Models
{
    public interface IOEmbedTweet
    {
        /// <summary>
        /// Author of the tweet.
        /// </summary>
        string AuthorName { get; }

        /// <summary>
        /// Hyperlink to the author public page.
        /// </summary>
        string AuthorURL { get; }

        /// <summary>
        /// HTML generated to display the tweet on your website.
        /// </summary>
        string HTML { get; }

        /// <summary>
        /// Hyperlink to the tweet.
        /// </summary>
        string URL { get; }

        string ProviderURL { get; }

        /// <summary>
        /// Width of the div containing the embedded tweet.
        /// </summary>
        double Width { get; }

        /// <summary>
        /// Width of the div containing the embedded tweet.
        /// </summary>
        double Height { get; }
        string Version { get; }
        string Type { get; }
        string CacheAge { get; }
    }
}