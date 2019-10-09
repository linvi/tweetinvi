using Tweetinvi.Exceptions;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Client.Validators
{
    public interface IUsersClientParametersValidator
    {
        void Validate(IGetUserParameters parameters);
        void Validate(IGetUsersParameters parameters);
        void Validate(IGetFollowerIdsParameters parameters);
        void Validate(IGetFollowersParameters parameters);
        void Validate(IGetFriendIdsParameters parameters);
        void Validate(IGetFriendsParameters parameters);
        void Validate(IGetRelationshipBetweenParameters parameters);
        void Validate(IGetProfileImageParameters parameters);
    }
    
    public interface IInternalUsersClientParametersValidator : IUsersClientParametersValidator
    {
        void Initialize(ITwitterClient client);
    }
    
    public class UsersClientParametersValidator : IInternalUsersClientParametersValidator
    {
        private readonly IUsersClientRequiredParametersValidator _usersClientRequiredParametersValidator;
        private ITwitterClient _client;

        public UsersClientParametersValidator(IUsersClientRequiredParametersValidator usersClientRequiredParametersValidator)
        {
            _usersClientRequiredParametersValidator = usersClientRequiredParametersValidator;
        }

        private TwitterLimits Limits => _client.Config.Limits;

        public void Initialize(ITwitterClient client)
        {
            _client = client;
        }

        public void Validate(IGetUserParameters parameters)
        {
            _usersClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IGetUsersParameters parameters)
        {
            _usersClientRequiredParametersValidator.Validate(parameters);
            
            var maxSize = Limits.USERS_GET_USERS_MAX_SIZE;
            if (parameters.Users.Length > maxSize)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.Users)}", maxSize, nameof(Limits.USERS_GET_USERS_MAX_SIZE), "users");
            }
        }

        public void Validate(IGetFollowerIdsParameters parameters)
        {
            _usersClientRequiredParametersValidator.Validate(parameters);
            
            var maxPageSize = Limits.USERS_GET_FOLLOWER_IDS_PAGE_MAX_SIZE;
            if (parameters.PageSize > maxPageSize)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.PageSize)}", maxPageSize, nameof(Limits.USERS_GET_FOLLOWER_IDS_PAGE_MAX_SIZE), "page size");
            }
        }

        public void Validate(IGetFollowersParameters parameters)
        {
            _usersClientRequiredParametersValidator.Validate(parameters);
            
            Validate(parameters as IGetFollowerIdsParameters);
            
            var maxUserPerPage = Limits.USERS_GET_USERS_MAX_SIZE;
            if (parameters.GetUsersPageSize > maxUserPerPage)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.GetUsersPageSize)}", maxUserPerPage, nameof(Limits.USERS_GET_USERS_MAX_SIZE), "user ids");
            }
        }

        public void Validate(IGetFriendIdsParameters parameters)
        {
            _usersClientRequiredParametersValidator.Validate(parameters);
            
            var maxPageSize = Limits.USERS_GET_FRIEND_IDS_PAGE_MAX_SIZE;
            if (parameters.PageSize > maxPageSize)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.PageSize)}", maxPageSize, nameof(Limits.USERS_GET_FRIEND_IDS_PAGE_MAX_SIZE), "page size");
            }
        }

        public void Validate(IGetFriendsParameters parameters)
        {
            _usersClientRequiredParametersValidator.Validate(parameters);
            
            Validate(parameters as IGetFriendIdsParameters);

            var maxUserPerPage = Limits.USERS_GET_USERS_MAX_SIZE;
            if (parameters.GetUsersPageSize > maxUserPerPage)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.GetUsersPageSize)}", maxUserPerPage, nameof(Limits.USERS_GET_USERS_MAX_SIZE), "user ids");
            }
        }

        public void Validate(IGetRelationshipBetweenParameters parameters)
        {
            _usersClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IGetProfileImageParameters parameters)
        {
            _usersClientRequiredParametersValidator.Validate(parameters);
        }
    }
}