using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi.Core.Interfaces.Models
{
    public interface IAccountSettings
    {
        /// <summary>
        /// Account settings backend properties.
        /// </summary>
        IAccountSettingsDTO AccountSettingsDTO { get; set; }

        /// <summary>
        /// User screen name (used for authentication)
        /// </summary>
        string ScreenName { get; }

        /// <summary>
        /// Privacy mode used on the account.
        /// </summary>
        PrivacyMode PrivacyMode { get; }

        /// <summary>
        /// Languages used by the user.
        /// </summary>
        Language Language { get; }
        
        /// <summary>
        /// Enforce https mode.
        /// </summary>
        bool AlwaysUseHttps { get; }

        /// <summary>
        /// Can this user be found by email address.
        /// </summary>
        bool DiscoverableByEmail { get; }

        /// <summary>
        /// Can this user be found by phone number.
        /// </summary>
        bool DiscoverableByMobilePhone { get; }

        /// <summary>
        /// Can the tweets published by the user be geo tagged.
        /// </summary>
        bool GeoEnabled { get; }
        bool ShowAllInlineMedia { get; }
        bool UseCookiePersonalization { get; }

        /// <summary>
        /// Specify who can send you private messages.
        /// </summary>
        AllowDirectMessagesFrom AllowDirectMessagesFrom { get; }

        /// <summary>
        /// Specify which groups can send you private messages.
        /// </summary>
        AllowDirectMessagesFrom AllowGroupDirectMessagesFrom { get; }

        /// <summary>
        /// Specify who can ask you to be a contributor.
        /// </summary>
        AllowContributorRequestMode AllowContributorRequest { get; }

        /// <summary>
        /// Primary timezone of the account.
        /// </summary>
        ITimeZone TimeZone { get; }

        /// <summary>
        /// Locations used to display trends.
        /// </summary>
        ITrendLocation[] TrendLocations { get; }

        /// <summary>
        /// Specify if you want the notifications to be disabled during the sleeping hours.
        /// </summary>
        bool SleepTimeEnabled { get; }

        /// <summary>
        /// Specify the time after which you do not want to receive any notification.
        /// </summary>
        int SleepTimeStartHour { get; }

        /// <summary>
        /// Specify the time after which you do want to receive notifications.
        /// </summary>
        int SleepTimeEndHour { get; }
    }
}