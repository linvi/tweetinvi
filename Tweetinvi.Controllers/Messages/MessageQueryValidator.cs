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

        bool CanMessageBePublished(IPublishMessageParameters parameters);
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

        public bool CanMessageBePublished(IPublishMessageParameters parameters)
        {
            var message = parameters.Message;
            var text = parameters.Text;

            bool messageTextIsValid = IsMessageTextValid(text);
            bool isRecipientValid = _userQueryValidator.CanUserBeIdentified(parameters.Recipient) ||
                                    _userQueryValidator.IsUserIdValid(parameters.RecipientId) ||
                                    _userQueryValidator.IsScreenNameValid(parameters.RecipientScreenName);

            bool isMessageInValidState = message == null || (!message.IsMessagePublished && !message.IsMessageDestroyed);

            if (!isMessageInValidState)
            {
                return false;
            }

            return isRecipientValid && messageTextIsValid;
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