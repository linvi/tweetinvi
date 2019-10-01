using System;
using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.AccountSettings
{
    public interface IAccountSettingsQueryGenerator
    {
        string GetUpdateProfileImageQuery(IUpdateProfileImageParameters parameters);
        string GetUpdateProfileBannerQuery(IUpdateProfileBannerParameters parameters);
    }

    public class AccountSettingsQueryGenerator : IAccountSettingsQueryGenerator
    {
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
    }
}