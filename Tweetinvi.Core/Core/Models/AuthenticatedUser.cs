using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Iterators;
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
        private readonly ITwitterListController _twitterListController;
        private readonly ISavedSearchController _savedSearchController;

        private ITwitterCredentials _savedCredentials;

        public AuthenticatedUser(
            IUserDTO userDTO,
            ICredentialsAccessor credentialsAccessor,
            ITimelineController timelineController,
            IMessageController messageController,
            ITwitterListController twitterListController,
            ISavedSearchController savedSearchController)

            : base(userDTO, timelineController, twitterListController)
        {
            _credentialsAccessor = credentialsAccessor;
            _messageController = messageController;
            _twitterListController = twitterListController;
            _savedSearchController = savedSearchController;

            Credentials = _credentialsAccessor.CurrentThreadCredentials;
        }

        public string Email => UserDTO.Email;

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
        public ITwitterIterator<ITweet, long?> GetHomeTimeline()
        {
            return Client.Timeline.GetHomeTimelineIterator();
        }

        public ITwitterIterator<ITweet, long?> GetHomeTimeline(IGetHomeTimelineParameters parameters)
        {
            return Client.Timeline.GetHomeTimelineIterator(parameters);
        }

        // Mentions Timeline
        public Task<IEnumerable<IMention>> GetMentionsTimeline(int maximumNumberOfMentions = 40)
        {
            return ExecuteAuthenticatedUserOperation(() => _timelineController.GetMentionsTimeline(maximumNumberOfMentions));
        }

        // Friendships
        public Task<bool> UpdateRelationship(IUpdateRelationshipParameters parameters)
        {
            return Client.Account.UpdateRelationship(parameters);
        }

        // Friends - Followers
        public ITwitterIterator<long> GetUserIdsRequestingFriendship()
        {
            return Client.Account.GetUserIdsRequestingFriendship(new GetUserIdsRequestingFriendshipParameters());
        }

        public IMultiLevelCursorIterator<long, IUser> GetUsersRequestingFriendship()
        {
            return Client.Account.GetUsersRequestingFriendship(new GetUsersRequestingFriendshipParameters());
        }

        public ITwitterIterator<long> GetUserIdsYouRequestedToFollow()
        {
            return Client.Account.GetUserIdsYouRequestedToFollow();
        }

        public IMultiLevelCursorIterator<long, IUser> GetUsersYouRequestedToFollow()
        {
            return Client.Account.GetUsersYouRequestedToFollow(new GetUsersYouRequestedToFollowParameters());
        }


        // Follow
        public Task<bool> FollowUser(IUserIdentifier user)
        {
            return Client.Account.FollowUser(user);
        }

        public Task<bool> FollowUser(long userId)
        {
            return Client.Account.FollowUser(userId);
        }

        public Task<bool> FollowUser(string username)
        {
            return Client.Account.FollowUser(username);
        }

        public Task<bool> UnFollowUser(IUserIdentifier user)
        {
            return Client.Account.UnFollowUser(user);
        }

        public Task<bool> UnFollowUser(long userId)
        {
            return Client.Account.UnFollowUser(userId);
        }

        public Task<bool> UnFollowUser(string username)
        {
            return Client.Account.UnFollowUser(username);
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
            return Client.Account.BlockUser(user);
        }

        public Task<bool> BlockUser(long? userId)
        {
            return Client.Account.BlockUser(userId);
        }

        public Task<bool> BlockUser(string username)
        {
            return Client.Account.BlockUser(username);
        }

        // Unblock
        public override Task<bool> UnBlockUser()
        {
            throw new InvalidOperationException("You cannot unblock yourself...");
        }

        public Task<bool> UnBlockUser(IUserIdentifier user)
        {
            return Client.Account.UnBlockUser(user);
        }

        public Task<bool> UnBlockUser(long userId)
        {
            return Client.Account.UnblockUser(userId);
        }

        public Task<bool> UnBlockUser(string username)
        {
            return Client.Account.UnblockUser(username);
        }

        // Get Blocked Users
        public ITwitterIterator<long> GetBlockedUserIds()
        {
            return Client.Account.GetBlockedUserIds();
        }

        public ITwitterIterator<IUser> GetBlockedUsers()
        {
            return Client.Account.GetBlockedUsers();
        }

        // Spam
        public override Task<bool> ReportUserForSpam()
        {
            throw new InvalidOperationException("You cannot report yourself for spam...");
        }

        public Task<bool> ReportUserForSpam(IUserIdentifier user)
        {
            return Client.Account.ReportUserForSpam(user);
        }

        public Task<bool> ReportUserForSpam(string username)
        {
            return Client.Account.ReportUserForSpam(username);
        }

        public Task<bool> ReportUserForSpam(long? userId)
        {
            return Client.Account.ReportUserForSpam(userId);
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
        public Task<IAccountSettings> UpdateAccountSettings(IUpdateAccountSettingsParameters parameters)
        {
            return Client.AccountSettings.UpdateAccountSettings(parameters);
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
        public ITwitterIterator<long> GetMutedUserIds()
        {
            return Client.Account.GetMutedUserIds();
        }

        public ITwitterIterator<IUser> GetMutedUsers()
        {
            return Client.Account.GetMutedUsers();
        }

        public Task<bool> MuteUser(IUserIdentifier user)
        {
            return Client.Account.MuteUser(user);
        }

        public Task<bool> MuteUser(long? userId)
        {
            return Client.Account.MuteUser(userId);
        }

        public Task<bool> MuteUser(string username)
        {
            return Client.Account.MuteUser(username);
        }

        public Task<bool> UnMuteUser(IUserIdentifier user)
        {
            return Client.Account.UnMuteUser(user);
        }

        public Task<bool> UnMuteUser(long userId)
        {
            return Client.Account.UnMuteUser(userId);
        }

        public Task<bool> UnMuteUser(string username)
        {
            return Client.Account.UnMuteUser(username);
        }
    }
}