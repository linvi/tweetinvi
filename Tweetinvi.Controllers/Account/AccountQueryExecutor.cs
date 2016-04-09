using System;
using System.Collections.Generic;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.DTO.QueryDTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Web;

namespace Tweetinvi.Controllers.Account
{
    public interface IAccountQueryExecutor
    {
        IAccountSettingsDTO GetAuthenticatedUserAccountSettings();
        IAccountSettingsDTO UpdateAuthenticatedUserSettings(IAccountSettingsRequestParameters accountSettingsRequestParameters);

        // Profile
        bool UpdateProfileImage(IAccountUpdateProfileImageParameters parameters);
        IUserDTO UpdateProfileParameters(IAccountUpdateProfileParameters parameters);
        bool UpdateProfileBanner(IAccountUpdateProfileBannerParameters parameters);
        bool RemoveUserProfileBanner();
        bool UpdateProfileBackgroundImage(IAccountUpdateProfileBackgroundImageParameters parameters);

        // Mute
        IEnumerable<long> GetMutedUserIds(int maxUserIds = Int32.MaxValue);

        bool MuteUser(IUserIdentifier userIdentifier);
        bool MuteUser(long userId);
        bool MuteUser(string screenName);

        bool UnMuteUser(IUserIdentifier userIdentifier);
        bool UnMuteUser(long userId);
        bool UnMuteUser(string screenName);

        // Suggestions
        IEnumerable<IUserDTO> GetSuggestedUsers(string slug, Language? language);
        IEnumerable<ICategorySuggestion> GetSuggestedCategories(Language? language);
        IEnumerable<IUserDTO> GetSuggestedUsersWithTheirLatestTweet(string slug);
    }

    public class AccountQueryExecutor : IAccountQueryExecutor
    {
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly IAccountQueryGenerator _accountQueryGenerator;

        public AccountQueryExecutor(
            ITwitterAccessor twitterAccessor,
            IAccountQueryGenerator accountQueryGenerator)
        {
            _twitterAccessor = twitterAccessor;
            _accountQueryGenerator = accountQueryGenerator;
        }

        public IAccountSettingsDTO GetAuthenticatedUserAccountSettings()
        {
            var query = _accountQueryGenerator.GetAuthenticatedUserAccountSettingsQuery();
            return _twitterAccessor.ExecuteGETQuery<IAccountSettingsDTO>(query);
        }

        public IAccountSettingsDTO UpdateAuthenticatedUserSettings(IAccountSettingsRequestParameters accountSettingsRequestParameters)
        {
            var query = _accountQueryGenerator.GetUpdateAuthenticatedUserAccountSettingsQuery(accountSettingsRequestParameters);
            return _twitterAccessor.ExecutePOSTQuery<IAccountSettingsDTO>(query);
        }

        public bool UpdateProfileImage(IAccountUpdateProfileImageParameters parameters)
        {
            var query = _accountQueryGenerator.GetUpdateProfileImageQuery(parameters);

            return _twitterAccessor.TryExecuteMultipartQuery(new MultipartHttpRequestParameters
            {
                Query = query,
                HttpMethod = HttpMethod.POST,
                Binaries = new [] { parameters.Binary },
                ContentId = "image",
                Timeout = parameters.Timeout
            });
        }

        public IUserDTO UpdateProfileParameters(IAccountUpdateProfileParameters parameters)
        {
            var query = _accountQueryGenerator.GetUpdateProfileParametersQuery(parameters);
            return _twitterAccessor.ExecutePOSTQuery<IUserDTO>(query);
        }

        public bool UpdateProfileBanner(IAccountUpdateProfileBannerParameters parameters)
        {
            var query = _accountQueryGenerator.GetUpdateProfileBannerQuery(parameters);

            if (parameters.Binary == null)
            {
                throw new ArgumentNullException("Banner binary cannot be null.");
            }

            var multipartParameters = new MultipartHttpRequestParameters
            {
                Query = query,
                HttpMethod = HttpMethod.POST,
                Binaries = new [] { parameters.Binary }, 
                ContentId = "banner",
                Timeout = parameters.Timeout
            };

            return _twitterAccessor.TryExecuteMultipartQuery(multipartParameters);
        }

        public bool RemoveUserProfileBanner()
        {
            var query = _accountQueryGenerator.GetRemoveUserProfileBannerQuery();
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public bool UpdateProfileBackgroundImage(IAccountUpdateProfileBackgroundImageParameters parameters)
        {
            var query = _accountQueryGenerator.GetUpdateProfilBackgroundImageQuery(parameters);

            if (parameters.Binary != null)
            {
                return _twitterAccessor.TryExecuteMultipartQuery(new MultipartHttpRequestParameters
                {
                    Query = query,
                    HttpMethod = HttpMethod.POST,
                    Binaries = new [] { parameters.Binary },
                    ContentId = "image",
                    Timeout = parameters.Timeout
                });
            }

            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        // Mute
        public IEnumerable<long> GetMutedUserIds(int maxUserIds = Int32.MaxValue)
        {
            string query = _accountQueryGenerator.GetMutedUserIdsQuery();
            return _twitterAccessor.ExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, maxUserIds);
        }

        public bool MuteUser(IUserIdentifier userIdentifier)
        {
            var query = _accountQueryGenerator.GetMuteQuery(userIdentifier);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public bool MuteUser(long userId)
        {
            var query = _accountQueryGenerator.GetMuteQuery(userId);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public bool MuteUser(string screenName)
        {
            var query = _accountQueryGenerator.GetMuteQuery(screenName);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public bool UnMuteUser(IUserIdentifier userIdentifier)
        {
            var query = _accountQueryGenerator.GetUnMuteQuery(userIdentifier);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public bool UnMuteUser(long userId)
        {
            var query = _accountQueryGenerator.GetUnMuteQuery(userId);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public bool UnMuteUser(string screenName)
        {
            var query = _accountQueryGenerator.GetUnMuteQuery(screenName);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        // Suggestions
        public IEnumerable<ICategorySuggestion> GetSuggestedCategories(Language? language)
        {
            var query = _accountQueryGenerator.GetSuggestedCategories(language);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ICategorySuggestion>>(query);
        }

        public IEnumerable<IUserDTO> GetSuggestedUsers(string slug, Language? language)
        {
            var query = _accountQueryGenerator.GetUserSuggestionsQuery(slug, language);
            return _twitterAccessor.ExecuteGETQueryWithPath<IEnumerable<IUserDTO>>(query, "users");
        }

        public IEnumerable<IUserDTO> GetSuggestedUsersWithTheirLatestTweet(string slug)
        {
            var query = _accountQueryGenerator.GetSuggestedUsersWithTheirLatestTweetQuery(slug);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<IUserDTO>>(query);
        }

    }
}