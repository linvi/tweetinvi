using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Tweetinvi.Parameters.TrendsClient;

namespace Tweetinvi.Core.Client.Validators
{
    public interface IParametersValidator :
        IAccountActivityClientParametersValidator,
        IAccountSettingsClientParametersValidator,
        IAuthClientParametersValidator,
        IHelpClientParametersValidator,
        IMessagesClientParametersValidator,
        ISearchClientParametersValidator,
        ITwitterListsClientParametersValidator,
        ITimelineClientParametersValidator,
        ITrendsClientParametersValidator,
        ITweetsClientParametersValidator,
        IUploadClientParametersValidator,
        IUsersClientParametersValidator
    {
    }

    public class ParametersValidator : IParametersValidator
    {
        private readonly IAccountActivityClientParametersValidator _accountActivityClientParametersValidator;
        private readonly IAccountSettingsClientParametersValidator _accountSettingsClientParametersValidator;
        private readonly IAuthClientParametersValidator _authClientParametersValidator;
        private readonly IHelpClientParametersValidator _helpClientParametersValidator;
        private readonly IMessagesClientParametersValidator _messagesClientParametersValidator;
        private readonly ISearchClientParametersValidator _searchClientParametersValidator;
        private readonly ITwitterListsClientParametersValidator _twitterListsClientParametersValidator;
        private readonly ITrendsClientParametersValidator _trendsClientParametersValidator;
        private readonly ITimelineClientParametersValidator _timelineClientParametersValidator;
        private readonly ITweetsClientParametersValidator _tweetsClientParametersValidator;
        private readonly IUploadClientParametersValidator _uploadClientParametersValidator;
        private readonly IUsersClientParametersValidator _usersClientParametersValidator;

        public ParametersValidator(
            IAccountActivityClientParametersValidator accountActivityClientParametersValidator,
            IAccountSettingsClientParametersValidator accountSettingsClientParametersValidator,
            IAuthClientParametersValidator authClientParametersValidator,
            IHelpClientParametersValidator helpClientParametersValidator,
            IMessagesClientParametersValidator messagesClientParametersValidator,
            ISearchClientParametersValidator searchClientParametersValidator,
            ITwitterListsClientParametersValidator twitterListsClientParametersValidator,
            ITrendsClientParametersValidator trendsClientParametersValidator,
            ITimelineClientParametersValidator timelineClientParametersValidator,
            ITweetsClientParametersValidator tweetsClientParametersValidator,
            IUploadClientParametersValidator uploadClientParametersValidator,
            IUsersClientParametersValidator usersClientParametersValidator)
        {
            _accountActivityClientParametersValidator = accountActivityClientParametersValidator;
            _accountSettingsClientParametersValidator = accountSettingsClientParametersValidator;
            _authClientParametersValidator = authClientParametersValidator;
            _helpClientParametersValidator = helpClientParametersValidator;
            _messagesClientParametersValidator = messagesClientParametersValidator;
            _searchClientParametersValidator = searchClientParametersValidator;
            _twitterListsClientParametersValidator = twitterListsClientParametersValidator;
            _trendsClientParametersValidator = trendsClientParametersValidator;
            _timelineClientParametersValidator = timelineClientParametersValidator;
            _tweetsClientParametersValidator = tweetsClientParametersValidator;
            _uploadClientParametersValidator = uploadClientParametersValidator;
            _usersClientParametersValidator = usersClientParametersValidator;
        }

        public void Validate(ICreateAccountActivityWebhookParameters parameters)
        {
            _accountActivityClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetAccountActivityWebhookEnvironmentsParameters parameters)
        {
            _accountActivityClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetAccountActivityEnvironmentWebhooksParameters parameters)
        {
            _accountActivityClientParametersValidator.Validate(parameters);
        }

        public void Validate(IDeleteAccountActivityWebhookParameters parameters)
        {
            _accountActivityClientParametersValidator.Validate(parameters);
        }

        public void Validate(ITriggerAccountActivityWebhookCRCParameters parameters)
        {
            _accountActivityClientParametersValidator.Validate(parameters);
        }

        public void Validate(ISubscribeToAccountActivityParameters parameters)
        {
            _accountActivityClientParametersValidator.Validate(parameters);
        }

        public void Validate(ICountAccountActivitySubscriptionsParameters parameters)
        {
            _accountActivityClientParametersValidator.Validate(parameters);
        }

