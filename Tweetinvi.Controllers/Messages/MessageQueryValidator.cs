using System;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Messages
{
    public interface IMessageQueryValidator
    {
        bool IsMessageTextValid(string message);

        void ThrowIfMessageCannotBePublished(IPublishMessageParameters parameters);
        void ThrowIfMessageCannotBeDestroyed(IEventDTO messageEvent);
        void ThrowIfMessageCannotBeDestroyed(long messageId);
    }

    public class MessageQueryValidator : IMessageQueryValidator
    {
        private readonly IUserQueryValidator _userQueryValidator;

        public MessageQueryValidator(IUserQueryValidator userQueryValidator)
        {
            _userQueryValidator = userQueryValidator;
        }

        public bool IsMessageTextValid(string message)
        {
            return !string.IsNullOrEmpty(message);
        }

        public void ThrowIfMessageCannotBePublished(IPublishMessageParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters), "Publish message parameters cannot be null.");
            }
            if (!IsMessageTextValid(parameters.Text))
            {
                throw new ArgumentException("Message text is not valid.");
            }
            if (!_userQueryValidator.IsUserIdValid(parameters.RecipientId))
            {
                throw new ArgumentException("Recipient User ID is not valid");
            }
        }

        public void ThrowIfMessageCannotBeDestroyed(IEventDTO messageEvent)
        {
            if (messageEvent == null)
            {
                throw new ArgumentNullException("Message parameters cannot be null.");
            }
            if(messageEvent.Type != EventType.MessageCreate)
            {
                throw new ArgumentException("Event must represent a message", nameof(messageEvent));
            }

            if (messageEvent.MessageCreate.IsDestroyed)
            {
                throw new ArgumentException("Message already destroyed.");
            }
        }

        public void ThrowIfMessageCannotBeDestroyed(long messageId)
        {
            if (messageId == TweetinviSettings.DEFAULT_ID)
            {
                throw new ArgumentException("Message Id must be set.");
            }
        }
    }
}