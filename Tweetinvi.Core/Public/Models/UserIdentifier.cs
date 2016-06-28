using System.Globalization;

namespace Tweetinvi.Models
{
    public class UserIdentifier : IUserIdentifier
    {
        private UserIdentifier()
        {
            Id = TweetinviSettings.DEFAULT_ID;
        }

        public UserIdentifier(long userId) : this()
        {
            Id = userId;
            IdStr = userId.ToString(CultureInfo.InvariantCulture);
        }

        public UserIdentifier(string userScreenName) : this()
        {
            ScreenName = userScreenName;
        }

        public long Id { get; set; }
        public string IdStr { get; set; }
        public string ScreenName { get; set; }
    }
}