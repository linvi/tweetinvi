using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Models.Authentication;
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
        public AuthenticatedUser(IUserDTO userDTO, ITwitterClient client) : base(userDTO, client)
        {
        }

        public string Email => UserDTO.Email;

        public IReadOnlyTwitterCredentials Credentials => Client.Credentials;

        // Home Timeline
        public Task<ITweet[]> GetHomeTimelineAsync()
        {
            return Client.Timelines.GetHomeTimelineAsync();
        }

        public Task<ITweet[]> GetMentionsTimelineAsync()
        {
            return Client.Timelines.GetMentionsTimelineAsync();
        }

        // Friendships
        public Task UpdateRelationshipAsync(IUpdateRelationshipParameters parameters)
        {
            return Client.Users.UpdateRelationshipAsync(parameters);
        }

        // Friends - Followers
        public Task<long[]> GetUserIdsRequestingFriendshipAsync()
        {
            return Client.Users.GetUserIdsRequestingFriendshipAsync(new GetUserIdsRequestingFriendshipParameters());
        }

        public Task<IUser[]> GetUsersRequestingFriendshipAsync()
        {
            return Client.Users.GetUsersRequestingFriendshipAsync(new GetUsersRequestingFriendshipParameters());
        }

        public Task<long[]> GetUserIdsYouRequestedToFollowAsync()
        {
            return Client.Users.GetUserIdsYouRequestedToFollowAsync();
        }

        public Task<IUser[]> GetUsersYouRequestedToFollowAsync()
        {
            return Client.Users.GetUsersYouRequestedToFollowAsync(new GetUsersYouRequestedToFollowParameters());
        }


        // Follow
        public Task FollowUserAsync(IUserIdentifier user)
        {
            return Client.Users.FollowUserAsync(user);
        }

        public Task FollowUserAsync(long userId)
        {
            return Client.Users.FollowUserAsync(userId);
        }

        public Task FollowUserAsync(string username)
        {
            return Client.Users.FollowUserAsync(username);
        }

        public Task UnfollowUserAsync(IUserIdentifier user)
        {
            return Client.Users.UnfollowUserAsync(user);
        }

        public Task UnfollowUserAsync(long userId)
        {
            return Client.Users.UnfollowUserAsync(userId);
        }

        public Task UnfollowUserAsync(string username)
        {
            return Client.Users.UnfollowUserAsync(username);
        }

        public Task<ISavedSearch[]> ListSavedSearchesAsync()
        {
            return Client.Search.ListSavedSearchesAsync();
        }

        // Block
        public override Task BlockUserAsync()
        {
            throw new InvalidOperationException("You cannot block yourself...");
        }

        public Task BlockUserAsync(IUserIdentifier user)
        {
            return Client.Users.BlockUserAsync(user);
        }

        public Task BlockUserAsync(long userId)
        {
            return Client.Users.BlockUserAsync(userId);
        }

        public Task BlockUserAsync(string username)
        {
            return Client.Users.BlockUserAsync(username);
        }

        // Unblock
        public override Task UnblockUserAsync()
        {
            throw new InvalidOperationException("You cannot unblock yourself...");
        }

        public Task UnblockUserAsync(IUserIdentifier user)
        {
            return Client.Users.UnblockUserAsync(user);
        }

        public Task UnblockUserAsync(long userId)
        {
            return Client.Users.UnblockUserAsync(userId);
        }

        public Task UnblockUserAsync(string username)
        {
            return Client.Users.UnblockUserAsync(username);
        }

        // Get Blocked Users
        public Task<long[]> GetBlockedUserIdsAsync()
        {
            return Client.Users.GetBlockedUserIdsAsync();
        }

        public Task<IUser[]> GetBlockedUsersAsync()
        {
            return Client.Users.GetBlockedUsersAsync();
        }

        // Spam
        public override Task ReportUserForSpamAsync()
        {
            throw new InvalidOperationException("You cannot report yourself for spam...");
        }

        public Task ReportUserForSpamAsync(IUserIdentifier user)
        {
            return Client.Users.ReportUserForSpamAsync(user);
        }

        public Task ReportUserForSpamAsync(string username)
        {
            return Client.Users.ReportUserForSpamAsync(username);
        }

        public Task ReportUserForSpamAsync(long userId)
        {
            return Client.Users.ReportUserForSpamAsync(userId);
        }

        // Direct Messages
        public Task<IMessage[]> GetLatestMessagesAsync()
        {
            return Client.Messages.GetMessagesAsync();
        }

        public Task<IMessage> PublishMessageAsync(IPublishMessageParameters publishMessageParameters)
        {
            return Client.Messages.PublishMessageAsync(publishMessageParameters);
        }

        // Tweet
        public Task<ITweet> PublishTweetAsync(IPublishTweetParameters parameters)
        {
            return Client.Tweets.PublishTweetAsync(parameters);
        }

        public Task<ITweet> PublishTweetAsync(string text)
        {
            return Client.Tweets.PublishTweetAsync(text);
        }

        // Settings
        public Task<IAccountSettings> UpdateAccountSettingsAsync(IUpdateAccountSettingsParameters parameters)
        {
            return Client.AccountSettings.UpdateAccountSettingsAsync(parameters);
        }

        // Twitter Lists
        public Task<ITwitterList> SubscribeToListAsync(ITwitterListIdentifier list)
        {
            return Client.Lists.SubscribeToListAsync(list);
        }

        public Task<ITwitterList> SubscribeToListAsync(long listId)
        {
            return Client.Lists.SubscribeToListAsync(listId);
        }

        public Task<ITwitterList> UnsubscribeFromListAsync(ITwitterListIdentifier list)
        {
            return Client.Lists.UnsubscribeFromListAsync(list);
        }

        public Task<ITwitterList> UnsubscribeFromListAsync(long listId)
        {
            return Client.Lists.UnsubscribeFromListAsync(listId);
        }

        // Mute
        public Task<long[]> GetMutedUserIdsAsync()
        {
            return Client.Users.GetMutedUserIdsAsync();
        }

        public Task<IUser[]> GetMutedUsersAsync()
        {
            return Client.Users.GetMutedUsersAsync();
        }

        public Task MuteUserAsync(IUserIdentifier user)
        {
            return Client.Users.MuteUserAsync(user);
        }

        public Task MuteUserAsync(long userId)
        {
            return Client.Users.MuteUserAsync(userId);
        }

        public Task MuteUserAsync(string username)
        {
            return Client.Users.MuteUserAsync(username);
        }

        public Task UnmuteUserAsync(IUserIdentifier user)
        {
            return Client.Users.UnmuteUserAsync(user);
        }

        public Task UnmuteUserAsync(long userId)
        {
            return Client.Users.UnmuteUserAsync(userId);
        }

        public Task UnmuteUserAsync(string username)
        {
            return Client.Users.UnmuteUserAsync(username);
        }
    }
}