﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Interfaces.QueryValidators;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Controllers.Account
{
    public interface IAccountQueryGenerator
    {
        string GetAuthenticatedUserAccountSettingsQuery();
        string GetUpdateAuthenticatedUserAccountSettingsQuery(IAccountSettingsRequestParameters accountSettingsRequestParameters);

        string GetAccountUpdateDeliveryDeviceQuery(UpdateDeliveryDeviceType device, bool? includeEntities);

        // Mute
        string GetMutedUserIdsQuery();

        string GetMuteQuery(IUserIdentifier userIdentifier);
        string GetMuteQuery(long userId);
        string GetMuteQuery(string screenName);

        string GetUnMuteQuery(IUserIdentifier userIdentifier);
        string GetUnMuteQuery(long userId);
        string GetUnMuteQuery(string screenName);

        // Suggestions
        string GetSuggestedCategories(Language? language);
        string GetUserSuggestionsQuery(string slug, Language? language);
        string GetSuggestedUsersWithTheirLatestTweetQuery(string slug);
    }

    public class AccountQueryGenerator : IAccountQueryGenerator
    {
        private readonly IUserQueryValidator _userQueryValidator;
        private readonly IUserQueryParameterGenerator _userQueryParameterGenerator;
        private readonly IQueryParameterGenerator _queryParameterGenerator;

        public AccountQueryGenerator(
            IUserQueryValidator userQueryValidator,
            IUserQueryParameterGenerator userQueryParameterGenerator,
            IQueryParameterGenerator queryParameterGenerator)
        {
            _userQueryValidator = userQueryValidator;
            _userQueryParameterGenerator = userQueryParameterGenerator;
            _queryParameterGenerator = queryParameterGenerator;
        }

        public string GetAuthenticatedUserAccountSettingsQuery()
        {
            return Resources.Account_GetSettings;
        }

        public string GetUpdateAuthenticatedUserAccountSettingsQuery(IAccountSettingsRequestParameters accountSettingsRequestParameters)
        {
            var baseQuery = new StringBuilder(Resources.Account_UpdateSettings);

            baseQuery.Append(GetLanguagesParameter(accountSettingsRequestParameters.Languages));
            baseQuery.AddParameterToQuery("time_zone", accountSettingsRequestParameters.TimeZone);
            baseQuery.AddParameterToQuery("sleep_time_enabled", accountSettingsRequestParameters.SleepTimeEnabled);
            baseQuery.AddParameterToQuery("start_sleep_time", accountSettingsRequestParameters.StartSleepTime);
            baseQuery.AddParameterToQuery("end_sleep_time", accountSettingsRequestParameters.EndSleepTime);
            baseQuery.AddParameterToQuery("trend_location_woeid", accountSettingsRequestParameters.TrendLocationWoeid);

            baseQuery.Append(_queryParameterGenerator.GenerateAdditionalRequestParameters(accountSettingsRequestParameters.FormattedCustomQueryParameters));

            return baseQuery.ToString();
        }

        private string GetLanguagesParameter(IEnumerable<Language> languages)
        {
            // ReSharper disable once SimplifyLinqExpression
            if (languages == null || !languages.Any(x => x != Language.Undefined))
            {
                return string.Empty;
            }

            var validLanguages = languages.Where(x => x != Language.Undefined).Select(x => x.GetLanguageCode());
            var parameters = string.Join(Uri.EscapeDataString(", "), validLanguages);

            return string.Format("&lang={0}", parameters);
        }

        public string GetAccountUpdateDeliveryDeviceQuery(UpdateDeliveryDeviceType device, bool? includeEntities)
        {
            string queryParams;
            if (includeEntities.HasValue)
            {
                queryParams = string.Format("device={0}&include_entities={1}", device.ToString().ToLower(), includeEntities.Value);
            }
            else {
                queryParams = string.Format("device={0}", device.ToString().ToLower());
            }
            return string.Format(Resources.Account_UpdateDeliveryDevice, queryParams);
        }

        // Mute
        public string GetMutedUserIdsQuery()
        {
            return Resources.Account_Mute_GetIds;
        }

        public string GetMuteQuery(IUserIdentifier userIdentifier)
        {
            if (!_userQueryValidator.CanUserBeIdentified(userIdentifier))
            {
                return null;
            }

            string userIdParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userIdentifier);
            return GenerateCreateMuteQuery(userIdParameter);
        }

        public string GetMuteQuery(long userId)
        {
            if (!_userQueryValidator.IsUserIdValid(userId))
            {
                return null;
            }

            string userIdParameter = _userQueryParameterGenerator.GenerateUserIdParameter(userId);
            return GenerateCreateMuteQuery(userIdParameter);
        }

        public string GetMuteQuery(string screenName)
        {
            if (!_userQueryValidator.IsScreenNameValid(screenName))
            {
                return null;
            }

            string userScreenNameParameter = _userQueryParameterGenerator.GenerateScreenNameParameter(screenName);
            return GenerateCreateMuteQuery(userScreenNameParameter);
        }

        private string GenerateCreateMuteQuery(string userIdentifierParameter)
        {
            return string.Format(Resources.Account_Mute_Create, userIdentifierParameter);
        }

        public string GetUnMuteQuery(IUserIdentifier userIdentifier)
        {
            if (!_userQueryValidator.CanUserBeIdentified(userIdentifier))
            {
                return null;
            }

            string userIdParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userIdentifier);
            return GenerateUnMuteQuery(userIdParameter);
        }

        public string GetUnMuteQuery(long userId)
        {
            if (!_userQueryValidator.IsUserIdValid(userId))
            {
                return null;
            }

            string userIdParameter = _userQueryParameterGenerator.GenerateUserIdParameter(userId);
            return GenerateUnMuteQuery(userIdParameter);
        }

        public string GetUnMuteQuery(string screenName)
        {
            if (!_userQueryValidator.IsScreenNameValid(screenName))
            {
                return null;
            }

            string userScreenNameParameter = _userQueryParameterGenerator.GenerateScreenNameParameter(screenName);
            return GenerateUnMuteQuery(userScreenNameParameter);
        }

        private string GenerateUnMuteQuery(string userIdentifierParameter)
        {
            return string.Format(Resources.Account_Mute_Destroy, userIdentifierParameter);
        }

        // Suggestions
        public string GetSuggestedCategories(Language? language)
        {
            var languageParameter = _queryParameterGenerator.GenerateLanguageParameter(language);

            return string.Format(Resources.Account_CategoriesSuggestions, languageParameter);
        }

        public string GetUserSuggestionsQuery(string slug, Language? language)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return null;
            }

            var languageParameter = _queryParameterGenerator.GenerateLanguageParameter(language);

            return string.Format(Resources.Account_UserSuggestions, slug, languageParameter);
        }

        public string GetSuggestedUsersWithTheirLatestTweetQuery(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return null;
            }

            return string.Format(Resources.Account_MembersSuggestions, slug);
        }
    }
}