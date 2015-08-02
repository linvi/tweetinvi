using System.Collections.Generic;
using Tweetinvi.Core.Enum;

namespace Tweetinvi.Core.Interfaces.Parameters
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
}