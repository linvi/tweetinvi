using System;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Parameters
{
    public interface IMessagePublishParameters : ICustomRequestParameters
    {
        string Text { get; }
        long RecipientId { get; }
        string RecipientScreenName { get; }
        IUserIdentifier Recipient { get; }

        IMessageDTO Message { get; }
    }

    public class MessagePublishParameters : CustomRequestParameters, IMessagePublishParameters
    {
        public MessagePublishParameters(string text, IUserIdentifier recipient)
        {
            Initialize(text, recipient);
        }

        public MessagePublishParameters(string text, long recipientId)
        {
            Initialize(text, new UserIdentifier(recipientId));
        }

        public MessagePublishParameters(string text, string recipientScreenName) : this(text, new UserIdentifier(recipientScreenName))
        {
            Initialize(text, new UserIdentifier(recipientScreenName));
        }

        public MessagePublishParameters(IMessageDTO message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("Message cannot be null.");
            }

            Initialize(message.Text, message.Recipient);

            Message = message;
        }

        public MessagePublishParameters(IMessage message)
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