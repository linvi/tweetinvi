using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Logic;

namespace Testinvi.Tweetinvi.Logic
{
    [TestClass]
    public class TweetTests
    {
        private FakeClassBuilder<Tweet> _fakeBuilder;


        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<Tweet>();
        }


        [TestMethod]
        public void TweetLength_Returns_StringExtensionValue()
        {
            // Arrange
            var tweet = CreateTweet();
            tweet.Text = "salut";

            // Act 
            var length = tweet.Length;

            // Assert
            Assert.AreEqual(length, 5);
        }

        [TestMethod]
        public void TweetLengthWith1Document_MediaSizeIs23_ReturnsMediaPlusTextSize()
        {
            // Arrange
            var tweet = CreateTweet();
            tweet.Text = "salut";
            tweet.AddMedia(new byte[10]);

            // Act 
            var length = tweet.Length;

            // Assert
            Assert.AreEqual(length, 28);
        }

        [TestMethod]
        public void TweetLengthWith2Documents_MediaSizeIs23_ReturnsMediaPlusTextSize()
        {
            // Arrange
            var tweet = CreateTweet();
            tweet.Text = "salut";
            tweet.AddMedia(new byte[10]);
            tweet.AddMedia(new byte[10]);

            // Act 
            var length = tweet.Length;

            // Assert
            Assert.AreEqual(length, 28);
        }

        public Tweet CreateTweet()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}
