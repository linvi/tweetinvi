namespace Tweetinvi.Core.Parameters
{
    public interface IMessagesReceivedParameters : IMessagesRetrieveRequestParametersBase
    {
        bool SkipStatus { get; set; }
    }

    public class MessagesReceivedParameters : CustomRequestParameters, IMessagesReceivedParameters
    {
        public MessagesReceivedParameters()
        {
            MaximumNumberOfMessagesToRetrieve = TweetinviConsts.MESSAGE_GET_COUNT;
            FullText = true;
            IncludeEntities = true;
        }

        public int MaximumNumberOfMessagesToRetrieve { get; set; }
        public long? SinceId { get; set; }
        public long? MaxId { get; set; }
        public bool IncludeEntities { get; set; }
        public bool FullText { get; set; }

        public bool SkipStatus { get; set; }
    }
}