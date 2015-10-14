using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Logic
{
    /// <summary>
    /// A token user is unique to a Token and provides action that will
    /// be executed from the connected user and that are not available
    /// from another user like (read my messages)
    /// </summary>
    public class LoggedUser : User, ILoggedUser
    {
        private readonly ICredentialsAccessor _credentialsAccessor;
        private readonly ITweetController _tweetController;
        private readonly IMessageController _messageController;
        private readonly IFriendshipController _friendshipController;
        private readonly IAccountController _accountController;
        private readonly ITwitterListController _twitterListController;
        private readonly ISavedSearchController _savedSearchController;

        private ITwitterCredentials _savedCredentials;

        public LoggedUser(
            IUserDTO userDTO,
            ICredentialsAccessor credentialsAccessor,
            ITimelineController timelineController,
            ITweetController tweetController,
            IUserController userController,
            IMessageController messageController,
            IFriendshipController friendshipController,
            IAccountController accountController,
            ITwitterListController twitterListController,
            ISavedSearchController savedSearchController,
            ITaskFactory taskFactory)

            : base(userDTO, userController, timelineController, friendshipController, twitterListController, taskFactory)
        {
            _credentialsAccessor = credentialsAccessor;
            _tweetController = tweetController;
            _messageController = messageController;
            _friendshipController = friendshipController;
            _accountController = accountController;
            _twitterListController = twitterListController;
            _savedSearchController = savedSearchController;

            Credentials = _credentialsAccessor.CurrentThreadCredentials;
        }

        public void SetCredentials(ITwitterCredentials credentials)
        {
            Credentials = credentials;
        }

        public ITwitterCredentials Credentials { get; private set; }
        public IEnumerable<IMessage> LatestDirectMessagesReceived { get; set; }
        public IEnumerable<IMessage> LatestDirectMessagesSent { get; set; }
        public IEnumerable<IMention> LatestMentionsTimeline { get; set; }
        public IEnumerable<ITweet> LatestHomeTimeline { get; set; }

        public IEnumerable<ISuggestedUserList> SuggestedUserList { get; set; }

        public T ExecuteLoggedUserOperation<T>(Func<T> operation)
        {
            StartLoggedUserOperation();
            var result = operation();
            CompletedLoggedUserOperation();
            return result;
        }

        public void ExecuteLoggedUserOperation(Action operation)
        {
            StartLoggedUserOperation();
            operation();
            CompletedLoggedUserOperation();
        }

        private void StartLoggedUserOperation()
        {
            _savedCredentials = _credentialsAccessor.CurrentThreadCredentials;
            _credentialsAccessor.CurrentThreadCredentials = Credentials;
        }

        private void CompletedLoggedUserOperation()
        {
            _credentialsAccessor.CurrentThreadCredentials = _savedCredentials;
        }

        // Home Timeline
        public IEnumerable<ITweet> GetHomeTimeline(int maximumNumberOfTweets = 40)
        {
            return ExecuteLoggedUserOperation(() => _timelineController.GetHomeTimeline(maximumNumberOfTweets));
        }

        public IEnumerable<ITweet> GetHomeTimeline(IHomeTimelineParameters timelineRequestParameters)
        {
            return ExecuteLoggedUserOperation(() => _timelineController.GetHomeTimeline(timelineRequestParameters));
        }

        // Mentions Timeline
        public IEnumerable<IMention> GetMentionsTimeline(int maximumNumberOfMentions = 40)
        {
            return ExecuteLoggedUserOperation(() => _timelineController.GetMentionsTimeline(maximumNumberOfMentions));
        }

        // Frienships
        public IRelationshipDetails GetRelationshipWith(IUserIdentifier userIdentifier)
        {
            return ExecuteLoggedUserOperation(() => _friendshipController.GetRelationshipBetween(this, userIdentifier));
        }

        public IRelationshipDetails GetRelationshipWith(long userId)
        {
            return ExecuteLoggedUserOperation(() => _friendshipController.GetRelationshipBetween(Id, userId));
        }

        public IRelationshipDetails GetRelationshipWith(string screenName)
        {
            return ExecuteLoggedUserOperation(() => _friendshipController.GetRelationshipBetween(ScreenName, screenName));
        }

        public bool UpdateRelationshipAuthorizationsWith(IUserIdentifier userIdentifier, bool retweetsEnabled, bool deviceNotificationsEnabled)
        {
            return ExecuteLoggedUserOperation(() => _friendshipController.UpdateRelationshipAuthorizationsWith(userIdentifier, retweetsEnabled, deviceNotificationsEnabled));
        }

        public bool UpdateRelationshipAuthorizationsWith(long userId, bool retweetsEnabled, bool deviceNotificationsEnabled)
        {
            return ExecuteLoggedUserOperation(() => _friendshipController.UpdateRelationshipAuthorizationsWith(userId, retweetsEnabled, deviceNotificationsEnabled));
        }

        public bool UpdateRelationshipAuthorizationsWith(string screenName, bool retweetsEnabled, bool deviceNotificationsEnabled)
        {
            return ExecuteLoggedUserOperation(() => _friendshipController.UpdateRelationshipAuthorizationsWith(screenName, retweetsEnabled, deviceNotificationsEnabled));
        }

        // Friends - Followers
        public IEnumerable<IUser> GetUsersRequestingFriendship(int maximumUserIdsToRetrieve = 75000)
        {
            return ExecuteLoggedUserOperation(() => _friendshipController.GetUsersRequestingFriendship(maximumUserIdsToRetrieve));
        }

        public IEnumerable<IUser> GetUsersYouRequestedToFollow(int maximumUserIdsToRetrieve = 75000)
        {
            return ExecuteLoggedUserOperation(() => _friendshipController.GetUsersYouRequestedToFollow(maximumUserIdsToRetrieve));
        }

        // Follow
        public bool FollowUser(IUser user)
        {
            return ExecuteLoggedUserOperation(() => _friendshipController.CreateFriendshipWith(user));
        }

        public bool FollowUser(long userId)
        {
            return ExecuteLoggedUserOperation(() => _friendshipController.CreateFriendshipWith(userId));
        }

        public bool FollowUser(string screenName)
        {
            return ExecuteLoggedUserOperation(() => _friendshipController.CreateFriendshipWith(screenName));
        }

        public bool UnFollowUser(IUser user)
        {
            return ExecuteLoggedUserOperation(() => _friendshipController.DestroyFriendshipWith(user));
        }

        public bool UnFollowUser(long userId)
        {
            return ExecuteLoggedUserOperation(() => _friendshipController.DestroyFriendshipWith(userId));
        }

        public bool UnFollowUser(string screenName)
        {
            return ExecuteLoggedUserOperation(() => _friendshipController.DestroyFriendshipWith(screenName));
        }

        public IEnumerable<ISavedSearch> GetSavedSearches()
        {
            return ExecuteLoggedUserOperation(() => _savedSearchController.GetSavedSearches());
        }

        // Block
        public override bool BlockUser()
        {
            throw new InvalidOperationException("You cannot block yourself...");
        }

        public bool BlockUser(IUserIdentifier userIdentifier)
        {
            return ExecuteLoggedUserOperation(() => _userController.BlockUser(userIdentifier));
        }

        public bool BlockUser(long userId)
        {
            return ExecuteLoggedUserOperation(() => _userController.BlockUser(userId));
        }

        public bool BlockUser(string userName)
        {
            return ExecuteLoggedUserOperation(() => _userController.BlockUser(userName));
        }

        // Unblock
        public override bool UnBlockUser()
        {
            throw new InvalidOperationException("You cannot unblock yourself...");
        }

        public bool UnBlockUser(IUserIdentifier userIdentifier)
        {
            return ExecuteLoggedUserOperation(() => _userController.UnBlockUser(userIdentifier));
        }

        public bool UnBlockUser(long userId)
        {
            return ExecuteLoggedUserOperation(() => _userController.UnBlockUser(userId));
        }

        public bool UnBlockUser(string userName)
        {
            return ExecuteLoggedUserOperation(() => _userController.UnBlockUser(userName));
        }

        // Get Blocked Users
        public IEnumerable<long> GetBlockedUserIds()
        {
            return ExecuteLoggedUserOperation(() => _userController.GetBlockedUserIds());
        }

        public IEnumerable<IUser> GetBlockedUsers()
        {
            return ExecuteLoggedUserOperation(() => _userController.GetBlockedUsers());
        }

        // Spam
        public override bool ReportUserForSpam()
        {
            throw new InvalidOperationException("You cannot report yourself for spam...");
        }

        public bool ReportUserForSpam(IUser user)
        {
            return ExecuteLoggedUserOperation(() => _userController.BlockUser(user));
        }

        public bool ReportUserForSpam(IUserIdentifier userIdentifier)
        {
            return ExecuteLoggedUserOperation(() => _userController.BlockUser(userIdentifier));
        }

        public bool ReportUserForSpam(string userName)
        {
            return ExecuteLoggedUserOperation(() => _userController.BlockUser(userName));
        }

        public bool ReportUserForSpam(long userId)
        {
            return ExecuteLoggedUserOperation(() => _userController.BlockUser(userId));
        }

        // Direct Messages
        public IEnumerable<IMessage> GetLatestMessagesReceived(int maximumMessages = 40)
        {
            return ExecuteLoggedUserOperation(() => _messageController.GetLatestMessagesReceived(maximumMessages));
        }

        public IEnumerable<IMessage> GetLatestMessagesSent(int maximumMessages = 40)
        {
            return ExecuteLoggedUserOperation(() => _messageController.GetLatestMessagesSent(maximumMessages));
        }

        public IMessage PublishMessage(IMessage message)
        {
            return ExecuteLoggedUserOperation(() => _messageController.PublishMessage(message.MessageDTO));
        }

        // Tweet
        public ITweet PublishTweet(string text, IPublishTweetOptionalParameters parameters)
        {
            return ExecuteLoggedUserOperation(() =>  _tweetController.PublishTweet(text, parameters));
        }

        // Settings
        public IAccountSettings AccountSettings { get; set; }

        /// <summary>
        /// Retrieve the settings of the Token's owner
        /// </summary>
        public IAccountSettings GetAccountSettings()
        {
            return ExecuteLoggedUserOperation(() => _accountController.GetLoggedUserSettings());
        }

        public IAccountSettings UpdateAccountSettings(
            IEnumerable<Language> languages = null,
            string timeZone = null,
            long? trendLocationWoeid = null,
            bool? sleepTimeEnabled = null,
            int? startSleepTime = null,
            int? endSleepTime = null)
        {
            return ExecuteLoggedUserOperation(() => _accountController.UpdateLoggedUserSettings(
                languages,
                timeZone,
                trendLocationWoeid,
                sleepTimeEnabled,
                startSleepTime,
                endSleepTime));
        }

        public IAccountSettings UpdateAccountSettings(IAccountSettingsRequestParameters accountSettingsRequestParameters)
        {
            return ExecuteLoggedUserOperation(() => _accountController.UpdateLoggedUserSettings(accountSettingsRequestParameters));
        }

        // Twitter Lists
        public bool SubsribeToList(ITwitterListIdentifier list)
        {
            return ExecuteLoggedUserOperation(() => _twitterListController.SubscribeLoggedUserToList(list));
        }

        public bool SubsribeToList(long listId)
        {
            return ExecuteLoggedUserOperation(() => _twitterListController.SubscribeLoggedUserToList(listId));
        }

        public bool SubsribeToList(string slug, long ownerId)
        {
            return ExecuteLoggedUserOperation(() => _twitterListController.SubscribeLoggedUserToList(slug, ownerId));
        }

        public bool SubsribeToList(string slug, string ownerScreenName)
        {
            return ExecuteLoggedUserOperation(() => _twitterListController.SubscribeLoggedUserToList(slug, ownerScreenName));
        }

        public bool SubsribeToList(string slug, IUserIdentifier owner)
        {
            return ExecuteLoggedUserOperation(() => _twitterListController.SubscribeLoggedUserToList(slug, owner));
        }

        public bool UnSubscribeFromList(ITwitterListIdentifier list)
        {
            return ExecuteLoggedUserOperation(() => _twitterListController.UnSubscribeLoggedUserFromList(list));
        }

        public bool UnSubscribeFromList(long listId)
        {
            return ExecuteLoggedUserOperation(() => _twitterListController.UnSubscribeLoggedUserFromList(listId));
        }

        public bool UnSubscribeFromList(string slug, long ownerId)
        {
            return ExecuteLoggedUserOperation(() => _twitterListController.UnSubscribeLoggedUserFromList(slug, ownerId));
        }

        public bool UnSubscribeFromList(string slug, string ownerScreenName)
        {
            return ExecuteLoggedUserOperation(() => _twitterListController.UnSubscribeLoggedUserFromList(slug, ownerScreenName));
        }

        public bool UnSubscribeFromList(string slug, IUserIdentifier owner)
        {
            return ExecuteLoggedUserOperation(() => _twitterListController.UnSubscribeLoggedUserFromList(slug, owner));
        }

        // Mute
        public IEnumerable<long> GetMutedUserIds(int maxUserIdsToRetrieve = Int32.MaxValue)
        {
            return ExecuteLoggedUserOperation(() => _accountController.GetMutedUserIds(maxUserIdsToRetrieve));
        }

        public IEnumerable<IUser> GetMutedUsers(int maxUsersToRetrieve = 250)
        {
            return ExecuteLoggedUserOperation(() => _accountController.GetMutedUsers(maxUsersToRetrieve));
        }

        public bool MuteUser(IUserIdentifier userIdentifier)
        {
            return ExecuteLoggedUserOperation(() => _accountController.MuteUser(userIdentifier));
        }

        public bool MuteUser(long userId)
        {
            return ExecuteLoggedUserOperation(() => _accountController.MuteUser(userId));
        }

        public bool MuteUser(string screenName)
        {
            return ExecuteLoggedUserOperation(() => _accountController.MuteUser(screenName));
        }

        public bool UnMuteUser(IUserIdentifier userIdentifier)
        {
            return ExecuteLoggedUserOperation(() => _accountController.UnMuteUser(userIdentifier));
        }

        public bool UnMuteUser(long userId)
        {
            return ExecuteLoggedUserOperation(() => _accountController.UnMuteUser(userId));
        }

        public bool UnMuteUser(string screenName)
        {
            return ExecuteLoggedUserOperation(() => _accountController.UnMuteUser(screenName));
        }

        #region Async

        public async Task<IEnumerable<IMessage>> GetLatestMessagesReceivedAsync(int count = 40)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => GetLatestMessagesReceived(count)));
        }

        public async Task<IEnumerable<IMessage>> GetLatestMessagesSentAsync(int maximumMessages = 40)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => GetLatestMessagesSent(maximumMessages)));
        }

        
        public async Task<IMessage> PublishMessageAsync(IMessage message)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => PublishMessage(message)));
        }

        // Home Timeline
        public async Task<IEnumerable<ITweet>> GetHomeTimelineAsync(int count = 40)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => GetHomeTimeline(count)));
        }

        public async Task<IEnumerable<ITweet>> GetHomeTimelineAsync(IHomeTimelineParameters timelineRequestParameters)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => GetHomeTimeline(timelineRequestParameters)));
        }

        // Mentions Timeline
        public async Task<IEnumerable<IMention>> GetMentionsTimelineAsync(int count = 40)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => GetMentionsTimeline(count)));
        }

        // Relationships
        public async Task<IRelationshipDetails> GetRelationshipWithAsync(IUserIdentifier userIdentifier)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => GetRelationshipWith(userIdentifier)));
        }

        public async Task<IRelationshipDetails> GetRelationshipWithAsync(long userId)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => GetRelationshipWith(userId)));
        }

        public async Task<IRelationshipDetails> GetRelationshipWithAsync(string screenName)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => GetRelationshipWith(screenName)));
        }


        public async Task<bool> UpdateRelationshipAuthorizationsWithAsync(IUserIdentifier user, bool retweetsEnabled, bool deviceNotificationsEnabled)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => UpdateRelationshipAuthorizationsWith(user, retweetsEnabled, deviceNotificationsEnabled)));
        }

        public async Task<bool> UpdateRelationshipAuthorizationsWithAsync(long userId, bool retweetsEnabled, bool deviceNotificationsEnabled)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => UpdateRelationshipAuthorizationsWith(userId, retweetsEnabled, deviceNotificationsEnabled)));
        }

        public async Task<bool> UpdateRelationshipAuthorizationsWithAsync(string screenName, bool retweetsEnabled, bool deviceNotificationsEnabled)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => UpdateRelationshipAuthorizationsWith(screenName, retweetsEnabled, deviceNotificationsEnabled)));
        }

        
        public async Task<IEnumerable<IUser>> GetUsersRequestingFriendshipAsync(int maximumUserIdsToRetrieve = 75000)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => GetUsersRequestingFriendship(maximumUserIdsToRetrieve)));
        }

        public async Task<IEnumerable<IUser>> GetUsersYouRequestedToFollowAsync(int maximumUsersToRetrieve = 75000)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => GetUsersYouRequestedToFollow(maximumUsersToRetrieve)));

        }

      
        public async Task<bool> FollowUserAsync(IUser user)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => FollowUser(user)));
        }

        public async Task<bool> FollowUserAsync(long userId)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => FollowUser(userId)));
        }

        public async Task<bool> FollowUserAsync(string screenName)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => FollowUser(screenName)));
        }

        public async Task<bool> UnFollowUserAsync(IUser user)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => UnFollowUser(user)));
        }

        public async Task<bool> UnFollowUserAsync(long userId)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => UnFollowUser(userId)));
        }

        public async Task<bool> UnFollowUserAsync(string screenName)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => UnFollowUser(screenName)));
        }


        public async Task<IEnumerable<ISavedSearch>> GetSavedSearchesAsync()
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => GetSavedSearches()));
        }

        public async Task<IEnumerable<long>> GetBlockedUsersIdsAsync()
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => GetBlockedUserIds()));
        }

        public async Task<IEnumerable<IUser>> GetBlockedUsersAsync()
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => GetBlockedUsers()));
        }

        public async Task<IAccountSettings> GetAccountSettingsAsync()
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => GetAccountSettings()));
        }

        public async Task<IAccountSettings> UpdateAccountSettingsAsync(
            IEnumerable<Language> languages = null,
            string timeZone = null,
            long? trendLocationWoeid = null,
            bool? sleepTimeEnabled = null,
            int? startSleepTime = null,
            int? endSleepTime = null)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() =>
                UpdateAccountSettings(
                    languages,
                    timeZone,
                    trendLocationWoeid,
                    sleepTimeEnabled,
                    startSleepTime,
                    endSleepTime
                )));
        }

        public async Task<IAccountSettings> UpdateLoggedUserSettingsAsync(IAccountSettingsRequestParameters accountSettingsRequestParameters)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => UpdateAccountSettings(accountSettingsRequestParameters)));
        }

        // Subscribe to List
        public async Task<bool> SubscribeToListAsync(ITwitterListIdentifier list)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => _twitterListController.SubscribeLoggedUserToList(list)));
        }

        public async Task<bool> SubscribeToListAsync(long listId)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => _twitterListController.SubscribeLoggedUserToList(listId)));
        }

        public async Task<bool> SubscribeToListAsync(string slug, long ownerId)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => _twitterListController.SubscribeLoggedUserToList(slug, ownerId)));
        }

        public async Task<bool> SubscribeToListAsync(string slug, string ownerScreenName)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => _twitterListController.SubscribeLoggedUserToList(slug, ownerScreenName)));
        }

        public async Task<bool> SubscribeToListAsync(string slug, IUserIdentifier owner)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => _twitterListController.SubscribeLoggedUserToList(slug, owner)));
        }

        // Unsubscribe From list
        public async Task<bool> UnSubscribeFromListAsync(ITwitterListIdentifier list)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => _twitterListController.UnSubscribeLoggedUserFromList(list)));
        }

        public async Task<bool> UnSubscribeFromListAsync(long listId)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => _twitterListController.UnSubscribeLoggedUserFromList(listId)));
        }

        public async Task<bool> UnSubscribeFromListAsync(string slug, long ownerId)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => _twitterListController.UnSubscribeLoggedUserFromList(slug, ownerId)));
        }

        public async Task<bool> UnSubscribeFromListAsync(string slug, string ownerScreenName)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => _twitterListController.UnSubscribeLoggedUserFromList(slug, ownerScreenName)));
        }

        public async Task<bool> UnSubscribeFromListAsync(string slug, IUserIdentifier owner)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => _twitterListController.UnSubscribeLoggedUserFromList(slug, owner)));
        }

        // Get Muted Users
        public async Task<IEnumerable<long>> GetMutedUserIdsAsync(int maxUserIds = Int32.MaxValue)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => GetMutedUserIds(maxUserIds)));
        }

        public async Task<IEnumerable<IUser>> GetMutedUsersAsync(int maxUserIds = 250)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => GetMutedUsers(maxUserIds)));
        }

        public async Task<bool> MuteUserAsync(IUserIdentifier userIdentifier)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => MuteUser(userIdentifier)));
        }

        public async Task<bool> MuteUserAsync(long userId)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => MuteUser(userId)));
        }

        public async Task<bool> MuteUserAsync(string screenName)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => MuteUser(screenName)));
        }

        public async Task<bool> UnMuteUserAsync(IUserIdentifier userIdentifier)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => UnMuteUser(userIdentifier)));
        }

        public async Task<bool> UnMuteUserAsync(long userId)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => UnMuteUser(userId)));
        }

        public async Task<bool> UnMuteUserAsync(string screenName)
        {
            return await ExecuteLoggedUserOperation(() => _taskFactory.ExecuteTaskAsync(() => UnMuteUser(screenName)));
        }

        #endregion
    }
}