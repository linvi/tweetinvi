using System;
using System.Collections.Generic;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

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
        IEnumerable<long> GetMutedUserIds(int maxUserIds = int.MaxValue);

        bool MuteUser(IUserIdentifier user);
        bool UnMuteUser(IUserIdentifier user);

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
                Timeout = parameters.Timeout,
                UploadProgressChanged = parameters.UploadProgressChanged
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
                Timeout = parameters.Timeout,
                UploadProgressChanged = parameters.UploadProgressChanged
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
                    Timeout = parameters.Timeout,
                    UploadProgressChanged = parameters.UploadProgressChanged
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

        public bool MuteUser(IUserIdentifier user)
        {
            var query = _accountQueryGenerator.GetMuteQuery(user);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public bool UnMuteUser(IUserIdentifier user)
        {
            var query = _accountQueryGenerator.GetUnMuteQuery(user);
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