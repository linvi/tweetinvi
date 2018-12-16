using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi
{
    public static class AccountAsync
    {
        public static ConfiguredTaskAwaitable<IAccountSettings> GetCurrentAccountSettings()
        {
            return Sync.ExecuteTaskAsync(Account.GetCurrentAccountSettings);
        }

        public static ConfiguredTaskAwaitable<IAccountSettings> UpdateAccountSettings(
            IEnumerable<Language> languages = null,
            string timeZone = null,
            long? trendLocationWoeid = null,
            bool? sleepTimeEnabled = null,
            int? startSleepTime = null,
            int? endSleepTime = null)
        {
            return Sync.ExecuteTaskAsync(() => Account.UpdateAccountSettings(languages, timeZone, trendLocationWoeid, sleepTimeEnabled, startSleepTime, endSleepTime));
        }

        public static ConfiguredTaskAwaitable<IAccountSettings> UpdateAccountSettings(IAccountSettingsRequestParameters settings)
        {
            return Sync.ExecuteTaskAsync(() => Account.UpdateAccountSettings(settings));
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
        public static ConfiguredTaskAwaitable<IEnumerable<long>> GetMutedUserIds(int maxNumberOfUserIdsToRetrieve = Int32.MaxValue)
        {
            return Sync.ExecuteTaskAsync(() => Account.GetMutedUserIds(maxNumberOfUserIdsToRetrieve));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<IUser>> GetMutedUsers(int maxNumberOfUsersToRetrieve = 250)
        {
            return Sync.ExecuteTaskAsync(() => Account.GetMutedUsers(maxNumberOfUsersToRetrieve));
        }

        public static ConfiguredTaskAwaitable<bool> MuteUser(IUserIdentifier user)
        {
            return Sync.ExecuteTaskAsync(() => Account.MuteUser(user));
        }

        public static ConfiguredTaskAwaitable<bool> MuteUser(long userId)
        {
            return Sync.ExecuteTaskAsync(() => Account.MuteUser(userId));
        }

        public static ConfiguredTaskAwaitable<bool> MuteUser(string screenName)
        {
            return Sync.ExecuteTaskAsync(() => Account.MuteUser(screenName));
        }

        public static ConfiguredTaskAwaitable<bool> UnMuteUser(IUserIdentifier user)
        {
            return Sync.ExecuteTaskAsync(() => Account.UnMuteUser(user));
        }

        public static ConfiguredTaskAwaitable<bool> UnMuteUser(long userId)
        {
            return Sync.ExecuteTaskAsync(() => Account.UnMuteUser(userId));
        }

        public static ConfiguredTaskAwaitable<bool> UnMuteUser(string screenName)
        {
            return Sync.ExecuteTaskAsync(() => Account.UnMuteUser(screenName));
        }


        #region Friendship

        public static ConfiguredTaskAwaitable<IEnumerable<long>> GetUserIdsRequestingFriendship(int maximumUserIdsToRetrieve = 75000)
        {
            return Sync.ExecuteTaskAsync(() => Account.GetUserIdsRequestingFriendship(maximumUserIdsToRetrieve));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<IUser>> GetUsersRequestingFriendship(int maximumUserIdsToRetrieve = 75000)
        {
            return Sync.ExecuteTaskAsync(() => Account.GetUsersRequestingFriendship(maximumUserIdsToRetrieve));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<long>> GetUserIdsYouRequestedToFollow(int maximumUserIdsToRetrieve = 75000)
        {
            return Sync.ExecuteTaskAsync(() => Account.GetUserIdsYouRequestedToFollow(maximumUserIdsToRetrieve));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<IUser>> GetUsersYouRequestedToFollow(int maximumUserIdsToRetrieve = 75000)
        {
            return Sync.ExecuteTaskAsync(() => Account.GetUsersYouRequestedToFollow(maximumUserIdsToRetrieve));
        }

        // Update Relationship Authorization With
        public static ConfiguredTaskAwaitable<bool> UpdateRelationshipAuthorizationsWith(IUserIdentifier user, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            return Sync.ExecuteTaskAsync(() => Account.UpdateRelationshipAuthorizationsWith(user, retweetsEnabled, deviceNotifictionEnabled));
        }

        public static ConfiguredTaskAwaitable<bool> UpdateRelationshipAuthorizationsWith(long userId, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            return Sync.ExecuteTaskAsync(() => Account.UpdateRelationshipAuthorizationsWith(userId, retweetsEnabled, deviceNotifictionEnabled));
        }

        public static ConfiguredTaskAwaitable<bool> UpdateRelationshipAuthorizationsWith(string userScreenName, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            return Sync.ExecuteTaskAsync(() => Account.UpdateRelationshipAuthorizationsWith(userScreenName, retweetsEnabled, deviceNotifictionEnabled));
        }


        // Lookup Relationships
        public static ConfiguredTaskAwaitable<IEnumerable<IRelationshipState>> GetMultipleRelationships(IEnumerable<IUserIdentifier> targetUserIdentifiers)
        {
            return Sync.ExecuteTaskAsync(() => Account.GetRelationshipsWith(targetUserIdentifiers));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<IRelationshipState>> GetMultipleRelationships(IEnumerable<long> targetUserIds)
        {
            return Sync.ExecuteTaskAsync(() => Account.GetRelationshipsWith(targetUserIds));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<IRelationshipState>> GetMultipleRelationships(IEnumerable<string> targetUserScreenNames)
        {
            return Sync.ExecuteTaskAsync(() => Account.GetRelationshipsWith(targetUserScreenNames));
        }


        // Get User Ids Whose Retweets Are Muted
        public static ConfiguredTaskAwaitable<IEnumerable<long>> GetUserIdsWhoseRetweetsAreMuted()
        {
            return Sync.ExecuteTaskAsync(() => Account.GetUserIdsWhoseRetweetsAreMuted());
        }

        public static ConfiguredTaskAwaitable<IEnumerable<IUser>> GetUsersWhoseRetweetsAreMuted()
        {
            return Sync.ExecuteTaskAsync(() => Account.GetUsersWhoseRetweetsAreMuted());
        }

        #endregion
    }
}
