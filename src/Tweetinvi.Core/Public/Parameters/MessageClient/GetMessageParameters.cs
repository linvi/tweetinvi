namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/direct-messages/sending-and-receiving/api-reference/get-event
    /// </summary>
    public interface IGetMessageParameters : ICustomRequestParameters
    {
        long MessageId { get; set; }
    }

    public class GetMessageParameters : CustomRequestParameters, IGetMessageParameters
    {
        public GetMessageParameters(long messageId)
        {
            MessageId = messageId;
        }

        public long MessageId { get; set; }
    }
}