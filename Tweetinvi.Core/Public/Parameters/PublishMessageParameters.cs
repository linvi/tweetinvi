using System;
using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// https://developer.twitter.com/en/docs/direct-messages/sending-and-receiving/api-reference/new-event
    /// </summary>
    public interface IPublishMessageParameters : ICustomRequestParameters
    {
        string Text { get; }
        long RecipientId { get; }
        long? AttachmentMediaId { get; set; }
        IQuickReplyOption[] QuickReplyOptions { get; set; }
    }

    /// <summary>
    /// https://developer.twitter.com/en/docs/direct-messages/sending-and-receiving/api-reference/new-event
    /// </summary>
    public class PublishMessageParameters : CustomRequestParameters, IPublishMessageParameters
    {
        public PublishMessageParameters(string text, long? recipientId)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text), "Message Text cannot be null or empty.");
            }

            if (recipientId == null)
            {
                throw new ArgumentNullException(nameof(text), "Recipient Text cannot be null or empty.");
            }

            Text = text;
            RecipientId = recipientId.Value;
        }

        public string Text { get; }

        public long RecipientId { get; }

        public long? AttachmentMediaId { get; set; }

        public IQuickReplyOption[] QuickReplyOptions { get; set; }
    }
}