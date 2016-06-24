namespace Tweetinvi.Models
{
    public interface ICategorySuggestion
    {
        string Name { get; }
        string Slug { get; }
        int Size { get; }
    }
}