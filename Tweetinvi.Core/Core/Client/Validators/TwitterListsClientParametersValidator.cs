using Tweetinvi.Exceptions;
using Tweetinvi.Parameters.ListsClient;

namespace Tweetinvi.Core.Client.Validators
{
    public interface ITwitterListsClientParametersValidator
    {
        void Validate(ICreateListParameters parameters);
        void Validate(IGetListParameters parameters);
        void Validate(IGetUserListsParameters parameters);
        void Validate(IUpdateListParameters parameters);
        void Validate(IDestroyListParameters parameters);
    }

    public class TwitterListsClientParametersValidator : ITwitterListsClientParametersValidator
    {
        private readonly ITwitterClient _client;
        private readonly ITwitterListsClientRequiredParametersValidator _twitterListsClientRequiredParametersValidator;

        public TwitterListsClientParametersValidator(
            ITwitterClient client,
            ITwitterListsClientRequiredParametersValidator twitterListsClientRequiredParametersValidator)
        {
            _client = client;
            _twitterListsClientRequiredParametersValidator = twitterListsClientRequiredParametersValidator;
        }

        private TwitterLimits Limits => _client.ClientSettings.Limits;

        public void Validate(ICreateListParameters parameters)
        {
            _twitterListsClientRequiredParametersValidator.Validate(parameters);

            var maxNameSize = Limits.LISTS_CREATE_NAME_MAX_SIZE;
            if (parameters.Name.Length > maxNameSize)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.Name)}", maxNameSize, nameof(Limits.LISTS_CREATE_NAME_MAX_SIZE), "characters");
            }
        }

        public void Validate(IGetListParameters parameters)
        {
            _twitterListsClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IGetUserListsParameters parameters)
        {
            _twitterListsClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IUpdateListParameters parameters)
        {
            _twitterListsClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IDestroyListParameters parameters)
        {
            _twitterListsClientRequiredParametersValidator.Validate(parameters);
        }
    }
}