using System.Collections.Generic;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Extensions;

namespace Tweetinvi.Core.Parameters
{
    /// <summary>
    /// For more description visit : https://dev.twitter.com/rest/reference/post/account/settings
    /// </summary>
    public interface IAccountSettingsRequestParameters : ICustomRequestParameters
    {
        /// <summary>
        /// The languages which Twitter should use for this user. 
        /// </summary>
        List<Language> Languages { get; set; }

        /// <summary>
        /// Must be a Timezone from http://api.rubyonrails.org/classes/ActiveSupport/TimeZone.html
        /// </summary>
        string TimeZone { get; set; }

        /// <summary>
        /// The Yahoo! Where On Earth ID to use as the user’s default trend location. 
        /// Global information is available by using 1 as the WOEID. 
        /// </summary>
        long? TrendLocationWoeid { get; set; }

        /// <summary>
        /// When enabled sleep time is the time when push or SMS notifications should not be sent to the user.
        /// </summary>
        bool? SleepTimeEnabled { get; set; }

        /// <summary>
        /// The hour that sleep time should begin if it is enabled. 
        /// The value for this parameter should be provided in ISO8601 format (i.e. 00-23). 
        /// The time is considered to be in the same timezone as the user’s time_zone setting.
        /// </summary>
        int? StartSleepTime { get; set; }

        /// <summary>
        /// The hour that sleep time should end if it is enabled.
        /// The value for this parameter should be provided in ISO8601 format (i.e. 00-23). 
        /// The time is considered to be in the same timezone as the user’s time_zone setting.
        /// </summary>
        int? EndSleepTime { get; set; }

        // REMOVED AS DESCRIPTION SAYS IT REQUIRES THE PASSWORD TO BE ADDED
        // Whether to allow others to include user as contributor. Possible values include :
        // “all” (anyone can include user), 
        // “following” (only followers can include user) or “none”.
        // Also note that changes to this field require the request also include a 
        // “current_password” value with the user’s password to successfully modify this field.
        // AllowContributorRequestMode? AllowContributorRequest { get; set; }

        /// <summary>
        /// Add a language that Twitter will use for this user.
        /// </summary>
        void AddLanguage(Language language);

        /// <summary>
        /// Remove a language that Twitter will use for this user.
        /// </summary>
        void RemoveLanguage(Language language);

        /// <summary>
        /// The timezone dates and times should be displayed in for the user. 
        /// The timezone must be one of the Rails TimeZone names.
        /// </summary>
        void SetTimeZone(TwitterTimeZone twitterTimeZone);
    }

    /// <summary>
    /// For more description visit : https://dev.twitter.com/rest/reference/post/account/settings
    /// </summary>
    public class AccountSettingsRequestParameters : CustomRequestParameters, IAccountSettingsRequestParameters
    {
        public AccountSettingsRequestParameters()
        {
            Languages = new List<Language>();
        }

        public List<Language> Languages { get; set; }
        public string TimeZone { get; set; }
        public long? TrendLocationWoeid { get; set; }
        public bool? SleepTimeEnabled { get; set; }
        public int? StartSleepTime { get; set; }
        public int? EndSleepTime { get; set; }

        public void AddLanguage(Language language)
        {
            if (Languages == null)
            {
                Languages = new List<Language>();
            }

            if (!Languages.Contains(language))
            {
                Languages.Add(language);
            }
        }

        public void RemoveLanguage(Language language)
        {
            if (Languages == null)
            {
                Languages = new List<Language>();
            }

            if (Languages.Contains(language))
            {
                Languages.Remove(language);
            }
        }

        public void SetTimeZone(TwitterTimeZone twitterTimeZone)
        {
            var tzinfo = twitterTimeZone.GetTZinfo();
            if (tzinfo != null)
            {
                TimeZone = tzinfo;
            }
        }
    }
}