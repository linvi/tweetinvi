using System;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Messages
{
    public interface IMessageQueryValidator
    {
        bool IsMessageTextValid(string message);

        void ThrowIfMessageCannotBePublished(IPublishMessageParameters parameters);
        void ThrowIfMessageCannotBeDestroyed(IMessageDTO message);
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
                throw new ArgumentNullException("Publish message parameters cannot be null.");
            }

            var text = parameters.Text;

            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.Recipient);

            if (!IsMessageTextValid(text))
            {
                throw new ArgumentException("Message text is not valid.");
            }
        }

        public void ThrowIfMessageCannotBeDestroyed(IMessageDTO message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("Message parameters cannot be null.");
            }

            if (!message.IsMessagePublished)
            {
                throw new ArgumentException("Message has not yet been published.");
            }

            if (message.IsMessageDestroyed)
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