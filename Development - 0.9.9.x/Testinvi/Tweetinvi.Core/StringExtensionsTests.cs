using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tweetinvi;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Parameters;

namespace Testinvi.Tweetinvi.Core
{
    namespace Testinvi.Tweetinvi
    {
        [TestClass]
        public class StringExtensionTests
        {
            [TestMethod]
            public void TestLengthWith2Urls()
            {
                string test = "Hello http://tweetinvi.codeplex.com/salutLescopains 3615 Gerard www.linviIsMe.com piloupe";

                int twitterLength = StringExtension.TweetLength(test);

                Assert.AreEqual(twitterLength, 71);
            }

            [TestMethod]
            public void TestLengthWith2UrlsAndHttps()
            {
                string test = "Hello https://tweetinvi.codeplex.com/salutLescopains 3615 Gerard www.linviIsMe.com piloupe";

                int twitterLength = StringExtension.TweetLength(test);

                Assert.AreEqual(twitterLength, 72);
            }


            [TestMethod]
            public void TestLengthWithURLFollowedByDotAndSingleChar()
            {
                string test = "Hello https://tweetinvi.codeplex.com.a 3615 Gerard www.linviIsMe.com piloupe";

                int twitterLength = StringExtension.TweetLength(test);

                Assert.AreEqual(twitterLength, 74);
            }

            [TestMethod]
            public void TestLengthWithURLFollowedByDotAndTwoChars()
            {
                string test = "Hello https://tweetinvi.codeplex.com.au 3615 Gerard www.linviIsMe.com piloupe";

                int twitterLength = StringExtension.TweetLength(test);

                Assert.AreEqual(twitterLength, 72);
            }

            [TestMethod]
            public void TestLengthWithURLFollowedByArgsAndDot()
            {
                string test = "Hello https://tweetinvi.codeplex.com/salutLescopains.a 3615 Gerard www.linviIsMe.com piloupe";

                int twitterLength = StringExtension.TweetLength(test);

                Assert.AreEqual(twitterLength, 72);
            }

            [TestMethod]
            public void TestLengthWithSmallUrl()
            {
                string test = "www.co.co";

                int twitterLength = StringExtension.TweetLength(test);

                Assert.AreEqual(twitterLength, 22);
            }

            private void TestURLWithMultiplePrefix(string url, int expectedLength)
            {
                var basicTweetURL = String.Format("Hello there http:// {0} bye!", url);
                Assert.AreEqual(basicTweetURL.TweetLength(), expectedLength);

                var wwwTweetURL = String.Format("Hello there http:// www.{0} bye!", url);
                Assert.AreEqual(wwwTweetURL.TweetLength(), expectedLength);

                var httpTweetURL = String.Format("Hello there http:// http://{0} bye!", url);
                Assert.AreEqual(httpTweetURL.TweetLength(), expectedLength);

                var httpsTweetURL = String.Format("Hello there http:// https://{0} bye!", url);
                Assert.AreEqual(httpsTweetURL.TweetLength(), expectedLength + 1);

                var httpwwwTweetURL = String.Format("Hello there http:// http://{0} bye!", url);
                Assert.AreEqual(httpwwwTweetURL.TweetLength(), expectedLength);

                var httpswwwTweetURL = String.Format("Hello there http:// https://{0} bye!", url);
                Assert.AreEqual(httpswwwTweetURL.TweetLength(), expectedLength + 1);
            }

            [TestMethod]
            public void MultipleURLOfVariousFormat()
            {
                // Simple URL
                TestURLWithMultiplePrefix("co.com", 47);
                TestURLWithMultiplePrefix("tweetinvi.codeplex.com", 47);
                TestURLWithMultiplePrefix("tweetinvi.codeplex.com.a", 49);
                TestURLWithMultiplePrefix("tweetinvi.codeplex.com.au", 47);

                // Url with '/'
                TestURLWithMultiplePrefix("tweetinvi.codeplex.com/", 47);
                TestURLWithMultiplePrefix("tweetinvi.codeplex.com/a", 47);
                TestURLWithMultiplePrefix("tweetinvi.codeplex.com/salut", 47);
                TestURLWithMultiplePrefix("tweetinvi.codeplex.com/salut.a", 47);
                TestURLWithMultiplePrefix("tweetinvi.codeplex.com/salut.linvi", 47);
                TestURLWithMultiplePrefix("tweetinvi.codeplex.com/salut/linvi.a", 47);
                TestURLWithMultiplePrefix("tweetinvi.codeplex.com/salut/linvi.plop", 47);

                // Url finishing with '.'
                TestURLWithMultiplePrefix("tweetinvi.codeplex.com/.", 48);
                TestURLWithMultiplePrefix("tweetinvi.codeplex.com.", 48);
                TestURLWithMultiplePrefix("tweetinvi.codeplex.com/a.", 48);
                TestURLWithMultiplePrefix("tweetinvi.codeplex.com/salut.", 48);
                TestURLWithMultiplePrefix("tweetinvi.codeplex.com/salut.a.", 48);
                TestURLWithMultiplePrefix("tweetinvi.codeplex.com/salut.linvi.", 48);
                TestURLWithMultiplePrefix("tweetinvi.codeplex.com/salut/linvi.a.", 48);
                TestURLWithMultiplePrefix("tweetinvi.codeplex.com/salut/linvi.plop.", 48);

                // Url finishing with a special character
                TestURLWithMultiplePrefix("tweetinvi.codeplex.com/a!", 48);
                TestURLWithMultiplePrefix("tweetinvi.codeplex.com/salut!", 48);
                TestURLWithMultiplePrefix("tweetinvi.codeplex.com/salut.a!", 48);
                TestURLWithMultiplePrefix("tweetinvi.codeplex.com/salut.linvi!", 48);
                TestURLWithMultiplePrefix("tweetinvi.codeplex.com/salut/linvi.a!", 48);
                TestURLWithMultiplePrefix("tweetinvi.codeplex.com/salut/linvi.plop!", 48);
            }

