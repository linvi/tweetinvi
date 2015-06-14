using System.Collections.Generic;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces.Parameters;

namespace Tweetinvi.Core.Parameters
{
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