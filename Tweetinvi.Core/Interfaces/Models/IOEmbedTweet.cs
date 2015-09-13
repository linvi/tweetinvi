namespace Tweetinvi.Core.Interfaces.Models
{
    public interface IOEmbedTweet
    {
        string AuthorName { get; }
        string AuthorURL { get; }
        string HTML { get; }
        string URL { get; }
        string ProviderURL { get; }
        double Width { get; }
        double Height { get; }
        string Version { get; }
        string Type { get; }
        string CacheAge { get; }
    }
}