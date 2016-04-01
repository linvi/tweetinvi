using System.Configuration;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tweetinvi;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Testinvi.Tweetinvi
{
    [TestClass]
    public class AuthTests
    {
        [TestMethod]
        public void ThreadCredentialsSetCorrectly()
        {
            // Arrange
            var credentials1 = Auth.CreateCredentials(
                ConfigurationManager.AppSettings["token_AccessToken"],
                ConfigurationManager.AppSettings["token_AccessTokenSecret"],
                ConfigurationManager.AppSettings["token_ConsumerKey"],
                ConfigurationManager.AppSettings["token_ConsumerSecret"]);

            var credentials2 = Auth.CreateCredentials(
                ConfigurationManager.AppSettings["token2_AccessToken"],
                ConfigurationManager.AppSettings["token2_AccessTokenSecret"],
                ConfigurationManager.AppSettings["token2_ConsumerKey"],
                ConfigurationManager.AppSettings["token2_ConsumerSecret"]);

            bool credentials2Set = false;
            bool thread1Initialized = false;

            // Act
            Auth.Credentials = credentials1;
            AssertAreCredentialsEquals(Auth.Credentials, credentials1);

            var thread = new Thread(() =>
            {
                AssertAreCredentialsEquals(Auth.Credentials, credentials1);
                thread1Initialized = true;

                // ReSharper disable once AccessToModifiedClosure
                while (!credentials2Set)
                {
                    Thread.Sleep(5);
                }

                AssertAreCredentialsEquals(Auth.Credentials, credentials1);
                Auth.Credentials = credentials2;
                AssertAreCredentialsEquals(Auth.Credentials, credentials2);
            });

            thread.Start();

            while (!thread1Initialized)
            {
                Thread.Sleep(5);
            }

            Auth.Credentials = credentials2;
            AssertAreCredentialsEquals(Auth.Credentials, credentials2);

            Thread t2 = new Thread(() =>
            {
                AssertAreCredentialsEquals(Auth.Credentials, credentials2);
                Auth.Credentials = credentials1;
                AssertAreCredentialsEquals(Auth.Credentials, credentials1);
            });

            t2.Start();
            t2.Join();

            credentials2Set = true;

            thread.Join();

            AssertAreCredentialsEquals(Auth.Credentials, credentials1);
        }

        [TestMethod]
        public void ApplicationCredentialsDefinedOnFirstSet()
        {
            // Arrange
            var credentials = GenerateTokenCredentials();

            // Act
            Auth.Credentials = credentials;

            // Assert
            Assert.AreEqual(credentials, Auth.Credentials);
        }

        private ITwitterCredentials GenerateTokenCredentials()
        {
            return null;
        }

        private void AssertAreCredentialsEquals(ITwitterCredentials credentials1, ITwitterCredentials credentials2)
        {
            Assert.AreEqual(credentials1.AccessToken, credentials2.AccessToken);
            Assert.AreEqual(credentials1.AccessTokenSecret, credentials2.AccessTokenSecret);
            Assert.AreEqual(credentials1.ConsumerKey, credentials2.ConsumerKey);
            Assert.AreEqual(credentials1.ConsumerSecret, credentials2.ConsumerSecret);
        }
    }
}