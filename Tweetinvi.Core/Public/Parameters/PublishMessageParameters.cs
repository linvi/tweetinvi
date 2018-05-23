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
    }

    /// <summary>
    /// https://developer.twitter.com/en/docs/direct-messages/sending-and-receiving/api-reference/new-event
    /// </summary>
    public class PublishMessageParameters : CustomRequestParameters, IPublishMessageParameters
    {
        public PublishMessageParameters(string text, long recipientId)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text), "Message Text cannot be null or empty.");
            }

            Text = text;
            RecipientId = recipientId;
        }

        public string Text { get; }

        public long RecipientId { get; }
    }
}