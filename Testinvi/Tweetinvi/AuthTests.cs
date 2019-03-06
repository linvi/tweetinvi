using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tweetinvi;
using Tweetinvi.Models;

namespace Testinvi.Tweetinvi
{
    [TestClass]
    public class AuthTests
    {
        [TestMethod]
        public void ApplicationCredentialsDefinedOnFirstSet()
        {
            // Arrange
            var credentials = A.Fake<ITwitterCredentials>();

            // Act
            Auth.Credentials = credentials;

            // Assert
            Assert.AreEqual(credentials, Auth.Credentials);
        }
    }
}