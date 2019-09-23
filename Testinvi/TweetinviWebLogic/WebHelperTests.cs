using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.WebLogic;

namespace Testinvi.TweetinviWebLogic
{
    [TestClass]
    public class WebHelperTests
    {
        private FakeClassBuilder<WebHelper> _fakeBuilder;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<WebHelper>();
        }

        [TestMethod]
        public void GetBaseURL_URLIsNull_ReturnsNull()
        {
            // Arrange
            var webHelper = CreateWebHelper();

            // Act
            var result = webHelper.GetBaseURL((string)null);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetBaseURL_InvalidURLFormat_ReturnsNull()
        {
            // Arrange
            var webHelper = CreateWebHelper();

            // Act
            var result = webHelper.GetBaseURL("THIS IS NOT A URL!");

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetBaseURL_URLIsValid_ReturnsBaseURL()
        {
            // Arrange
            var webHelper = CreateWebHelper();

            // Act
            var result = webHelper.GetBaseURL("https://www.google.com/plop?salut=24&j=2");

            // Assert
            Assert.AreEqual(result, "https://www.google.com/plop");
        }

        private WebHelper CreateWebHelper()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}