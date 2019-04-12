using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Logic.Model
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
            get { return _accountSettingsDTO; } 
            set { _accountSettingsDTO = value; }
        }

        public string ScreenName
        {
            get { return _accountSettingsDTO.ScreenName; }
        }

        public PrivacyMode PrivacyMode
        {
            get { return _accountSettingsDTO.PrivacyMode; }
        }

        public Language Language
        {
            get { return _accountSettingsDTO.Language; }
        }

        public bool AlwaysUseHttps
        {
            get { return _accountSettingsDTO.AlwaysUseHttps; }
        }

        public bool DiscoverableByEmail
        {
            get { return _accountSettingsDTO.DiscoverableByEmail; }
        }

        public bool DiscoverableByMobilePhone
        {
            get { return _accountSettingsDTO.DiscoverableByMobilePhone; }
        }

        public bool GeoEnabled
        {
            get { return _accountSettingsDTO.GeoEnabled; }
        }

        public bool UseCookiePersonalization
        {
            get { return _accountSettingsDTO.UseCookiePersonalization; }
        }

        public AllowDirectMessagesFrom AllowDirectMessagesFrom
        {
            get { return _accountSettingsDTO.AllowDirectMessagesFrom; }
        }

        public AllowDirectMessagesFrom AllowGroupDirectMessagesFrom
        {
            get { return _accountSettingsDTO.AllowGroupDirectMessagesFrom; }
        }

        public AllowContributorRequestMode AllowContributorRequest
        {
            get { return _accountSettingsDTO.AllowContributorRequest; }
        }

        public bool DisplaySensitiveMedia
        {
            get { return _accountSettingsDTO.DisplaySensitiveMedia; }
        }

        public bool SmartMute
        {
            get { return _accountSettingsDTO.SmartMute; }
        }

        public ITimeZone TimeZone
        {
            get { return _accountSettingsDTO.TimeZone; }
        }

        public bool SleepTimeEnabled
        {
            get { return _accountSettingsDTO.SleepTimeEnabled; }
        }

        public int SleepTimeStartHour
        {
            get { return _accountSettingsDTO.SleepTimeStartHour; }
        }

        public int SleepTimeEndHour
        {
            get { return _accountSettingsDTO.SleepTimeEndHour; }
        }
    }
}