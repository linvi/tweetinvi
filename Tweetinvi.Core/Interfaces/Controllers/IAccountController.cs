using System;
using System.Collections.Generic;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Core.Interfaces.Controllers
{
    public interface IAccountController
    {
        IAccountSettings GetAuthenticatedUserSettings();

        IAccountSettings UpdateAuthenticatedUserSettings(
            IEnumerable<Language> languages = null,
            string timeZone = null,
            long? trendLocationWoeid = null,
            bool? sleepTimeEnabled = null,
            int? startSleepTime = null,
            int? endSleepTime = null);

        IAccountSettings UpdateAuthenticatedUserSettings(IAccountSettingsRequestParameters accountSettingsRequestParameters);

        // Profile
        IAuthenticatedUser UpdateAccountProfile(IAccountUpdateProfileParameters parameters);

        bool UpdateProfileImage(byte[] imageBinary);
        bool UpdateProfileImage(IAccountUpdateProfileImageParameters parameters);


        bool UpdateProfileBanner(byte[] imageBinary);
        bool UpdateProfileBanner(IAccountUpdateProfileBannerParameters parameters);
        bool RemoveUserProfileBanner();

        bool UpdateProfileBackgroundImage(byte[] imageBinary);
        bool UpdateProfileBackgroundImage(long mediaId);
        bool UpdateProfileBackgroundImage(IAccountUpdateProfileBackgroundImageParameters parameters);

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