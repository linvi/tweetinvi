namespace Tweetinvi.Core.Interfaces.Models.StreamMessages
{
    public interface IWarningMessage
    {
        string Code { get; }
        string Message { get; }
    }
}