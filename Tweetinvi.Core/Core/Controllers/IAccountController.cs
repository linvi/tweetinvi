using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Controllers
{
    public interface IAccountController
    {
        Task<ITwitterResult<IUserDTO, IAuthenticatedUser>> GetAuthenticatedUser(IGetAuthenticatedUserParameters parameters, ITwitterRequest request);
        
        // FOLLOWERS
        Task<ITwitterResult<IUserDTO>> FollowUser(IFollowUserParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserDTO>> UnFollowUser(IUnFollowUserParameters parameters, ITwitterRequest request);
        ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetUserIdsRequestingFriendship(IGetUserIdsRequestingFriendshipParameters parameters, ITwitterRequest request);

        // BLOCK
        Task<ITwitterResult<IUserDTO>> BlockUser(IBlockUserParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserDTO>> UnblockUser(IUnblockUserParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserDTO>> ReportUserForSpam(IReportUserForSpamParameters parameters, ITwitterRequest request);
        ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetBlockedUserIds(IGetBlockedUserIdsParameters parameters, ITwitterRequest request);
        ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetBlockedUsers(IGetBlockedUsersParameters parameters, ITwitterRequest request);

        // FRIENDSHIPS
        Task<ITwitterResult<IRelationshipStateDTO[]>> GetRelationshipsWith(IGetRelationshipsWithParameters parameters, ITwitterRequest request);








        Task<IAccountSettings> GetAuthenticatedUserSettings();

        Task<IAccountSettings> UpdateAuthenticatedUserSettings(
            IEnumerable<Language> languages = null,
            string timeZone = null,
            long? trendLocationWoeid = null,
            bool? sleepTimeEnabled = null,
            int? startSleepTime = null,
            int? endSleepTime = null);

        Task<IAccountSettings> UpdateAuthenticatedUserSettings(IAccountSettingsRequestParameters accountSettingsRequestParameters);

        // Profile
        Task<IAuthenticatedUser> UpdateAccountProfile(IAccountUpdateProfileParameters parameters);

        Task<bool> UpdateProfileImage(byte[] imageBinary);
        Task<bool> UpdateProfileImage(IAccountUpdateProfileImageParameters parameters);


        Task<bool> UpdateProfileBanner(byte[] imageBinary);
        Task<bool> UpdateProfileBanner(IAccountUpdateProfileBannerParameters parameters);
        Task<bool> RemoveUserProfileBanner();

        Task<bool> UpdateProfileBackgroundImage(byte[] imageBinary);
        Task<bool> UpdateProfileBackgroundImage(long mediaId);
        Task<bool> UpdateProfileBackgroundImage(IAccountUpdateProfileBackgroundImageParameters parameters);

        // Mute
        Task<IEnumerable<long>> GetMutedUserIds(int maxUserIds = Int32.MaxValue);
        Task<IEnumerable<IUser>> GetMutedUsers(int maxUsersToRetrieve = 250);

        Task<bool> MuteUser(IUserIdentifier user);
        Task<bool> MuteUser(long userId);
        Task<bool> MuteUser(string screenName);

        Task<bool> UnMuteUser(IUserIdentifier user);
        Task<bool> UnMuteUser(long userId);
        Task<bool> UnMuteUser(string screenName);

        // Suggestions
        Task<IEnumerable<ICategorySuggestion>> GetSuggestedCategories(Language? language);
        Task<IEnumerable<IUser>> GetSuggestedUsers(string slug, Language? language);
        Task<IEnumerable<IUser>> GetSuggestedUsersWithTheirLatestTweet(string slug);
        IAccountSettings GenerateAccountSettingsFromJson(string json);
    }
}