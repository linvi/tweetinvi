using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi
{
    public static class AccountAsync
    {
        public static Task<IAccountSettings> GetCurrentAccountSettings()
        {
            return Sync.ExecuteTaskAsync(Account.GetCurrentAccountSettings);
        }

        public static Task<IAccountSettings> UpdateAccountSettings(
            IEnumerable<Language> languages = null,
            string timeZone = null,
            long? trendLocationWoeid = null,
            bool? sleepTimeEnabled = null,
            int? startSleepTime = null,
            int? endSleepTime = null)
        {
            return Sync.ExecuteTaskAsync(() => Account.UpdateAccountSettings(languages, timeZone, trendLocationWoeid, sleepTimeEnabled, startSleepTime, endSleepTime));
        }

        public static Task<IAccountSettings> UpdateAccountSettings(IAccountSettingsRequestParameters settings)
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
        public static Task<IEnumerable<long>> GetMutedUserIds(int maxNumberOfUserIdsToRetrieve = Int32.MaxValue)
        {
            return Sync.ExecuteTaskAsync(() => Account.GetMutedUserIds(maxNumberOfUserIdsToRetrieve));
        }

        public static Task<IEnumerable<IUser>> GetMutedUsers(int maxNumberOfUsersToRetrieve = 250)
        {
            return Sync.ExecuteTaskAsync(() => Account.GetMutedUsers(maxNumberOfUsersToRetrieve));
        }

        public static Task<bool> MuteUser(IUserIdentifier user)
        {
            return Sync.ExecuteTaskAsync(() => Account.MuteUser(user));
        }

        public static Task<bool> MuteUser(long userId)
        {
            return Sync.ExecuteTaskAsync(() => Account.MuteUser(userId));
        }

        public static Task<bool> MuteUser(string screenName)
        {
            return Sync.ExecuteTaskAsync(() => Account.MuteUser(screenName));
        }

        public static Task<bool> UnMuteUser(IUserIdentifier user)
        {
            return Sync.ExecuteTaskAsync(() => Account.UnMuteUser(user));
        }

        public static Task<bool> UnMuteUser(long userId)
        {
            return Sync.ExecuteTaskAsync(() => Account.UnMuteUser(userId));
        }

        public static Task<bool> UnMuteUser(string screenName)
        {
            return Sync.ExecuteTaskAsync(() => Account.UnMuteUser(screenName));
        }


        #region Friendship

        public static Task<IEnumerable<long>> GetUserIdsRequestingFriendship(int maximumUserIdsToRetrieve = 75000)
        {
            return Sync.ExecuteTaskAsync(() => Account.GetUserIdsRequestingFriendship(maximumUserIdsToRetrieve));
        }

        public static Task<IEnumerable<IUser>> GetUsersRequestingFriendship(int maximumUserIdsToRetrieve = 75000)
        {
            return Sync.ExecuteTaskAsync(() => Account.GetUsersRequestingFriendship(maximumUserIdsToRetrieve));
        }

        public static Task<IEnumerable<long>> GetUserIdsYouRequestedToFollow(int maximumUserIdsToRetrieve = 75000)
        {
            return Sync.ExecuteTaskAsync(() => Account.GetUserIdsYouRequestedToFollow(maximumUserIdsToRetrieve));
        }

        public static Task<IEnumerable<IUser>> GetUsersYouRequestedToFollow(int maximumUserIdsToRetrieve = 75000)
        {
            return Sync.ExecuteTaskAsync(() => Account.GetUsersYouRequestedToFollow(maximumUserIdsToRetrieve));
        }

        // Update Relationship Authorization With
        public static Task<bool> UpdateRelationshipAuthorizationsWith(IUserIdentifier user, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            return Sync.ExecuteTaskAsync(() => Account.UpdateRelationshipAuthorizationsWith(user, retweetsEnabled, deviceNotifictionEnabled));
        }

        public static Task<bool> UpdateRelationshipAuthorizationsWith(long userId, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            return Sync.ExecuteTaskAsync(() => Account.UpdateRelationshipAuthorizationsWith(userId, retweetsEnabled, deviceNotifictionEnabled));
        }

        public static Task<bool> UpdateRelationshipAuthorizationsWith(string userScreenName, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            return Sync.ExecuteTaskAsync(() => Account.UpdateRelationshipAuthorizationsWith(userScreenName, retweetsEnabled, deviceNotifictionEnabled));
        }


        // Lookup Relationships
        public static Task<IEnumerable<IRelationshipState>> GetMultipleRelationships(IEnumerable<IUserIdentifier> targetUserIdentifiers)
        {
            return Sync.ExecuteTaskAsync(() => Account.GetRelationshipsWith(targetUserIdentifiers));
        }

        public static Task<IEnumerable<IRelationshipState>> GetMultipleRelationships(IEnumerable<long> targetUserIds)
        {
            return Sync.ExecuteTaskAsync(() => Account.GetRelationshipsWith(targetUserIds));
        }

        public static Task<IEnumerable<IRelationshipState>> GetMultipleRelationships(IEnumerable<string> targetUserScreenNames)
        {
            return Sync.ExecuteTaskAsync(() => Account.GetRelationshipsWith(targetUserScreenNames));
        }


        // Get User Ids Whose Retweets Are Muted
        public static Task<IEnumerable<long>> GetUserIdsWhoseRetweetsAreMuted()
        {
            return Sync.ExecuteTaskAsync(() => Account.GetUserIdsWhoseRetweetsAreMuted());
        }

        public static Task<IEnumerable<IUser>> GetUsersWhoseRetweetsAreMuted()
        {
            return Sync.ExecuteTaskAsync(() => Account.GetUsersWhoseRetweetsAreMuted());
        }

        #endregion
    }
}
