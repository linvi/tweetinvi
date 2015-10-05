namespace Tweetinvi.Core.Parameters
{
    public interface IMessagesSentParameters : IMessagesRetrieveRequestParametersBase
    {
        int? PageNumber { get; set; }
    }

    public class MessagesSentParameters : CustomRequestParameters, IMessagesSentParameters
    {
        public MessagesSentParameters()
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

        public int? PageNumber { get; set; }
    }
}