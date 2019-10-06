using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.AccountSettings
{
    public interface IAccountSettingsQueryGenerator
    {
        string GetAccountSettingsQuery(IGetAccountSettingsParameters parameters);
        string GetUpdateAccountSettingsQuery(IUpdateAccountSettingsParameters parameters);
        string GetUpdateProfileQuery(IUpdateProfileParameters parameters);

        string GetUpdateProfileImageQuery(IUpdateProfileImageParameters parameters);
        string GetUpdateProfileBannerQuery(IUpdateProfileBannerParameters parameters);
        string GetRemoveProfileBannerQuery(IRemoveProfileBannerParameters parameters);
    }

    public class AccountSettingsQueryGenerator : IAccountSettingsQueryGenerator
    {
        public string GetAccountSettingsQuery(IGetAccountSettingsParameters parameters)
        {
            var query = new StringBuilder(Resources.Account_GetSettings);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetUpdateAccountSettingsQuery(IUpdateAccountSettingsParameters parameters)
        {
            var baseQuery = new StringBuilder(Resources.Account_UpdateSettings);

            var langParameterValue = parameters.DisplayLanguage == Language.Undefined ? null : parameters.DisplayLanguage?.GetLanguageCode();

            baseQuery.AddParameterToQuery("lang", langParameterValue);
            baseQuery.AddParameterToQuery("time_zone", parameters.TimeZone);
            baseQuery.AddParameterToQuery("sleep_time_enabled", parameters.SleepTimeEnabled);
            baseQuery.AddParameterToQuery("start_sleep_time", SleepHourToString(parameters.StartSleepHour));
            baseQuery.AddParameterToQuery("end_sleep_time", SleepHourToString(parameters.EndSleepHour));
            baseQuery.AddParameterToQuery("trend_location_woeid", parameters.TrendLocationWoeid);

            baseQuery.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return baseQuery.ToString();
        }

        public string GetUpdateProfileQuery(IUpdateProfileParameters parameters)
        {
            var query = new StringBuilder(Resources.Account_UpdateProfile);

            query.AddParameterToQuery("name", parameters.Name);
            query.AddParameterToQuery("url", parameters.WebsiteUrl);
            query.AddParameterToQuery("location", parameters.Location);
            query.AddParameterToQuery("description", parameters.Description);
            query.AddParameterToQuery("profile_link_color", parameters.ProfileLinkColor);
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddParameterToQuery("skip_status", parameters.SkipStatus);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        private static string SleepHourToString(int? hour)
        {
            if (hour == null)
            {
                return null;
            }

            if (hour >= 0 && hour < 10)
            {
                return $"0{hour}";
            }

            return hour.ToString();
        }

        public string GetUpdateProfileImageQuery(IUpdateProfileImageParameters parameters)
        {
            var query = new StringBuilder(Resources.Account_UpdateProfileImage);

            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddParameterToQuery("skip_status", parameters.SkipStatus);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetUpdateProfileBannerQuery(IUpdateProfileBannerParameters parameters)
        {
            var query = new StringBuilder(Resources.Account_UpdateProfileBanner);

            query.AddParameterToQuery("width", parameters.Width);
            query.AddParameterToQuery("height", parameters.Height);
            query.AddParameterToQuery("offset_left", parameters.OffsetLeft);
            query.AddParameterToQuery("offset_top", parameters.OffsetTop);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetRemoveProfileBannerQuery(IRemoveProfileBannerParameters parameters)
        {
            var query = new StringBuilder(Resources.Account_RemoveProfileBanner);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }
    }
}