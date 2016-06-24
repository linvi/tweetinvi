namespace Tweetinvi.Streaming.Events
{
    public interface IWarningMessageFallingBehind : IWarningMessage
    {
        int PercentFull { get; }
    }
}