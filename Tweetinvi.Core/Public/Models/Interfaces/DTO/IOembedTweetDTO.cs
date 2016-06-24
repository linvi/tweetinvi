namespace Tweetinvi.Models.DTO
{
    public interface IOEmbedTweetDTO
    {
        string AuthorName { get; set; }
        string AuthorURL { get; set; }

        string HTML { get; set; }
        string URL { get; set; }
        string ProviderURL { get; set; }
        
        double Width { get; set; }
        double Height { get; set; }

        string Version { get; set; }
        string Type { get; set; }
        string CacheAge { get; set; }
    }
}