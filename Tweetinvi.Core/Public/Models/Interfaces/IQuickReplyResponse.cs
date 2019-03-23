namespace Tweetinvi.Models
{
    public interface IQuickReplyResponse
    {
        QuickReplyType Type { get; }
        string Metadata { get; }
    }
}
