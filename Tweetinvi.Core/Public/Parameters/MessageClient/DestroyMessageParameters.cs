using Tweetinvi.Models;
using Tweetinvi.Models.DTO.Events;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/direct-messages/sending-and-receiving/guides/direct-message-migration
    /// </summary>
    public interface IDeleteMessageParameters : ICustomRequestParameters
    {
        /// <summary>
        /// Identifier of the message that you want to delete
        /// </summary>
        long MessageId { get; set; }
    }

    /// <inheritdoc/>
    public class DestroyMessageParameters : CustomRequestParameters, IDeleteMessageParameters
    {
        public DestroyMessageParameters(long messageId)
        {
            MessageId = messageId;
        }

        public DestroyMessageParameters(IMessageEventDTO messageEvent)
        {
            MessageId = messageEvent.Id;
        }

        public DestroyMessageParameters(IMessage message)
        {
            MessageId = message.Id;
        }

        /// <inheritdoc/>
        public long MessageId { get; set; }
    }
}