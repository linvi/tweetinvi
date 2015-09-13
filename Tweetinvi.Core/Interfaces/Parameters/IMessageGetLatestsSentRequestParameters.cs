namespace Tweetinvi.Core.Interfaces.Parameters
{
    public interface IMessageGetLatestsSentRequestParameters : IMessagesRetrieveRequestParametersBase
    {
        int? PageNumber { get; set; }
    }
}