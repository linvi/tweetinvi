using Tweetinvi.Core.Interfaces.Parameters;

namespace Tweetinvi.Core.Parameters
{
    public class GetLatestMessagesReceivedRequestParameters : CustomRequestParameters, IMessageGetLatestsReceivedRequestParameters
    {
        public GetLatestMessagesReceivedRequestParameters()
        {
            MaximumNumberOfMessagesToRetrieve = TweetinviConsts.MESSAGE_GET_COUNT;

            IncludeEntities = true;
        }

        public int MaximumNumberOfMessagesToRetrieve { get; set; }
        public long? SinceId { get; set; }
        public long? MaxId { get; set; }
        public bool IncludeEntities { get; set; }

        public bool SkipStatus { get; set; }
    }
}