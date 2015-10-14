using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Core.Interfaces.Async
{
    public interface ILoggedUserAsync
    {
        Task<IEnumerable<IMessage>> GetLatestMessagesReceivedAsync(int count = 40);
        Task<IEnumerable<IMessage>> GetLatestMessagesSentAsync(int maximumMessages = 40);
        Task<IMessage> PublishMessageAsync(IMessage message);

        Task<IEnumerable<ITweet>> GetHomeTimelineAsync(int count = 40);
        Task<IEnumerable<ITweet>> GetHomeTimelineAsync(IHomeTimelineParameters timelineRequestParameters);
        Task<IEnumerable<IMention>> GetMentionsTimelineAsync(int count = 40);

        Task<IRelationshipDetails> GetRelationshipWithAsync(IUserIdentifier userIdentifier);
        Task<IRelationshipDetails> GetRelationshipWithAsync(long userId);
        Task<IRelationshipDetails> GetRelationshipWithAsync(string screenName);

        Task<bool> UpdateRelationshipAuthorizationsWithAsync(IUserIdentifier user, bool retweetsEnabled, bool deviceNotificationsEnabled);
        Task<bool> UpdateRelationshipAuthorizationsWithAsync(long userId, bool retweetsEnabled, bool deviceNotificationsEnabled);
        Task<bool> UpdateRelationshipAuthorizationsWithAsync(string screenName, bool retweetsEnabled, bool deviceNotificationsEnabled);

        Task<IEnumerable<IUser>> GetUsersRequestingFriendshipAsync(int maximumUserIdsToRetrieve = 75000);
        Task<IEnumerable<IUser>> GetUsersYouRequestedToFollowAsync(int maximumUsersToRetrieve = 75000);

        Task<bool> FollowUserAsync(IUser user);
        Task<bool> FollowUserAsync(long userId);
        Task<bool> FollowUserAsync(string screenName);

        Task<bool> UnFollowUserAsync(IUser user);
        Task<bool> UnFollowUserAsync(long userId);
        Task<bool> UnFollowUserAsync(string screenName);

        Task<IEnumerable<ISavedSearch>> GetSavedSearchesAsync();

        Task<IEnumerable<long>> GetBlockedUsersIdsAsync();
        Task<IEnumerable<IUser>> GetBlockedUsersAsync();

        Task<IAccountSettings> GetAccountSettingsAsync();

        Task<IAccountSettings> UpdateAccountSettingsAsync(
            IEnumerable<Language> languages = null,
            string timeZone = null,
            long? trendLocationWoeid = null,
            bool? sleepTimeEnabled = null,
            int? startSleepTime = null,
            int? endSleepTime = null);

        Task<IAccountSettings> UpdateLoggedUserSettingsAsync(IAccountSettingsRequestParameters accountSettingsRequestParameters);

            // Mute
        Task<IEnumerable<long>> GetMutedUserIdsAsync(int maxUserIds = Int32.MaxValue);
        Task<IEnumerable<IUser>> GetMutedUsersAsync(int maxUserIds = 250);

        Task<bool> MuteUserAsync(IUserIdentifier userIdentifier);
        Task<bool> MuteUserAsync(long userId);
        Task<bool> MuteUserAsync(string screenName);

        Task<bool> UnMuteUserAsync(IUserIdentifier userIdentifier);
        Task<bool> UnMuteUserAsync(long userId);
        Task<bool> UnMuteUserAsync(string screenName);

        // Subscription
        Task<bool> SubscribeToListAsync(ITwitterListIdentifier list);
        Task<bool> SubscribeToListAsync(long listId);
        Task<bool> SubscribeToListAsync(string slug, long ownerId);
        Task<bool> SubscribeToListAsync(string slug, string ownerScreenName);
        Task<bool> SubscribeToListAsync(string slug, IUserIdentifier owner);
        Task<bool> UnSubscribeFromListAsync(ITwitterListIdentifier list);
        Task<bool> UnSubscribeFromListAsync(long listId);
        Task<bool> UnSubscribeFromListAsync(string slug, long ownerId);
        Task<bool> UnSubscribeFromListAsync(string slug, string ownerScreenName);
        Task<bool> UnSubscribeFromListAsync(string slug, IUserIdentifier owner);
    }
}