            [TestMethod]
            public void URLWithOnly2CharsAtTheEnd()
            {
                string url = "salut.co";

                var basicTweetURL = String.Format("Hello there http:// {0} bye!", url);
                Assert.AreEqual(basicTweetURL.TweetLength(), 33);

                int expectedLength = 47;
                var wwwTweetURL = String.Format("Hello there http:// www.{0} bye!", url);
                Assert.AreEqual(wwwTweetURL.TweetLength(), expectedLength);

                var httpTweetURL = String.Format("Hello there http:// http://{0} bye!", url);
                Assert.AreEqual(httpTweetURL.TweetLength(), expectedLength);

                var httpsTweetURL = String.Format("Hello there http:// https://{0} bye!", url);
                Assert.AreEqual(httpsTweetURL.TweetLength(), expectedLength + 1);

                var httpwwwTweetURL = String.Format("Hello there http:// http://{0} bye!", url);
                Assert.AreEqual(httpwwwTweetURL.TweetLength(), expectedLength);

                var httpswwwTweetURL = String.Format("Hello there http:// https://{0} bye!", url);
                Assert.AreEqual(httpswwwTweetURL.TweetLength(), expectedLength + 1);
            }

            [TestMethod]
            public void URLWithOnly2CharsAtTheEnd_ButWithASlashCharacter()
            {
                string url = "NOW-FREE/4 Parties! Live Shows/Music/Art Walk Weekend. https://pbsc.co/eg/4b MAP, & interactive for every Smart/iphone: goo.gl/.";
                Assert.AreEqual(url.TweetLength(), 145);

                string url2 = "NOW-FREE/4 Parties! Live Shows/Music/Art Walk Weekend. https://pbsc.co/eg/4b MAP, & interactive for every Smart/iphone: goo.gl/dqkd.";
                Assert.AreEqual(url2.TweetLength(), 145);
            }

            [TestMethod]
            public void URLWithEqualsCharacters()
            {
                string message = "The quick brown fox jumps over the lazy dog. My dog is freaking amazing. https://www.google.nl/search?q=dog&source=lnms&tbm=isch&sa=X&ei=IZ7fU-CwJIO50QWtmICoCA&ved=0CAgQ_AUoAQ&biw=1528&bih=876";
                Assert.AreEqual(message.TweetLength(), 96);
            }

            [TestMethod]
            public void TweetWithURLAndMedia_URLHasNoSpaceBeforeIt()
            {
                string text = "abcdefghijklmnopqrstuvwxy abcdefghijklmnopqrstuvwxy abcdefghijklmnopqrstuvwxy abcdefghijklmophttp://bit.ly/tinyurlwiki";
                Assert.AreEqual(Tweet.Length(text), 118);
                Assert.AreEqual(Tweet.Length(text, new PublishTweetOptionalParameters()
                {
                    MediaBinaries = new List<byte[]> { new byte[10] }
                }), 141);

            }

            [TestMethod]
            public void TweetWithURLAndMedia()
            {
                string text = "abcdefghijklmnopqrstuvwxy abcdefghijklmnopqrstuvwxy abcdefghijklmnopqrstuvwxy abcdefghijklmop http://bit.ly/tinyurlwiki";
                Assert.AreEqual(Tweet.Length(text), 116);
                Assert.AreEqual(Tweet.Length(text, new PublishTweetOptionalParameters()
                {
                    MediaBinaries = new List<byte[]> { new byte[10] }
                }), 139);

            }
        }
    }

}
