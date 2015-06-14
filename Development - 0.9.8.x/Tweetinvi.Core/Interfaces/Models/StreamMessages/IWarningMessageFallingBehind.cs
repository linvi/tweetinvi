namespace Tweetinvi.Core.Interfaces.Models.StreamMessages
{
    public interface IWarningMessageFallingBehind : IWarningMessage
    {
        int PercentFull { get; }
    }
}