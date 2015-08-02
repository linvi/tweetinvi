namespace Tweetinvi.Core.Interfaces.Models
{
    public interface ITrend
    {
        string Name { get; set; }
        string URL { get; set; }
        string Query { get; set; }
        string PromotedContent { get; set; }
    }
}