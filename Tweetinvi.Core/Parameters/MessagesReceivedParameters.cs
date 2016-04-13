namespace Tweetinvi.Core.Parameters
{
    /// <summary>
    /// https://dev.twitter.com/rest/reference/get/direct_messages
    /// </summary>
    public interface IMessagesReceivedParameters : IMessagesRetrieveRequestParametersBase
    {
        /// <summary>
        /// Tweets will not be included in the returned user objects.
        /// </summary>
        bool SkipStatus { get; set; }
    }

    /// <summary>
    /// https://dev.twitter.com/rest/reference/get/direct_messages
    /// </summary>
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