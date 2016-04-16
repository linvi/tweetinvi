using System;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Core;
using Tweetinvi.WebLogic;

namespace Testinvi.TweetinviWebLogic
{
    [TestClass]
    public class WebHelperTests
    {
        private FakeClassBuilder<WebHelper> _fakeBuilder;
        private Fake<ITweetinviSettingsAccessor> _fakeTweetinviSettingsAccessor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<WebHelper>();
            _fakeTweetinviSettingsAccessor = _fakeBuilder.GetFake<ITweetinviSettingsAccessor>();
            _fakeTweetinviSettingsAccessor.CallsTo(x => x.HttpRequestTimeout).Returns(10000);
        }

        [TestMethod]
        public void GetResponseStream_UrlIsNull_ReturnsNull()
        {
            // Arrange
            var webHelper = CreateWebHelper();

            // Act
            var result = webHelper.GetResponseStream((string)null);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetResponseStream_UrlIsEmpty_ReturnsNull()
        {
            // Arrange
            var webHelper = CreateWebHelper();

            // Act
            var result = webHelper.GetResponseStream(String.Empty);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetResponseStream_UrlIsGoogle_ReturnsAStream()
        {
            // Arrange
            var webHelper = CreateWebHelper();

            // Act
            var result = webHelper.GetResponseStream("http://www.google.com");

            // Assert
            Assert.IsNotNull(result);
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

        public WebHelper CreateWebHelper()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}