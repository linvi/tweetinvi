using System;
using Tweetinvi.Models;

namespace Tweetinvi.Parameters
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
    }
}