using Tweetinvi.Exceptions;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Client.Validators
{
    public interface IAccountClientParametersValidator
    {
        void Validate(IGetAuthenticatedUserParameters parameters);
        void Validate(IBlockUserParameters parameters);
        void Validate(IUnblockUserParameters parameters);
        void Validate(IReportUserForSpamParameters parameters);
        void Validate(IGetBlockedUserIdsParameters parameters);
        void Validate(IGetBlockedUsersParameters parameters);
        void Validate(IFollowUserParameters parameters);
        void Validate(IUnFollowUserParameters parameters);
        void Validate(IGetUserIdsRequestingFriendshipParameters parameters);
        void Validate(IGetUsersRequestingFriendshipParameters parameters);
        void Validate(IGetUserIdsYouRequestedToFollowParameters parameters);
        void Validate(IGetUsersYouRequestedToFollowParameters parameters);
        void Validate(IUpdateRelationshipParameters parameters);
        void Validate(IGetRelationshipsWithParameters parameters);
        
        //  MUTE
        void Validate(IGetUserIdsWhoseRetweetsAreMutedParameters parameters);
        void Validate(IGetMutedUserIdsParameters parameters);
        void Validate(IGetMutedUsersParameters parameters);
        void Validate(IMuteUserParameters parameters);
        void Validate(IUnMuteUserParameters parameters);
    }
    
    public interface IInternalAccountClientParametersValidator : IAccountClientParametersValidator
    {
        void Initialize(ITwitterClient client);
    }
    
    public class AccountClientParametersValidator : IInternalAccountClientParametersValidator
    {
        private readonly IAccountClientRequiredParametersValidator _accountClientRequiredParametersValidator;
        private ITwitterClient _client;

        public AccountClientParametersValidator(IAccountClientRequiredParametersValidator accountClientRequiredParametersValidator)
        {
            _accountClientRequiredParametersValidator = accountClientRequiredParametersValidator;
        }
        
        public void Initialize(ITwitterClient client)
        {
            _client = client;
        }
        
        private TwitterLimits Limits => _client.ClientSettings.Limits;
        
        public void Validate(IGetAuthenticatedUserParameters parameters)
        {
            _accountClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IBlockUserParameters parameters)
        {
            _accountClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IUnblockUserParameters parameters)
        {
            _accountClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IReportUserForSpamParameters parameters)
        {
            _accountClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IGetBlockedUserIdsParameters parameters)
        {
            _accountClientRequiredParametersValidator.Validate(parameters);
            
            var maxPageSize = Limits.ACCOUNT_GET_BLOCKED_USER_IDS_MAX_PAGE_SIZE;
            if (parameters.PageSize > maxPageSize)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.PageSize)}", maxPageSize, nameof(Limits.ACCOUNT_GET_BLOCKED_USER_IDS_MAX_PAGE_SIZE), "page size");
            }
        }

        public void Validate(IGetBlockedUsersParameters parameters)
        {
            _accountClientRequiredParametersValidator.Validate(parameters);
            
            var maxPageSize = Limits.ACCOUNT_GET_BLOCKED_USER_MAX_PAGE_SIZE;
            if (parameters.PageSize > maxPageSize)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.PageSize)}", maxPageSize, nameof(Limits.ACCOUNT_GET_BLOCKED_USER_MAX_PAGE_SIZE), "page size");
            }
        }

        public void Validate(IFollowUserParameters parameters)
        {
            _accountClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IUnFollowUserParameters parameters)
        {
            _accountClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IGetUserIdsRequestingFriendshipParameters parameters)
        {
            _accountClientRequiredParametersValidator.Validate(parameters);
            
            var maxPageSize = Limits.ACCOUNT_GET_USER_IDS_REQUESTING_FRIENDSHIP_MAX_PAGE_SIZE;
            if (parameters.PageSize > maxPageSize)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.PageSize)}", maxPageSize, nameof(Limits.ACCOUNT_GET_USER_IDS_REQUESTING_FRIENDSHIP_MAX_PAGE_SIZE), "page size");
            }
        }

        public void Validate(IGetUsersRequestingFriendshipParameters parameters)
        {
            Validate(parameters as IGetUserIdsRequestingFriendshipParameters);
            
            var maxSize = Limits.USERS_GET_USERS_MAX_SIZE;
            if (parameters.GetUsersPageSize > maxSize)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.GetUsersPageSize)}", maxSize, nameof(Limits.USERS_GET_USERS_MAX_SIZE), "users");
            }
        }

        public void Validate(IGetUserIdsYouRequestedToFollowParameters parameters)
        {
            _accountClientRequiredParametersValidator.Validate(parameters);
            
            var maxPageSize = Limits.ACCOUNT_GET_REQUESTED_USER_IDS_TO_FOLLOW_MAX_PAGE_SIZE;
            if (parameters.PageSize > maxPageSize)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.PageSize)}", maxPageSize, nameof(Limits.ACCOUNT_GET_REQUESTED_USER_IDS_TO_FOLLOW_MAX_PAGE_SIZE), "page size");
            }
        }

        public void Validate(IGetUsersYouRequestedToFollowParameters parameters)
        {
            Validate(parameters as IGetUserIdsYouRequestedToFollowParameters);
            
            var maxSize = Limits.USERS_GET_USERS_MAX_SIZE;
            if (parameters.GetUsersPageSize > maxSize)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.GetUsersPageSize)}", maxSize, nameof(Limits.USERS_GET_USERS_MAX_SIZE), "users");
            }
        }

        public void Validate(IUpdateRelationshipParameters parameters)
        {
            _accountClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IGetRelationshipsWithParameters parameters)
        {
            _accountClientRequiredParametersValidator.Validate(parameters);
            
            var maxUsers = Limits.ACCOUNT_GET_RELATIONSHIPS_WITH_MAX_SIZE;
            if (parameters.Users.Length > maxUsers)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.Users)}", maxUsers, nameof(Limits.ACCOUNT_GET_RELATIONSHIPS_WITH_MAX_SIZE), "users");
            }
        }

        public void Validate(IGetUserIdsWhoseRetweetsAreMutedParameters parameters)
        {
            _accountClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IGetMutedUserIdsParameters parameters)
        {
            _accountClientRequiredParametersValidator.Validate(parameters);
            
            var maxPageSize = Limits.ACCOUNT_GET_MUTED_USER_IDS_MAX_PAGE_SIZE;
            if (parameters.PageSize > maxPageSize)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.PageSize)}", maxPageSize, nameof(Limits.ACCOUNT_GET_MUTED_USER_IDS_MAX_PAGE_SIZE), "users");
            }
        }

        public void Validate(IGetMutedUsersParameters parameters)
        {
            _accountClientRequiredParametersValidator.Validate(parameters);
            
            var maxPageSize = Limits.ACCOUNT_GET_MUTED_USERS_MAX_PAGE_SIZE;
            if (parameters.PageSize > maxPageSize)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.PageSize)}", maxPageSize, nameof(Limits.ACCOUNT_GET_MUTED_USERS_MAX_PAGE_SIZE), "users");
            }
        }

        public void Validate(IMuteUserParameters parameters)
        {
            _accountClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IUnMuteUserParameters parameters)
        {
            _accountClientRequiredParametersValidator.Validate(parameters);
        }
    }
}