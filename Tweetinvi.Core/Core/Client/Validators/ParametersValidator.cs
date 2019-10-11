using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Client.Validators
{
    public interface IParametersValidator : 
        IAccountClientParametersValidator,
        IAccountSettingsClientParametersValidator,
        ITimelineClientParametersValidator,
        ITweetsClientParametersValidator,
        IUploadClientParametersValidator,
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
        private readonly IInternalTimelineClientParametersValidator _timelineClientParametersValidator;
        private readonly IInternalTweetsClientParametersValidator _tweetsClientParametersValidator;
        private readonly IInternalUploadClientParametersValidator _uploadClientParametersValidator;
        private readonly IInternalUsersClientParametersValidator _usersClientParametersValidator;

        public ParametersValidator(
            IInternalAccountClientParametersValidator accountClientParametersValidator,
            IInternalAccountSettingsClientParametersValidator accountSettingsClientParametersValidator,
            IInternalTimelineClientParametersValidator timelineClientParametersValidator,
            IInternalTweetsClientParametersValidator tweetsClientParametersValidator,
            IInternalUploadClientParametersValidator uploadClientParametersValidator,
            IInternalUsersClientParametersValidator usersClientParametersValidator)
        {
            _accountClientParametersValidator = accountClientParametersValidator;
            _accountSettingsClientParametersValidator = accountSettingsClientParametersValidator;
            _timelineClientParametersValidator = timelineClientParametersValidator;
            _tweetsClientParametersValidator = tweetsClientParametersValidator;
            _uploadClientParametersValidator = uploadClientParametersValidator;
            _usersClientParametersValidator = usersClientParametersValidator;
        }
        
        public void Initialize(ITwitterClient client)
        {
            _accountClientParametersValidator.Initialize(client);
            _accountSettingsClientParametersValidator.Initialize(client);
            _tweetsClientParametersValidator.Initialize(client);
            _uploadClientParametersValidator.Initialize(client);
            _usersClientParametersValidator.Initialize(client);
        }
        
        public void Validate(IGetAuthenticatedUserParameters parameters)
        {
            _accountClientParametersValidator.Validate(parameters);
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

        public void Validate(IGetMutedUserIdsParameters parameters)
        {
            _accountClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetMutedUsersParameters parameters)
        {
            _accountClientParametersValidator.Validate(parameters);
        }

        public void Validate(IMuteUserParameters parameters)
        {
            _accountClientParametersValidator.Validate(parameters);
        }

        public void Validate(IUnMuteUserParameters parameters)
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
        
        public void Validate(IGetRetweetsOfMeTimelineParameters parameters)
        {
            _timelineClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetTweetParameters parameters)
        {
            _tweetsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetTweetsParameters parameters)
        {
            _tweetsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IPublishTweetParameters parameters)
        {
            _tweetsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IDestroyTweetParameters parameters)
        {
            _tweetsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetFavoriteTweetsParameters parameters)
        {
            _tweetsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetRetweetsParameters parameters)
        {
            _tweetsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IPublishRetweetParameters parameters)
        {
            _tweetsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IDestroyRetweetParameters parameters)
        {
            _tweetsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IUploadParameters parameters)
        {
            _uploadClientParametersValidator.Validate(parameters);
        }

        public void Validate(IAddMediaMetadataParameters parameters)
        {
            _uploadClientParametersValidator.Validate(parameters);
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
    }
}