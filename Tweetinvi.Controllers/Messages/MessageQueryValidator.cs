using System;
using Tweetinvi.Core;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.QueryValidators;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Controllers.Messages
{
    public interface IMessageQueryValidator
    {
        bool IsMessageTextValid(string message);
        bool IsMessageIdValid(long messageId);

        void ThrowIfMessageCannotBePublished(IPublishMessageParameters parameters);
        bool CanMessageDTOBeDestroyed(IMessageDTO messageDTO);
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

        public bool IsMessageIdValid(long messageId)
        {
            return messageId != TweetinviSettings.DEFAULT_ID;
        }

        public void ThrowIfMessageCannotBePublished(IPublishMessageParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("Publish message parameters cannot be null.");
            }

            var message = parameters.Message;
            var text = parameters.Text;

            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.Recipient);

            if (!IsMessageTextValid(text))
            {
                throw new ArgumentException("Message text is not valid.");
            }

            if (message != null)
            {
                if (message.IsMessagePublished)
                {
                    throw new ArgumentException("Message has already been published.");
                }

                if (message.IsMessageDestroyed)
                {
                    throw new ArgumentException("Message has already been destroyed.");
                }
            }
        }

        public bool CanMessageDTOBeDestroyed(IMessageDTO messageDTO)
        {
            bool isMessageInValidStateToBeDestroyed = messageDTO != null &&
                                                      messageDTO.Id != TweetinviSettings.DEFAULT_ID &&
                                                      messageDTO.IsMessagePublished &&
                                                      !messageDTO.IsMessageDestroyed;

            return isMessageInValidStateToBeDestroyed;
        }
    }
}