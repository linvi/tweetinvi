using Tweetinvi.Core.Interfaces.Parameters;

namespace Tweetinvi.Core.Parameters
{
    public class GetLatestMessagesSentRequestParameters : CustomRequestParameters, IMessageGetLatestsSentRequestParameters
    {
        public GetLatestMessagesSentRequestParameters()
        {
            MaximumNumberOfMessagesToRetrieve = TweetinviConsts.MESSAGE_GET_COUNT;
        }

        public int MaximumNumberOfMessagesToRetrieve { get; set; }
        public long? SinceId { get; set; }
        public long? MaxId { get; set; }
        public bool IncludeEntities { get; set; }

        public int? PageNumber { get; set; }
    }
}