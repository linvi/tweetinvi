namespace Tweetinvi.Models
{
    public interface IQuickReplyOption
    {
        string Label { get; }
        string Description { get; }
        string Metadata { get; }
    }
}
