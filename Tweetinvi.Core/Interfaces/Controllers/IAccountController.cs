using System;
using System.Collections.Generic;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Core.Interfaces.Controllers
{
    public interface IAccountController
    {
        IAccountSettings GetLoggedUserSettings();

        IAccountSettings UpdateLoggedUserSettings(
            IEnumerable<Language> languages = null,
            string timeZone = null,
            long? trendLocationWoeid = null,
            bool? sleepTimeEnabled = null,
            int? startSleepTime = null,
            int? endSleepTime = null);

        IAccountSettings UpdateLoggedUserSettings(IAccountSettingsRequestParameters accountSettingsRequestParameters);

        // Mute
        IEnumerable<long> GetMutedUserIds(int maxUserIds = Int32.MaxValue);
        IEnumerable<IUser> GetMutedUsers(int maxUsersToRetrieve = 250);

        bool MuteUser(IUserIdentifier userIdentifier);
        bool MuteUser(long userId);
        bool MuteUser(string screenName);

        bool UnMuteUser(IUserIdentifier userIdentifier);
        bool UnMuteUser(long userId);
        bool UnMuteUser(string screenName);

        // Suggestions
        IEnumerable<ICategorySuggestion> GetSuggestedCategories(Language? language);
        IEnumerable<IUser> GetSuggestedUsers(string slug, Language? language);
        IEnumerable<IUser> GetSuggestedUsersWithTheirLatestTweet(string slug);
    }
}