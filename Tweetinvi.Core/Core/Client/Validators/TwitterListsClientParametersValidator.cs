using System;
using Tweetinvi.Exceptions;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Client.Validators
{
    public interface ITwitterListsClientParametersValidator
    {
        void Validate(ICreateListParameters parameters);
        void Validate(IGetListParameters parameters);
        void Validate(IGetListsSubscribedByUserParameters parameters);
        void Validate(IUpdateListParameters parameters);
        void Validate(IDestroyListParameters parameters);
        void Validate(IGetListsOwnedByUserParameters parameters);

        // MEMBERS
        void Validate(IAddMemberToListParameters parameters);
        void Validate(IAddMembersToListParameters parameters);
        void Validate(IGetListsAUserIsMemberOfParameters parameters);
        void Validate(IGetMembersOfListParameters parameters);
        void Validate(ICheckIfUserIsMemberOfListParameters parameters);
        void Validate(IRemoveMemberFromListParameters parameters);
        void Validate(IRemoveMembersFromListParameters parameters);
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

        public void Validate(IGetListsSubscribedByUserParameters parameters)
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

        public void Validate(IGetListsOwnedByUserParameters parameters)
        {
            _twitterListsClientRequiredParametersValidator.Validate(parameters);

            var maxPageSize = Limits.LISTS_GET_USER_OWNED_LISTS_MAX_SIZE;
            if (parameters.PageSize > maxPageSize)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.PageSize)}", maxPageSize, nameof(Limits.LISTS_GET_USER_OWNED_LISTS_MAX_SIZE), "page size");
            }
        }

        public void Validate(IAddMemberToListParameters parameters)
        {
            _twitterListsClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IAddMembersToListParameters parameters)
        {
            _twitterListsClientRequiredParametersValidator.Validate(parameters);

            if (parameters.Users.Count == 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(parameters)}.{nameof(parameters.Users)}", "You must have at least 1 user");
            }

            var maxUsers = TwitterLimits.DEFAULTS.LISTS_ADD_MEMBERS_MAX_USERS;
            if (parameters.Users.Count > maxUsers)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.Users)}", maxUsers, nameof(Limits.LISTS_ADD_MEMBERS_MAX_USERS), "users");
            }
        }

        public void Validate(IGetListsAUserIsMemberOfParameters parameters)
        {
            _twitterListsClientRequiredParametersValidator.Validate(parameters);

            var maxPageSize = Limits.LISTS_GET_USER_MEMBERSHIPS_LISTS_MAX_SIZE;
            if (parameters.PageSize > maxPageSize)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.PageSize)}", maxPageSize, nameof(Limits.LISTS_GET_USER_MEMBERSHIPS_LISTS_MAX_SIZE), "page size");
            }
        }

        public void Validate(IGetMembersOfListParameters parameters)
        {
            _twitterListsClientRequiredParametersValidator.Validate(parameters);

            var maxPageSize = Limits.LISTS_GET_MEMBERS_MAX_SIZE;
            if (parameters.PageSize > maxPageSize)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.PageSize)}", maxPageSize, nameof(Limits.LISTS_GET_MEMBERS_MAX_SIZE), "page size");
            }
        }

        public void Validate(ICheckIfUserIsMemberOfListParameters parameters)
        {
            _twitterListsClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IRemoveMemberFromListParameters parameters)
        {
            _twitterListsClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IRemoveMembersFromListParameters parameters)
        {
            _twitterListsClientRequiredParametersValidator.Validate(parameters);

            if (parameters.Users.Count == 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(parameters)}.{nameof(parameters.Users)}", "You must have at least 1 user");
            }

            var maxUsers = TwitterLimits.DEFAULTS.LISTS_REMOVE_MEMBERS_MAX_USERS;
            if (parameters.Users.Count > maxUsers)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.Users)}", maxUsers, nameof(Limits.LISTS_REMOVE_MEMBERS_MAX_USERS), "users");
            }
        }
    }
}