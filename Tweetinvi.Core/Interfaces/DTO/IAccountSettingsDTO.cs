using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.DTO
{
    public interface IAccountSettingsDTO
    {
        string ScreenName { get; set; }
        PrivacyMode PrivacyMode { get; set; }
        Language Language { get; set; }

        bool AlwaysUseHttps { get; set; }
        bool DiscoverableByEmail { get; set; }
        bool DiscoverableByMobilePhone { get; set; }

        bool GeoEnabled { get; set; }
        bool ShowAllInlineMedia { get; set; }
        bool UseCookiePersonalization { get; set; }

        AllowDirectMessagesFrom AllowDirectMessagesFrom { get; set; }
        AllowDirectMessagesFrom AllowGroupDirectMessagesFrom { get; set; }
        AllowContributorRequestMode AllowContributorRequest { get; set; }

        ITimeZone TimeZone { get; set; }
        ITrendLocation[] TrendLocations { get; set; }

        bool SleepTimeEnabled { get; set; }
        int SleepTimeStartHour { get; set; }
        int SleepTimeEndHour { get; set; }
    }
}