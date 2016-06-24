using System.Globalization;

namespace Tweetinvi.Models
{
    public class TweetIdentifier : ITweetIdentifier
    {
        public TweetIdentifier(long tweetId)
        {
            Id = tweetId;
            IdStr = tweetId.ToString(CultureInfo.InvariantCulture);
        }

        public long Id { get; set; }
        public string IdStr { get; set; }
    }
}