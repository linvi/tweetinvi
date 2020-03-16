using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
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
        private readonly ISavedSearchController _savedSearchController;

        public AuthenticatedUser(IUserDTO userDTO, ITwitterClient client) : base(userDTO, client)
        {
            var executionContext = client.CreateTwitterExecutionContext();
            _savedSearchController = executionContext.Container.Resolve<ISavedSearchController>();
        }

        public string Email => UserDTO.Email;

        public IReadOnlyTwitterCredentials Credentials => Client.Credentials;

        // Home Timeline
        public ITwitterIterator<ITweet, long?> GetHomeTimelineIterator()
        {
            return Client.Timelines.GetHomeTimelineIterator();
        }

        public ITwitterIterator<ITweet, long?> GetHomeTimelineIterator(IGetHomeTimelineParameters parameters)
        {
            return Client.Timelines.GetHomeTimelineIterator(parameters);
        }

        public ITwitterIterator<ITweet, long?> GetMentionsTimelineIterator()
        {
            return Client.Timelines.GetMentionsTimelineIterator();
        }

        public ITwitterIterator<ITweet, long?> GetMentionsTimelineIterator(IGetMentionsTimelineParameters parameters)
        {
            return Client.Timelines.GetMentionsTimelineIterator();
        }

        // Friendships
        public Task UpdateRelationship(IUpdateRelationshipParameters parameters)
        {
            return Client.Users.UpdateRelationship(parameters);
        }

        // Friends - Followers
        public ITwitterIterator<long> GetUserIdsRequestingFriendship()
        {
            return Client.Users.GetUserIdsRequestingFriendship(new GetUserIdsRequestingFriendshipParameters());
        }

        public IMultiLevelCursorIterator<long, IUser> GetUsersRequestingFriendship()
        {
            return Client.Users.GetUsersRequestingFriendship(new GetUsersRequestingFriendshipParameters());
        }

        public ITwitterIterator<long> GetUserIdsYouRequestedToFollow()
        {
            return Client.Users.GetUserIdsYouRequestedToFollow();
        }

        public IMultiLevelCursorIterator<long, IUser> GetUsersYouRequestedToFollow()
        {
            return Client.Users.GetUsersYouRequestedToFollow(new GetUsersYouRequestedToFollowParameters());
        }


        // Follow
        public Task FollowUser(IUserIdentifier user)
        {
            return Client.Users.FollowUser(user);
        }

        public Task FollowUser(long userId)
        {
            return Client.Users.FollowUser(userId);
        }

        public Task FollowUser(string username)
        {
            return Client.Users.FollowUser(username);
        }

        public Task UnfollowUser(IUserIdentifier user)
        {
            return Client.Users.UnfollowUser(user);
        }

        public Task UnfollowUser(long userId)
        {
            return Client.Users.UnfollowUser(userId);
        }

        public Task UnfollowUser(string username)
        {
            return Client.Users.UnfollowUser(username);
        }

        public Task<IEnumerable<ISavedSearch>> GetSavedSearches()
        {
            return _savedSearchController.GetSavedSearches();
        }

        // Block
        public override Task BlockUser()
        {
            throw new InvalidOperationException("You cannot block yourself...");
        }

        public Task BlockUser(IUserIdentifier user)
        {
            return Client.Users.BlockUser(user);
        }

        public Task BlockUser(long userId)
        {
            return Client.Users.BlockUser(userId);
        }

        public Task BlockUser(string username)
        {
            return Client.Users.BlockUser(username);
        }

        // Unblock
        public override Task UnblockUser()
        {
            throw new InvalidOperationException("You cannot unblock yourself...");
        }

        public Task UnblockUser(IUserIdentifier user)
        {
            return Client.Users.UnblockUser(user);
        }

        public Task UnblockUser(long userId)
        {
            return Client.Users.UnblockUser(userId);
        }

        public Task UnblockUser(string username)
        {
            return Client.Users.UnblockUser(username);
        }

        // Get Blocked Users
        public ITwitterIterator<long> GetBlockedUserIds()
        {
            return Client.Users.GetBlockedUserIds();
        }

        public ITwitterIterator<IUser> GetBlockedUsers()
        {
            return Client.Users.GetBlockedUsers();
        }

        // Spam
        public override Task ReportUserForSpam()
        {
            throw new InvalidOperationException("You cannot report yourself for spam...");
        }

        public Task ReportUserForSpam(IUserIdentifier user)
        {
            return Client.Users.ReportUserForSpam(user);
        }

        public Task ReportUserForSpam(string username)
        {
            return Client.Users.ReportUserForSpam(username);
        }

        public Task ReportUserForSpam(long userId)
        {
            return Client.Users.ReportUserForSpam(userId);
        }

        // Direct Messages
        public ITwitterIterator<IMessage> GetLatestMessages()
        {
            throw new NotImplementedException();
        }

        public Task<IMessage> PublishMessage(IPublishMessageParameters publishMessageParameters)
        {
            return Client.Messages.PublishMessage(publishMessageParameters);
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
        public Task<ITwitterList> SubscribeToList(ITwitterListIdentifier list)
        {
            return Client.Lists.SubscribeToList(list);
        }

        public Task<ITwitterList> SubscribeToList(long? listId)
        {
            return Client.Lists.SubscribeToList(listId);
        }

        public Task<ITwitterList> UnsubscribeFromList(ITwitterListIdentifier list)
        {
            return Client.Lists.UnsubscribeFromList(list);
        }

        public Task<ITwitterList> UnsubscribeFromList(long? listId)
        {
            return Client.Lists.UnsubscribeFromList(listId);
        }

        // Mute
        public ITwitterIterator<long> GetMutedUserIds()
        {
            return Client.Users.GetMutedUserIds();
        }

        public ITwitterIterator<IUser> GetMutedUsers()
        {
            return Client.Users.GetMutedUsers();
        }

        public Task MuteUser(IUserIdentifier user)
        {
            return Client.Users.MuteUser(user);
        }

        public Task MuteUser(long userId)
        {
            return Client.Users.MuteUser(userId);
        }

        public Task MuteUser(string username)
        {
            return Client.Users.MuteUser(username);
        }

        public Task UnmuteUser(IUserIdentifier user)
        {
            return Client.Users.UnmuteUser(user);
        }

        public Task UnmuteUser(long userId)
        {
            return Client.Users.UnmuteUser(userId);
        }

        public Task UnmuteUser(string username)
        {
            return Client.Users.UnmuteUser(username);
        }
    }
}