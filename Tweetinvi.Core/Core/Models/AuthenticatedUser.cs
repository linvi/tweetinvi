using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Logic;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Models
{
    /// <summary>
    /// A token user is unique to a Token and provides action that will
    /// be executed from the connected user and that are not available
    /// from another user like (read my messages)
    /// </summary>
    public class AuthenticatedUser : User, IAuthenticatedUser
    {
        private readonly ICredentialsAccessor _credentialsAccessor;
        private readonly IMessageController _messageController;
        private readonly IFriendshipController _friendshipController;
        private readonly IAccountController _accountController;
        private readonly ITwitterListController _twitterListController;
        private readonly ISavedSearchController _savedSearchController;

        private ITwitterCredentials _savedCredentials;

        public AuthenticatedUser(
            IUserDTO userDTO,
            ICredentialsAccessor credentialsAccessor,
            ITimelineController timelineController,
            IUserController userController,
            IMessageController messageController,
            IFriendshipController friendshipController,
            IAccountController accountController,
            ITwitterListController twitterListController,
            ISavedSearchController savedSearchController)

            : base(userDTO, userController, timelineController, friendshipController, twitterListController)
        {
            _credentialsAccessor = credentialsAccessor;
            _messageController = messageController;
            _friendshipController = friendshipController;
            _accountController = accountController;
            _twitterListController = twitterListController;
            _savedSearchController = savedSearchController;

            Credentials = _credentialsAccessor.CurrentThreadCredentials;
        }

        public string Email { get { return _userDTO.Email; } }

        public void SetCredentials(ITwitterCredentials credentials)
        {
            Credentials = credentials;
        }

        public ITwitterCredentials Credentials { get; set; }
        public IEnumerable<IMessage> LatestDirectMessages { get; set; }
        public IEnumerable<IMention> LatestMentionsTimeline { get; set; }
        public IEnumerable<ITweet> LatestHomeTimeline { get; set; }

        public IEnumerable<ISuggestedUserList> SuggestedUserList { get; set; }

        public T ExecuteAuthenticatedUserOperation<T>(Func<T> operation)
        {
            StartAuthenticatedUserOperation();
            var result = operation();
            CompletedAuthenticatedUserOperation();
            return result;
        }

        public void ExecuteAuthenticatedUserOperation(Action operation)
        {
            StartAuthenticatedUserOperation();
            operation();
            CompletedAuthenticatedUserOperation();
        }

        private void StartAuthenticatedUserOperation()
        {
            _savedCredentials = _credentialsAccessor.CurrentThreadCredentials;
            _credentialsAccessor.CurrentThreadCredentials = Credentials;
        }

        private void CompletedAuthenticatedUserOperation()
        {
            _credentialsAccessor.CurrentThreadCredentials = _savedCredentials;
        }

        // Home Timeline
        public Task<IEnumerable<ITweet>> GetHomeTimeline(int maximumNumberOfTweets = 40)
        {
            return ExecuteAuthenticatedUserOperation(() => _timelineController.GetHomeTimeline(maximumNumberOfTweets));
        }

        public Task<IEnumerable<ITweet>> GetHomeTimeline(IHomeTimelineParameters timelineRequestParameters)
        {
            return ExecuteAuthenticatedUserOperation(() => _timelineController.GetHomeTimeline(timelineRequestParameters));
        }

        // Mentions Timeline
        public Task<IEnumerable<IMention>> GetMentionsTimeline(int maximumNumberOfMentions = 40)
        {
            return ExecuteAuthenticatedUserOperation(() => _timelineController.GetMentionsTimeline(maximumNumberOfMentions));
        }

        // Frienships
        public override Task<IRelationshipDetails> GetRelationshipWith(IUserIdentifier user)
        {
            return ExecuteAuthenticatedUserOperation(() => _friendshipController.GetRelationshipBetween(this, user));
        }

        public Task<IRelationshipDetails> GetRelationshipWith(long userId)
        {
            return ExecuteAuthenticatedUserOperation(() => _friendshipController.GetRelationshipBetween(Id, userId));
        }

        public Task<IRelationshipDetails> GetRelationshipWith(string screenName)
        {
            return ExecuteAuthenticatedUserOperation(() => _friendshipController.GetRelationshipBetween(ScreenName, screenName));
        }

        public Task<bool> UpdateRelationshipAuthorizationsWith(IUserIdentifier user, bool retweetsEnabled, bool deviceNotificationsEnabled)
        {
            return ExecuteAuthenticatedUserOperation(() => _friendshipController.UpdateRelationshipAuthorizationsWith(user, retweetsEnabled, deviceNotificationsEnabled));
        }

        public Task<bool> UpdateRelationshipAuthorizationsWith(long userId, bool retweetsEnabled, bool deviceNotificationsEnabled)
        {
            return ExecuteAuthenticatedUserOperation(() => _friendshipController.UpdateRelationshipAuthorizationsWith(userId, retweetsEnabled, deviceNotificationsEnabled));
        }

        public Task<bool> UpdateRelationshipAuthorizationsWith(string screenName, bool retweetsEnabled, bool deviceNotificationsEnabled)
        {
            return ExecuteAuthenticatedUserOperation(() => _friendshipController.UpdateRelationshipAuthorizationsWith(screenName, retweetsEnabled, deviceNotificationsEnabled));
        }

        // Friends - Followers
        public Task<IEnumerable<IUser>> GetUsersRequestingFriendship(int maximumUserIdsToRetrieve = TweetinviConsts.FRIENDSHIPS_INCOMING_USERS_MAX_PER_REQ)
        {
            return ExecuteAuthenticatedUserOperation(() => _friendshipController.GetUsersRequestingFriendship(maximumUserIdsToRetrieve));
        }

        public Task<IEnumerable<IUser>> GetUsersYouRequestedToFollow(int maximumUserIdsToRetrieve = TweetinviConsts.FRIENDSHIPS_OUTGOING_USERS_MAX_PER_REQ)
        {
            return ExecuteAuthenticatedUserOperation(() => _friendshipController.GetUsersYouRequestedToFollow(maximumUserIdsToRetrieve));
        }

        // Follow
        public Task<bool> FollowUser(IUserIdentifier user)
        {
            return ExecuteAuthenticatedUserOperation(() => _friendshipController.CreateFriendshipWith(user));
        }

        public Task<bool> FollowUser(long userId)
        {
            return ExecuteAuthenticatedUserOperation(() => _friendshipController.CreateFriendshipWith(userId));
        }

        public Task<bool> FollowUser(string screenName)
        {
            return ExecuteAuthenticatedUserOperation(() => _friendshipController.CreateFriendshipWith(screenName));
        }

        public Task<bool> UnFollowUser(IUserIdentifier user)
        {
            return ExecuteAuthenticatedUserOperation(() => _friendshipController.DestroyFriendshipWith(user));
        }

        public Task<bool> UnFollowUser(long userId)
        {
            return ExecuteAuthenticatedUserOperation(() => _friendshipController.DestroyFriendshipWith(userId));
        }

        public Task<bool> UnFollowUser(string screenName)
        {
            return ExecuteAuthenticatedUserOperation(() => _friendshipController.DestroyFriendshipWith(screenName));
        }

        public Task<IEnumerable<ISavedSearch>> GetSavedSearches()
        {
            return ExecuteAuthenticatedUserOperation(() => _savedSearchController.GetSavedSearches());
        }

        // Block
        public override Task<bool> BlockUser()
        {
            throw new InvalidOperationException("You cannot block yourself...");
        }

        public Task<bool> BlockUser(IUserIdentifier user)
        {
            return Client.Users.BlockUser(user);
        }

        public Task<bool> BlockUser(long? userId)
        {
            return Client.Users.BlockUser(userId);
        }

        public Task<bool> BlockUser(string username)
        {
            return Client.Users.BlockUser(username);
        }

        // Unblock
        public override Task<bool> UnBlockUser()
        {
            throw new InvalidOperationException("You cannot unblock yourself...");
        }

        public Task<bool> UnBlockUser(IUserIdentifier user)
        {
            return Client.Users.UnblockUser(user);
        }

        public Task<bool> UnBlockUser(long userId)
        {
            return Client.Users.UnblockUser(userId);
        }

        public Task<bool> UnBlockUser(string username)
        {
            return Client.Users.UnblockUser(username);
        }

        // Get Blocked Users
        public Task<ICursorResult<long>> GetBlockedUserIds()
        {
            return Client.Users.GetBlockedUserIds();
        }

        public Task<IEnumerable<IUser>> GetBlockedUsers()
        {
            return ExecuteAuthenticatedUserOperation(() => _userController.GetBlockedUsers());
        }

        // Spam
        public override Task<bool> ReportUserForSpam()
        {
            throw new InvalidOperationException("You cannot report yourself for spam...");
        }

        public Task<bool> ReportUserForSpam(IUserIdentifier user)
        {
            throw new NotImplementedException("TODO");
        }

        public Task<bool> ReportUserForSpam(string userName)
        {
            throw new NotImplementedException("TODO");
        }

        public Task<bool> ReportUserForSpam(long userId)
        {
            throw new NotImplementedException("TODO");
        }

        // Direct Messages
        public async Task<IEnumerable<IMessage>> GetLatestMessages(int count)
        {
            var asyncOperation = await GetLatestMessagesWithCursor(count);
            return asyncOperation.Result;
        }

        public Task<AsyncCursorResult<IEnumerable<IMessage>>> GetLatestMessagesWithCursor(int count)
        {
            return ExecuteAuthenticatedUserOperation(() => _messageController.GetLatestMessages(count));
        }

        public Task<IMessage> PublishMessage(IPublishMessageParameters publishMessageParameters)
        {
            return ExecuteAuthenticatedUserOperation(() => _messageController.PublishMessage(publishMessageParameters));
        }

        // Tweet
        public Task<ITweet> PublishTweet(IPublishTweetParameters parameters)
        {
            return Client.Tweets.PublishTweet(parameters);
        }

        public Task<ITweet> PublishTweet(string text)
        {
            return Client.Tweets.PublishTweet(text);
        }

        // Settings
        
        /// <summary>
        /// Retrieve the settings of the Token's owner
        /// </summary>
        public Task<IAccountSettings> GetAccountSettings()
        {
            return ExecuteAuthenticatedUserOperation(() => _accountController.GetAuthenticatedUserSettings());
        }

        public Task<IAccountSettings> UpdateAccountSettings(
            IEnumerable<Language> languages = null,
            string timeZone = null,
            long? trendLocationWoeid = null,
            bool? sleepTimeEnabled = null,
            int? startSleepTime = null,
            int? endSleepTime = null)
        {
            return ExecuteAuthenticatedUserOperation(() => _accountController.UpdateAuthenticatedUserSettings(
                languages,
                timeZone,
                trendLocationWoeid,
                sleepTimeEnabled,
                startSleepTime,
                endSleepTime));
        }

        public Task<IAccountSettings> UpdateAccountSettings(IAccountSettingsRequestParameters accountSettingsRequestParameters)
        {
            return ExecuteAuthenticatedUserOperation(() => _accountController.UpdateAuthenticatedUserSettings(accountSettingsRequestParameters));
        }

        // Twitter Lists
        public Task<bool> SubscribeToList(ITwitterListIdentifier list)
        {
            return ExecuteAuthenticatedUserOperation(() => _twitterListController.SubscribeAuthenticatedUserToList(list));
        }

        public Task<bool> SubscribeToList(long listId)
        {
            return ExecuteAuthenticatedUserOperation(() => _twitterListController.SubscribeAuthenticatedUserToList(listId));
        }

        public Task<bool> SubscribeToList(string slug, long ownerId)
        {
            return ExecuteAuthenticatedUserOperation(() => _twitterListController.SubscribeAuthenticatedUserToList(slug, ownerId));
        }

        public Task<bool> SubscribeToList(string slug, string ownerScreenName)
        {
            return ExecuteAuthenticatedUserOperation(() => _twitterListController.SubscribeAuthenticatedUserToList(slug, ownerScreenName));
        }

        public Task<bool> SubscribeToList(string slug, IUserIdentifier owner)
        {
            return ExecuteAuthenticatedUserOperation(() => _twitterListController.SubscribeAuthenticatedUserToList(slug, owner));
        }

        public Task<bool> UnSubscribeFromList(ITwitterListIdentifier list)
        {
            return ExecuteAuthenticatedUserOperation(() => _twitterListController.UnSubscribeAuthenticatedUserFromList(list));
        }

        public Task<bool> UnSubscribeFromList(long listId)
        {
            return ExecuteAuthenticatedUserOperation(() => _twitterListController.UnSubscribeAuthenticatedUserFromList(listId));
        }

        public Task<bool> UnSubscribeFromList(string slug, long ownerId)
        {
            return ExecuteAuthenticatedUserOperation(() => _twitterListController.UnSubscribeAuthenticatedUserFromList(slug, ownerId));
        }

        public Task<bool> UnSubscribeFromList(string slug, string ownerScreenName)
        {
            return ExecuteAuthenticatedUserOperation(() => _twitterListController.UnSubscribeAuthenticatedUserFromList(slug, ownerScreenName));
        }

        public Task<bool> UnSubscribeFromList(string slug, IUserIdentifier owner)
        {
            return ExecuteAuthenticatedUserOperation(() => _twitterListController.UnSubscribeAuthenticatedUserFromList(slug, owner));
        }

        // Mute
        public Task<IEnumerable<long>> GetMutedUserIds(int maxUserIdsToRetrieve = Int32.MaxValue)
        {
            return ExecuteAuthenticatedUserOperation(() => _accountController.GetMutedUserIds(maxUserIdsToRetrieve));
        }

        public Task<IEnumerable<IUser>> GetMutedUsers(int maxUsersToRetrieve = 250)
        {
            return ExecuteAuthenticatedUserOperation(() => _accountController.GetMutedUsers(maxUsersToRetrieve));
        }

        public Task<bool> MuteUser(IUserIdentifier user)
        {
            return ExecuteAuthenticatedUserOperation(() => _accountController.MuteUser(user));
        }

        public Task<bool> MuteUser(long userId)
        {
            return ExecuteAuthenticatedUserOperation(() => _accountController.MuteUser(userId));
        }

        public Task<bool> MuteUser(string screenName)
        {
            return ExecuteAuthenticatedUserOperation(() => _accountController.MuteUser(screenName));
        }

        public Task<bool> UnMuteUser(IUserIdentifier user)
        {
            return ExecuteAuthenticatedUserOperation(() => _accountController.UnMuteUser(user));
        }

        public Task<bool> UnMuteUser(long userId)
        {
            return ExecuteAuthenticatedUserOperation(() => _accountController.UnMuteUser(userId));
        }

        public Task<bool> UnMuteUser(string screenName)
        {
            return ExecuteAuthenticatedUserOperation(() => _accountController.UnMuteUser(screenName));
        }
    }
}