using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.Models
{
    public class AccountSettings : IAccountSettings
    {
        private IAccountSettingsDTO _accountSettingsDTO;

        public AccountSettings(IAccountSettingsDTO accountSettingsDTO)
        {
            _accountSettingsDTO = accountSettingsDTO;
        }

        public IAccountSettingsDTO AccountSettingsDTO
        {
            get => _accountSettingsDTO;
            set => _accountSettingsDTO = value;
        }

        public string ScreenName => _accountSettingsDTO.ScreenName;
        public PrivacyMode PrivacyMode => _accountSettingsDTO.PrivacyMode;
        public Language Language => _accountSettingsDTO.Language;
        public bool AlwaysUseHttps => _accountSettingsDTO.AlwaysUseHttps;
        public bool DiscoverableByEmail => _accountSettingsDTO.DiscoverableByEmail;
        public bool DiscoverableByMobilePhone => _accountSettingsDTO.DiscoverableByMobilePhone;
        public bool GeoEnabled => _accountSettingsDTO.GeoEnabled;
        public bool UseCookiePersonalization => _accountSettingsDTO.UseCookiePersonalization;
        public AllowDirectMessagesFrom AllowDirectMessagesFrom => _accountSettingsDTO.AllowDirectMessagesFrom;
        public AllowDirectMessagesFrom AllowGroupDirectMessagesFrom => _accountSettingsDTO.AllowGroupDirectMessagesFrom;
        public AllowContributorRequestMode AllowContributorRequest => _accountSettingsDTO.AllowContributorRequest;
        public bool DisplaySensitiveMedia => _accountSettingsDTO.DisplaySensitiveMedia;
        public bool SmartMute => _accountSettingsDTO.SmartMute;
        public ITimeZone TimeZone => _accountSettingsDTO.TimeZone;
        public bool SleepTimeEnabled => _accountSettingsDTO.SleepTimeEnabled;
        public int StartSleepHour => _accountSettingsDTO.SleepTimeStartHour;
        public int EndSleepHour => _accountSettingsDTO.SleepTimeEndHour;
        public string TranslatorType => _accountSettingsDTO.TranslatorType;
        public ITrendLocation[] TrendLocations => _accountSettingsDTO.TrendLocations;
    }
}