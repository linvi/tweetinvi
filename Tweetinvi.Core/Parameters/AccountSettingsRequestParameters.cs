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
        List<Language> Languages { get; set; }

        /// <summary>
        /// Must be a Timezone from http://api.rubyonrails.org/classes/ActiveSupport/TimeZone.html
        /// </summary>
        string TimeZone { get; set; }

        long? TrendLocationWoeid { get; set; }
        bool? SleepTimeEnabled { get; set; }
        int? StartSleepTime { get; set; }
        int? EndSleepTime { get; set; }

        // REMOVED AS DESCRIPTION SAYS IT REQUIRES THE PASSWORD TO BE ADDED
        // Whether to allow others to include user as contributor. Possible values include “all” (anyone can include user), “following” (only followers can include user) or “none”.
        // Also note that changes to this field require the request also include a “current_password” value with the user’s password to successfully modify this field.
        // AllowContributorRequestMode? AllowContributorRequest { get; set; }

        void AddLanguage(Language language);
        void RemoveLanguage(Language language);
        void SetTimeZone(TwitterTimeZone twitterTimeZone);
    }

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