using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Tweetinvi.Parameters.Auth;
using Tweetinvi.Parameters.ListsClient;

namespace Tweetinvi.Core.Client.Validators
{
    public interface IParametersValidator :
        IAccountClientParametersValidator,
        IAccountSettingsClientParametersValidator,
        IAuthClientParametersValidator,
        IHelpClientParametersValidator,
        ITwitterListsClientParametersValidator,
        ITimelineClientParametersValidator,
        ITweetsClientParametersValidator,
        IUploadClientParametersValidator,
        IUsersClientParametersValidator
    {
    }

    public class ParametersValidator : IParametersValidator
    {
        private readonly IAccountClientParametersValidator _accountClientParametersValidator;
        private readonly IAccountSettingsClientParametersValidator _accountSettingsClientParametersValidator;
        private readonly IAuthClientParametersValidator _authClientParametersValidator;
        private readonly IHelpClientParametersValidator _helpClientParametersValidator;
        private readonly ITwitterListsClientParametersValidator _twitterListsClientParametersValidator;
        private readonly ITimelineClientParametersValidator _timelineClientParametersValidator;
        private readonly ITweetsClientParametersValidator _tweetsClientParametersValidator;
        private readonly IUploadClientParametersValidator _uploadClientParametersValidator;
        private readonly IUsersClientParametersValidator _usersClientParametersValidator;

        public ParametersValidator(
            IAccountClientParametersValidator accountClientParametersValidator,
            IAccountSettingsClientParametersValidator accountSettingsClientParametersValidator,
            IAuthClientParametersValidator authClientParametersValidator,
            IHelpClientParametersValidator helpClientParametersValidator,
            ITwitterListsClientParametersValidator twitterListsClientParametersValidator,
            ITimelineClientParametersValidator timelineClientParametersValidator,
            ITweetsClientParametersValidator tweetsClientParametersValidator,
            IUploadClientParametersValidator uploadClientParametersValidator,
            IUsersClientParametersValidator usersClientParametersValidator)
        {
            _accountClientParametersValidator = accountClientParametersValidator;
            _accountSettingsClientParametersValidator = accountSettingsClientParametersValidator;
            _authClientParametersValidator = authClientParametersValidator;
            _helpClientParametersValidator = helpClientParametersValidator;
            _twitterListsClientParametersValidator = twitterListsClientParametersValidator;
            _timelineClientParametersValidator = timelineClientParametersValidator;
            _tweetsClientParametersValidator = tweetsClientParametersValidator;
            _uploadClientParametersValidator = uploadClientParametersValidator;
            _usersClientParametersValidator = usersClientParametersValidator;
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

        public void Validate(ICreateBearerTokenParameters parameters, ITwitterRequest request)
        {
            _authClientParametersValidator.Validate(parameters, request);
        }

        public void Validate(IRequestAuthUrlParameters parameters)
        {
            _authClientParametersValidator.Validate(parameters);
        }

        public void Validate(IRequestCredentialsParameters parameters)
        {
            _authClientParametersValidator.Validate(parameters);
        }

        public void Validate(IInvalidateAccessTokenParameters parameters, ITwitterRequest request)
        {
            _authClientParametersValidator.Validate(parameters, request);
        }

        public void Validate(IInvalidateBearerTokenParameters parameters, ITwitterRequest request)
        {
            _authClientParametersValidator.Validate(parameters, request);
        }

        public void Validate(IGetRateLimitsParameters parameters)
        {
            _helpClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetTwitterConfigurationParameters parameters)
        {
            _helpClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetSupportedLanguagesParameters parameters)
        {
            _helpClientParametersValidator.Validate(parameters);
        }

        public void Validate(ICreateListParameters parameters)
        {
            _twitterListsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IDestroyListParameters parameters)
        {
            _twitterListsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetListParameters parameters)
        {
            _twitterListsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetHomeTimelineParameters parameters)
        {
            _timelineClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetUserTimelineParameters parameters)
        {
            _timelineClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetMentionsTimelineParameters parameters)
        {
            _timelineClientParametersValidator.Validate(parameters);
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

        public void Validate(IGetRetweeterIdsParameters parameters)
        {
            _tweetsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IFavoriteTweetParameters parameters)
        {
            _tweetsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IUnFavoriteTweetParameters parameters)
        {
            _tweetsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetOEmbedTweetParameters parameters)
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