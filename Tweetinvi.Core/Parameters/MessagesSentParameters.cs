namespace Tweetinvi.Core.Parameters
{
    /// <summary>
    /// https://dev.twitter.com/rest/reference/get/direct_messages/sent
    /// </summary>
    public interface IMessagesSentParameters : IMessagesRetrieveRequestParametersBase
    {
        /// <summary>
        /// Specifies the page of results to retrieve.
        /// </summary>
        int? PageNumber { get; set; }
    }

    /// <summary>
    /// https://dev.twitter.com/rest/reference/get/direct_messages/sent
    /// </summary>
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