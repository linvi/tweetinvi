using System;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Parameters
{
    /// <summary>
    /// https://dev.twitter.com/rest/reference/post/direct_messages/new
    /// </summary>
    public interface IPublishMessageParameters : ICustomRequestParameters
    {
        string Text { get; }
        long RecipientId { get; }
        string RecipientScreenName { get; }
        IUserIdentifier Recipient { get; }

        IMessageDTO Message { get; }
    }

    /// <summary>
    /// https://dev.twitter.com/rest/reference/post/direct_messages/new
    /// </summary>
    public class PublishMessageParameters : CustomRequestParameters, IPublishMessageParameters
    {
        public PublishMessageParameters(string text, IUserIdentifier recipient)
        {
            Initialize(text, recipient);
        }

        public PublishMessageParameters(string text, long recipientId)
        {
            Initialize(text, new UserIdentifier(recipientId));
        }

        public PublishMessageParameters(string text, string recipientScreenName) : this(text, new UserIdentifier(recipientScreenName))
        {
            Initialize(text, new UserIdentifier(recipientScreenName));
        }

        public PublishMessageParameters(IMessageDTO message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("Message cannot be null.");
            }

            Initialize(message.Text, message.Recipient);

            Message = message;
        }

        public PublishMessageParameters(IMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("Message cannot be null.");
            }

            Initialize(message.Text, message.Recipient);

            Message = message.MessageDTO;
        }

        private void Initialize(string text, IUserIdentifier recipient)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("Message Text cannot be null or empty.");
            }

            if (recipient == null)
            {
                throw new ArgumentNullException("Message recipient cannot be null.");
            }

            Text = text;
            Recipient = recipient;
        }

        public string Text { get; private set; }

        public long RecipientId
        {
            get { return Recipient.Id; }
        }

        public string RecipientScreenName
        {
            get { return Recipient.ScreenName; }
        }

        public IUserIdentifier Recipient { get; private set; }

        public IMessageDTO Message { get; private set; }
    }
}