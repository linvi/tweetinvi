using System;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Exceptions;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Client.Validators
{
    public interface ITwitterListsClientRequiredParametersValidator : ITwitterListsClientParametersValidator
    {
    }

    public class TwitterListsClientRequiredParametersValidator : ITwitterListsClientRequiredParametersValidator
    {
        private readonly ITwitterListQueryValidator _twitterListQueryValidator;
        private readonly IUserQueryValidator _userQueryValidator;

        public TwitterListsClientRequiredParametersValidator(
            ITwitterListQueryValidator twitterListQueryValidator,
            IUserQueryValidator userQueryValidator)
        {
            _twitterListQueryValidator = twitterListQueryValidator;
            _userQueryValidator = userQueryValidator;
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

            _twitterListQueryValidator.ThrowIfListIdentifierIsNotValid(parameters.List);
        }

        public void Validate(IGetListsSubscribedByUserParameters parameters)
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

            _twitterListQueryValidator.ThrowIfListIdentifierIsNotValid(parameters.List);
        }

        public void Validate(IDestroyListParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            _twitterListQueryValidator.ThrowIfListIdentifierIsNotValid(parameters.List);
        }

        public void Validate(IGetListsOwnedByUserParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.User);
        }

        public void Validate(IAddMemberToListParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            _twitterListQueryValidator.ThrowIfListIdentifierIsNotValid(parameters.List);
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.User);
        }

        public void Validate(IAddMembersToListParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            _twitterListQueryValidator.ThrowIfListIdentifierIsNotValid(parameters.List);
        }

        public void Validate(IGetListsAUserIsMemberOfParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.User);
        }

        public void Validate(IGetMembersOfListParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            _twitterListQueryValidator.ThrowIfListIdentifierIsNotValid(parameters.List);
        }

        public void Validate(ICheckIfUserIsMemberOfListParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            _twitterListQueryValidator.ThrowIfListIdentifierIsNotValid(parameters.List);
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.User);
        }

        public void Validate(IRemoveMemberFromListParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            _twitterListQueryValidator.ThrowIfListIdentifierIsNotValid(parameters.List);
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.User);
        }

        public void Validate(IRemoveMembersFromListParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            _twitterListQueryValidator.ThrowIfListIdentifierIsNotValid(parameters.List);
        }
    }
}