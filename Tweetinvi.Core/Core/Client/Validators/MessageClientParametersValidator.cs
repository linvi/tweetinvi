using Tweetinvi.Exceptions;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Client.Validators
{
    public interface IMessagesClientParametersValidator
    {
        void Validate(IPublishMessageParameters parameters);
        void Validate(IDeleteMessageParameters parameters);
        void Validate(IGetMessageParameters parameters);
        void Validate(IGetMessagesParameters parameters);
    }

    public class MessagesClientParametersValidator : IMessagesClientParametersValidator
    {
        private readonly ITwitterClient _client;
        private readonly IMessagesClientRequiredParametersValidator _messagesClientRequiredParametersValidator;

        public MessagesClientParametersValidator(
            ITwitterClient client,
            IMessagesClientRequiredParametersValidator messagesClientRequiredParametersValidator)
        {
            _client = client;
            _messagesClientRequiredParametersValidator = messagesClientRequiredParametersValidator;
        }

        private TwitterLimits Limits => _client.ClientSettings.Limits;

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

        public void Validate(IGetMessagesParameters parameters)
        {
            _messagesClientRequiredParametersValidator.Validate(parameters);

            var maxPageSize = Limits.MESSAGES_GET_MAX_PAGE_SIZE;
            if (parameters.PageSize > maxPageSize)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.PageSize)}", maxPageSize, nameof(Limits.MESSAGES_GET_MAX_PAGE_SIZE), "page size");
            }
        }
    }
}