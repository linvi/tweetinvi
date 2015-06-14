namespace Tweetinvi.Core.Interfaces.Parameters
{
    public interface IMessageGetLatestsReceivedRequestParameters : IMessagesRetrieveRequestParametersBase
    {
        bool SkipStatus { get; set; }
    }
}