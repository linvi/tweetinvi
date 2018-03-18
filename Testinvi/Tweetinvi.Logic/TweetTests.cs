using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi;
using Tweetinvi.Core.Injectinvi;
using Tweet = Tweetinvi.Logic.Tweet;

namespace Testinvi.Tweetinvi.Logic
{
    [TestClass]
    public class TweetTests
    {
        private FakeClassBuilder<Tweet> _fakeBuilder;


        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<Tweet>("tweetMode");
        }


        [TestMethod]
        [Ignore]
        public void TweetLength_Returns_StringExtensionValue()
        {
            // Arrange
            var tweet = CreateTweet();
            tweet.Text = "salut";

            // Act 
            //var length = tweet.CalculateLength(false);

            // Assert
            //Assert.AreEqual(length, 5);
        }

        [TestMethod]
        [Ignore]
        public void TweetLengthWith1Document_MediaSizeIs23_ReturnsMediaPlusTextSize()
        {
            // Arrange
            var tweet = CreateTweet();
            tweet.Text = "salut";

            // Act 
            //var length = tweet.CalculateLength(true);

            // Assert
            //Assert.AreEqual(length, 29);
        }

        public Tweet CreateTweet()
        {
            return _fakeBuilder.GenerateClass(new ConstructorNamedParameter("tweetMode", TweetMode.Extended));
        }
    }
}
