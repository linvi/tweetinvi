using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Account
{
    public interface IAccountQueryGenerator
    {
        string GetAuthenticatedUserAccountSettingsQuery();
        string GetUpdateAuthenticatedUserAccountSettingsQuery(IAccountSettingsRequestParameters accountSettingsRequestParameters);

        // Profile
        string GetUpdateProfileParametersQuery(IAccountUpdateProfileParameters parameters);
        string GetUpdateProfileImageQuery(IAccountUpdateProfileImageParameters parameters);

        string GetUpdateProfileBannerQuery(IAccountUpdateProfileBannerParameters parameters);
        string GetRemoveUserProfileBannerQuery();
        string GetUpdateProfilBackgroundImageQuery(IAccountUpdateProfileBackgroundImageParameters parameters);

        // Mute
        string GetMutedUserIdsQuery();

        string GetMuteQuery(IUserIdentifier user);

        string GetUnMuteQuery(IUserIdentifier user);

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

        // Profile
        public string GetUpdateProfileParametersQuery(IAccountUpdateProfileParameters parameters)
        {
            var query = new StringBuilder(Resources.Account_UpdateProfile);

            query.AddParameterToQuery("name", parameters.Name);
            query.AddParameterToQuery("url", parameters.Url);
            query.AddParameterToQuery("location", parameters.Location);
            query.AddParameterToQuery("description", parameters.Description);
            query.AddParameterToQuery("profile_link_color", parameters.ProfileLinkColor);
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddParameterToQuery("skip_status", parameters.SkipStatus);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetUpdateProfileImageQuery(IAccountUpdateProfileImageParameters parameters)
        {
            var query = new StringBuilder(Resources.Account_UpdateProfileImage);

            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddParameterToQuery("skip_status", parameters.SkipStatus);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetUpdateProfileBannerQuery(IAccountUpdateProfileBannerParameters parameters)
        {
            var query = new StringBuilder(Resources.Account_UpdateProfileBanner);

            query.AddParameterToQuery("width", parameters.Width);
            query.AddParameterToQuery("height", parameters.Height);
            query.AddParameterToQuery("offset_left", parameters.OffsetLeft);
            query.AddParameterToQuery("offset_top", parameters.OffsetTop);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetRemoveUserProfileBannerQuery()
        {
            return Resources.Account_RemoveProfileBanner;
        }

        public string GetUpdateProfilBackgroundImageQuery(IAccountUpdateProfileBackgroundImageParameters parameters)
        {
            var query = new StringBuilder(Resources.Account_UpdateProfileBackgroundImage);

            if (parameters.Binary == null)
            {
                query.AddParameterToQuery("media_id", parameters.MediaId);
            }

            query.AddParameterToQuery("tile", parameters.UseTileMode);
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddParameterToQuery("skip_status", parameters.SkipStatus);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        // Mute
        public string GetMutedUserIdsQuery()
        {
            return Resources.Account_Mute_GetIds;
        }

        public string GetMuteQuery(IUserIdentifier user)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(user);

            string userIdParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(user);
            return GenerateCreateMuteQuery(userIdParameter);
        }

        private string GenerateCreateMuteQuery(string userParameter)
        {
            return string.Format(Resources.Account_Mute_Create, userParameter);
        }

        public string GetUnMuteQuery(IUserIdentifier user)
        {
           _userQueryValidator.ThrowIfUserCannotBeIdentified(user);

            string userIdParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(user);
            return GenerateUnMuteQuery(userIdParameter);
        }

        private string GenerateUnMuteQuery(string userParameter)
        {
            return string.Format(Resources.Account_Mute_Destroy, userParameter);
        }

        // Suggestions
        public string GetSuggestedCategories(Language? language)
        {
            var languageParameter = _queryParameterGenerator.GenerateLanguageParameter(language);

            return string.Format(Resources.Account_CategoriesSuggestions, languageParameter);
        }

        public string GetUserSuggestionsQuery(string slug, Language? language)
        {
            if (slug == null)
            {
                throw new ArgumentNullException("Slug cannot be null.");
            }

            if (slug == "")
            {
                throw new ArgumentException("Slug cannot be empty.");
            }

            var languageParameter = _queryParameterGenerator.GenerateLanguageParameter(language);

            return string.Format(Resources.Account_UserSuggestions, slug, languageParameter);
        }

        public string GetSuggestedUsersWithTheirLatestTweetQuery(string slug)
        {
            if (slug == null)
            {
                throw new ArgumentNullException("Slug cannot be null.");
            }

            if (slug == "")
            {
                throw new ArgumentException("Slug cannot be empty.");
            }

            return string.Format(Resources.Account_MembersSuggestions, slug);
        }
    }
}