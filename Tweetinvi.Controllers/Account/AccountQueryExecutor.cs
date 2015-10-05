using System;
using System.Collections.Generic;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.DTO.QueryDTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Controllers.Account
{
    public interface IAccountQueryExecutor
    {
        IAccountSettingsDTO GetLoggedUserAccountSettings();
        IAccountSettingsDTO UpdateLoggedUserSettings(IAccountSettingsRequestParameters accountSettingsRequestParameters);

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

        public IAccountSettingsDTO GetLoggedUserAccountSettings()
        {
            string query = _accountQueryGenerator.GetLoggedUserAccountSettingsQuery();
            return _twitterAccessor.ExecuteGETQuery<IAccountSettingsDTO>(query);
        }

        public IAccountSettingsDTO UpdateLoggedUserSettings(IAccountSettingsRequestParameters accountSettingsRequestParameters)
        {
            string query = _accountQueryGenerator.GetUpdateLoggedUserAccountSettingsQuery(accountSettingsRequestParameters);
            return _twitterAccessor.ExecutePOSTQuery<IAccountSettingsDTO>(query);
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