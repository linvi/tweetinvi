using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi
{
    public static class AccountAsync
    {
        public static async Task<IAccountSettings> GetCurrentAccountSettings()
        {
            return await Sync.ExecuteTaskAsync(() => Account.GetCurrentAccountSettings());
        }

        public static async Task<IAccountSettings> UpdateAccountSettings(
            IEnumerable<Language> languages = null,
            string timeZone = null,
            long? trendLocationWoeid = null,
            bool? sleepTimeEnabled = null,
            int? startSleepTime = null,
            int? endSleepTime = null)
        {
            return await Sync.ExecuteTaskAsync(() => Account.UpdateAccountSettings(languages, timeZone, trendLocationWoeid, sleepTimeEnabled, startSleepTime, endSleepTime));
        }

        public static async Task<IAccountSettings> UpdateAccountSettings(IAccountSettingsRequestParameters settings)
        {
            return await Sync.ExecuteTaskAsync(() => Account.UpdateAccountSettings(settings));
        }

        public static IAccountSettingsRequestParameters CreateUpdateAccountSettingsRequestParameters()
        {
            return Account.CreateUpdateAccountSettingsRequestParameters();
        }

        public static IAccountSettingsRequestParameters CreateUpdateAccountSettingsRequestParameters(IAccountSettings accountSettings)
        {
            return Account.CreateUpdateAccountSettingsRequestParameters(accountSettings);
        }

        // Mute
        public static async Task<IEnumerable<long>> GetMutedUserIds(int maxNumberOfUserIdsToRetrieve = Int32.MaxValue)
        {
            return await Sync.ExecuteTaskAsync(() => Account.GetMutedUserIds(maxNumberOfUserIdsToRetrieve));
        }

        public static async Task<IEnumerable<IUser>> GetMutedUsers(int maxNumberOfUsersToRetrieve = 250)
        {
            return await Sync.ExecuteTaskAsync(() => Account.GetMutedUsers(maxNumberOfUsersToRetrieve));
        }

        public static async Task<bool> MuteUser(IUserIdentifier userIdentifier)
        {
            return await Sync.ExecuteTaskAsync(() => Account.MuteUser(userIdentifier));
        }

        public static async Task<bool> MuteUser(long userId)
        {
            return await Sync.ExecuteTaskAsync(() => Account.MuteUser(userId));
        }

        public static async Task<bool> MuteUser(string screenName)
        {
            return await Sync.ExecuteTaskAsync(() => Account.MuteUser(screenName));
        }

        public static async Task<bool> UnMuteUser(IUserIdentifier userIdentifier)
        {
            return await Sync.ExecuteTaskAsync(() => Account.UnMuteUser(userIdentifier));
        }

        public static async Task<bool> UnMuteUser(long userId)
        {
            return await Sync.ExecuteTaskAsync(() => Account.UnMuteUser(userId));
        }

        public static async Task<bool> UnMuteUser(string screenName)
        {
            return await Sync.ExecuteTaskAsync(() => Account.UnMuteUser(screenName));
        }


        #region Friendship

        public static async Task<IEnumerable<long>> GetUserIdsRequestingFriendship(int maximumUserIdsToRetrieve = 75000)
        {
            return await Sync.ExecuteTaskAsync(() => Account.GetUserIdsRequestingFriendship(maximumUserIdsToRetrieve));
        }

        public static async Task<IEnumerable<IUser>> GetUsersRequestingFriendship(int maximumUserIdsToRetrieve = 75000)
        {
            return await Sync.ExecuteTaskAsync(() => Account.GetUsersRequestingFriendship(maximumUserIdsToRetrieve));
        }

        public static async Task<IEnumerable<long>> GetUserIdsYouRequestedToFollow(int maximumUserIdsToRetrieve = 75000)
        {
            return await Sync.ExecuteTaskAsync(() => Account.GetUserIdsYouRequestedToFollow(maximumUserIdsToRetrieve));
        }

        public static async Task<IEnumerable<IUser>> GetUsersYouRequestedToFollow(int maximumUserIdsToRetrieve = 75000)
        {
            return await Sync.ExecuteTaskAsync(() => Account.GetUsersYouRequestedToFollow(maximumUserIdsToRetrieve));
        }

        // Update Relationship Authorization With
        public static async Task<bool> UpdateRelationshipAuthorizationsWith(IUserIdentifier userIdentifier, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            return await Sync.ExecuteTaskAsync(() => Account.UpdateRelationshipAuthorizationsWith(userIdentifier, retweetsEnabled, deviceNotifictionEnabled));
        }

        public static async Task<bool> UpdateRelationshipAuthorizationsWith(long userId, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            return await Sync.ExecuteTaskAsync(() => Account.UpdateRelationshipAuthorizationsWith(userId, retweetsEnabled, deviceNotifictionEnabled));
        }

        public static async Task<bool> UpdateRelationshipAuthorizationsWith(string userScreenName, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            return await Sync.ExecuteTaskAsync(() => Account.UpdateRelationshipAuthorizationsWith(userScreenName, retweetsEnabled, deviceNotifictionEnabled));
        }


        // Lookup Relationships
        public static async Task<IEnumerable<IRelationshipState>> GetMultipleRelationships(IEnumerable<IUserIdentifier> targetUserIdentifiers)
        {
            return await Sync.ExecuteTaskAsync(() => Account.GetRelationshipsWith(targetUserIdentifiers));
        }

        public static async Task<IEnumerable<IRelationshipState>> GetMultipleRelationships(IEnumerable<long> targetUserIds)
        {
            return await Sync.ExecuteTaskAsync(() => Account.GetRelationshipsWith(targetUserIds));
        }

        public static async Task<IEnumerable<IRelationshipState>> GetMultipleRelationships(IEnumerable<string> targetUserScreenNames)
        {
            return await Sync.ExecuteTaskAsync(() => Account.GetRelationshipsWith(targetUserScreenNames));
        }


        // Get User Ids Whose Retweets Are Muted
        public static async Task<IEnumerable<long>> GetUserIdsWhoseRetweetsAreMuted()
        {
            return await Sync.ExecuteTaskAsync(() => Account.GetUserIdsWhoseRetweetsAreMuted());
        }

        public static async Task<IEnumerable<IUser>> GetUsersWhoseRetweetsAreMuted()
        {
            return await Sync.ExecuteTaskAsync(() => Account.GetUsersWhoseRetweetsAreMuted());
        }

        #endregion
    }
}
