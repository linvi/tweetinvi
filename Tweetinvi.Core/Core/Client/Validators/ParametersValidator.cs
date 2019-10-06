using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Client.Validators
{
    public interface IParametersValidator : 
        IAccountClientParametersValidator,
        IAccountSettingsClientParametersValidator,
        ITweetsClientParametersValidator,
        IUsersClientParametersValidator
    {
    }
    
    public interface IInternalParametersValidator : IParametersValidator
    {
        void Initialize(ITwitterClient client);
    }
    
    public class ParametersValidator : IInternalParametersValidator
    {
        private readonly IInternalAccountClientParametersValidator _accountClientParametersValidator;
        private readonly IInternalAccountSettingsClientParametersValidator _accountSettingsClientParametersValidator;
        private readonly IInternalTweetsClientParametersValidator _tweetsClientParametersValidator;
        private readonly IInternalUsersClientParametersValidator _usersClientParametersValidator;

        public ParametersValidator(
            IInternalAccountClientParametersValidator accountClientParametersValidator,
            IInternalAccountSettingsClientParametersValidator accountSettingsClientParametersValidator,
            IInternalTweetsClientParametersValidator tweetsClientParametersValidator,
            IInternalUsersClientParametersValidator usersClientParametersValidator)
        {
            _usersClientParametersValidator = usersClientParametersValidator;
            _accountClientParametersValidator = accountClientParametersValidator;
            _accountSettingsClientParametersValidator = accountSettingsClientParametersValidator;
            _tweetsClientParametersValidator = tweetsClientParametersValidator;
        }
        
        public void Validate(IBlockUserParameters parameters)
        {
            _accountClientParametersValidator.Validate(parameters);
        }

        public void Validate(IUnblockUserParameters parameters)
        {
            _accountClientParametersValidator.Validate(parameters);
        }

        public void Validate(IReportUserForSpamParameters parameters)
        {
            _accountClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetBlockedUserIdsParameters parameters)
        {
            _accountClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetBlockedUsersParameters parameters)
        {
            _accountClientParametersValidator.Validate(parameters);
        }

        public void Validate(IFollowUserParameters parameters)
        {
            _accountClientParametersValidator.Validate(parameters);
        }

        public void Validate(IUnFollowUserParameters parameters)
        {
            _accountClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetUserIdsRequestingFriendshipParameters parameters)
        {
            _accountClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetUsersRequestingFriendshipParameters parameters)
        {
            _accountClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetUserIdsYouRequestedToFollowParameters parameters)
        {
            _accountClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetUsersYouRequestedToFollowParameters parameters)
        {
            _accountClientParametersValidator.Validate(parameters);
        }

        public void Validate(IUpdateRelationshipParameters parameters)
        {
            _accountClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetRelationshipsWithParameters parameters)
        {
            _accountClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetUserIdsWhoseRetweetsAreMutedParameters parameters)
        {
            _accountClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetAccountSettingsParameters parameters)
        {
            _accountSettingsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IUpdateAccountSettingsParameters parameters)
        {
            _accountSettingsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IUpdateProfileParameters parameters)
        {
            _accountSettingsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IUpdateProfileImageParameters parameters)
        {
            _accountSettingsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IUpdateProfileBannerParameters parameters)
        {
            _accountSettingsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IRemoveProfileBannerParameters parameters)
        {
            _accountSettingsClientParametersValidator.Validate(parameters);
        }
        
        public void Validate(IPublishTweetParameters parameters)
        {
            _tweetsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetFavoriteTweetsParameters parameters)
        {
            _tweetsClientParametersValidator.Validate(parameters);
        }
        
        public void Initialize(ITwitterClient client)
        {
            _usersClientParametersValidator.Initialize(client);
        }

        public void Validate(IGetUserParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetUsersParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetFollowerIdsParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetFollowersParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetFriendIdsParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetFriendsParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetRelationshipBetweenParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetProfileImageParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetAuthenticatedUserParameters parameters)
        {
            _accountClientParametersValidator.Validate(parameters);
        }
    }
}