namespace Tweetinvi.Core.Parameters
{
    public interface IMessagesRetrieveRequestParametersBase : ICustomRequestParameters
    {
        int MaximumNumberOfMessagesToRetrieve { get; set; }

        long? SinceId { get; set; }
        long? MaxId { get; set; }

        bool IncludeEntities { get; set; }
        bool FullText { get; set; }
    }
}