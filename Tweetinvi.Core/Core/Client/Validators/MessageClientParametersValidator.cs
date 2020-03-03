using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Client.Validators
{
    public interface IMessagesClientParametersValidator
    {
        void Validate(IPublishMessageParameters parameters);
        void Validate(IDeleteMessageParameters parameters);
        void Validate(IGetMessageParameters parameters);
    }

    public class MessagesClientParametersValidator : IMessagesClientParametersValidator
    {
        private readonly IMessagesClientRequiredParametersValidator _messagesClientRequiredParametersValidator;

        public MessagesClientParametersValidator(IMessagesClientRequiredParametersValidator messagesClientRequiredParametersValidator)
        {
            _messagesClientRequiredParametersValidator = messagesClientRequiredParametersValidator;
        }

        public void Validate(IPublishMessageParameters parameters)
        {
            _messagesClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IDeleteMessageParameters parameters)
        {
            _messagesClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IGetMessageParameters parameters)
        {
            _messagesClientRequiredParametersValidator.Validate(parameters);
        }
    }
}