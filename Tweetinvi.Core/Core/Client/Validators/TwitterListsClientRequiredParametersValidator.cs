using System;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Parameters.ListsClient;

namespace Tweetinvi.Core.Client.Validators
{
    public interface ITwitterListsClientRequiredParametersValidator : ITwitterListsClientParametersValidator
    {
    }

    public class TwitterListsClientRequiredParametersValidator : ITwitterListsClientRequiredParametersValidator
    {
        private readonly ITwitterListQueryValidator _twitterListQueryValidator;

        public TwitterListsClientRequiredParametersValidator(ITwitterListQueryValidator twitterListQueryValidator)
        {
            _twitterListQueryValidator = twitterListQueryValidator;
        }

        public void Validate(ICreateListParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (string.IsNullOrEmpty(parameters.Name))
            {
                throw new ArgumentNullException($"{nameof(parameters)}.{nameof(parameters.Name)}");
            }
        }

        public void Validate(IGetListParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            _twitterListQueryValidator.ThrowIfListIdentifierIsNotValid(parameters.Id);
        }

        public void Validate(IGetUserListsParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
        }

        public void Validate(IUpdateListParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            _twitterListQueryValidator.ThrowIfListIdentifierIsNotValid(parameters.Id);
        }

        public void Validate(IDestroyListParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            _twitterListQueryValidator.ThrowIfListIdentifierIsNotValid(parameters.Id);
        }


    }
}