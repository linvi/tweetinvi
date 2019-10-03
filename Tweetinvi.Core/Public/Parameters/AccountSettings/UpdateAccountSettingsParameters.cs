using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more description visit : https://dev.twitter.com/en/docs/accounts-and-users/manage-account-settings/api-reference/post-account-settings
    /// </summary>
    public interface IUpdateAccountSettingsParameters : ICustomRequestParameters
    {
        /// <summary>
        /// The languages which Twitter should use for this user.
        /// <para>PLEASE MAKE SURE this is a Language from the DisplayLanguages list</para>
        /// <para>You can use a Language.IsADisplayLanguage() extension function to ensure it is supported</para>
        /// <remarks>Tweetinvi will not prevent this parameter to set to any Language for future support</remarks>
        /// </summary>
        Language? DisplayLanguage { get; set; }

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
        int? StartSleepHour { get; set; }

        /// <summary>
        /// The hour that sleep time should end if it is enabled.
        /// The value for this parameter should be provided in ISO8601 format (i.e. 00-23). 
        /// The time is considered to be in the same timezone as the user’s time_zone setting.
        /// </summary>
        int? EndSleepHour { get; set; }

        /// <summary>
        /// The timezone dates and times should be displayed in for the user. 
        /// The timezone must be one of the Rails TimeZone names.
        /// </summary>
        void SetTimeZone(TimeZoneFromTwitter timeZoneFromTwitter);
    }

    
    /// <inheritdoc/>
    public class UpdateAccountSettingsParameters : CustomRequestParameters, IUpdateAccountSettingsParameters
    {
        /// <inheritdoc/>
        public Language? DisplayLanguage { get; set; }
        /// <inheritdoc/>
        public string TimeZone { get; set; }
        /// <inheritdoc/>
        public long? TrendLocationWoeid { get; set; }
        /// <inheritdoc/>
        public bool? SleepTimeEnabled { get; set; }
        /// <inheritdoc/>
        public int? StartSleepHour { get; set; }
        /// <inheritdoc/>
        public int? EndSleepHour { get; set; }
        /// <inheritdoc/>
        public void SetTimeZone(TimeZoneFromTwitter timeZoneFromTwitter)
        {
            var tzinfo = timeZoneFromTwitter.GetTZinfo();
            if (tzinfo != null)
            {
                TimeZone = tzinfo;
            }
        }
    }
}