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
            var length = tweet.CalculateLength(false);

            // Assert
            Assert.AreEqual(length, 5);
        }

        [TestMethod]
        public void TweetLengthWith1Document_MediaSizeIs23_ReturnsMediaPlusTextSize()
        {
            // Arrange
            var tweet = CreateTweet();
            tweet.Text = "salut";

            // Act 
            var length = tweet.CalculateLength(true);

            // Assert
            Assert.AreEqual(length, 29);
        }

        public Tweet CreateTweet()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}