        public void Validate(IIsAccountSubscribedToAccountActivityParameters parameters)
        {
            _accountActivityClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetAccountActivitySubscriptionsParameters parameters)
        {
            _accountActivityClientParametersValidator.Validate(parameters);
        }

        public void Validate(IUnsubscribeFromAccountActivityParameters parameters)
        {
            _accountActivityClientParametersValidator.Validate(parameters);
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

        public void Validate(IGetPlaceParameters parameters)
        {
            _helpClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGeoSearchParameters parameters)
        {
            _helpClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGeoSearchReverseParameters parameters)
        {
            _helpClientParametersValidator.Validate(parameters);
        }

        public void Validate(IPublishMessageParameters parameters)
        {
            _messagesClientParametersValidator.Validate(parameters);
        }

        public void Validate(IDeleteMessageParameters parameters)
        {
            _messagesClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetMessageParameters parameters)
        {
            _messagesClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetMessagesParameters parameters)
        {
            _messagesClientParametersValidator.Validate(parameters);
        }

        public void Validate(ICreateListParameters parameters)
        {
            _twitterListsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetListParameters parameters)
        {
            _twitterListsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetListsSubscribedByUserParameters parameters)
        {
            _twitterListsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IUpdateListParameters parameters)
        {
            _twitterListsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IDestroyListParameters parameters)
        {
            _twitterListsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetListsOwnedByUserParameters parameters)
        {
            _twitterListsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetTweetsFromListParameters parameters)
        {
            _twitterListsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IAddMemberToListParameters parameters)
        {
            _twitterListsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IAddMembersToListParameters parameters)
        {
            _twitterListsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetUserListMembershipsParameters parameters)
        {
            _twitterListsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetMembersOfListParameters parameters)
        {
            _twitterListsClientParametersValidator.Validate(parameters);
        }

        public void Validate(ICheckIfUserIsMemberOfListParameters parameters)
        {
            _twitterListsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IRemoveMemberFromListParameters parameters)
        {
            _twitterListsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IRemoveMembersFromListParameters parameters)
        {
            _twitterListsClientParametersValidator.Validate(parameters);
        }

        public void Validate(ISubscribeToListParameters parameters)
        {
            _twitterListsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IUnsubscribeFromListParameters parameters)
        {
            _twitterListsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetListSubscribersParameters parameters)
        {
            _twitterListsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetUserListSubscriptionsParameters parameters)
        {
            _twitterListsClientParametersValidator.Validate(parameters);
        }

        public void Validate(ICheckIfUserIsSubscriberOfListParameters parameters)
        {
            _twitterListsClientParametersValidator.Validate(parameters);
        }

        public void Validate(ISearchTweetsParameters parameters)
        {
            _searchClientParametersValidator.Validate(parameters);
        }

        public void Validate(ISearchUsersParameters parameters)
        {
            _searchClientParametersValidator.Validate(parameters);
        }

        public void Validate(ICreateSavedSearchParameters parameters)
        {
            _searchClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetSavedSearchParameters parameters)
        {
            _searchClientParametersValidator.Validate(parameters);
        }

        public void Validate(IListSavedSearchesParameters parameters)
        {
            _searchClientParametersValidator.Validate(parameters);
        }

        public void Validate(IDestroySavedSearchParameters parameters)
        {
            _searchClientParametersValidator.Validate(parameters);
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

        public void Validate(IGetTrendsLocationCloseToParameters parameters)
        {
            _trendsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetTrendsAtParameters parameters)
        {
            _trendsClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetTrendsLocationParameters parameters)
        {
            _trendsClientParametersValidator.Validate(parameters);
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

        public void Validate(IGetUserFavoriteTweetsParameters parameters)
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

        public void Validate(IUnfavoriteTweetParameters parameters)
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

        public void Validate(IGetAuthenticatedUserParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IBlockUserParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IUnblockUserParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IReportUserForSpamParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetBlockedUserIdsParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetBlockedUsersParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IFollowUserParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IUnfollowUserParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetUserIdsRequestingFriendshipParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetUsersRequestingFriendshipParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetUserIdsYouRequestedToFollowParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetUsersYouRequestedToFollowParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IUpdateRelationshipParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetRelationshipsWithParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetUserIdsWhoseRetweetsAreMutedParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetMutedUserIdsParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetMutedUsersParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IMuteUserParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IUnmuteUserParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }
    }
}