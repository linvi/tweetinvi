using Xunit;
using Tweetinvi.Core.Extensions;

namespace Testinvi.NetCore
{
    public class CoreTestClass
    {   
        [Fact]
        public void TestLengthWith2Urls()
        {
            string test = "Hello http://tweetinvi.codeplex.com/salutLescopains 3615 Gerard www.linviIsMe.com piloupe";

            int twitterLength = StringExtension.TweetLength(test);

            Assert.Equal(twitterLength, 73);
        }

        [Fact]
        public void TestLengthWith2UrlsAndHttps()
        {
            string test = "Hello https://tweetinvi.codeplex.com/salutLescopains 3615 Gerard www.linviIsMe.com piloupe";

            int twitterLength = StringExtension.TweetLength(test);

            Assert.Equal (twitterLength, 73);
        }
    }
}
