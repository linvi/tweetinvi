using System.Configuration;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tweetinvi;
using Tweetinvi.Models;

namespace Testinvi.Tweetinvi
{
    [TestClass]
    public class AuthTests
    {
        [TestMethod]
        public void ThreadCredentialsSetCorrectly()
        {
            // Arrange
            var credentials1 = Auth.CreateCredentials("a", "a", "a", "a");
            var credentials2 = Auth.CreateCredentials("b", "b", "b", "b");

            bool credentials2Set = false;
            bool thread1Initialized = false;

            // Act
            Auth.Credentials = credentials1; // Very first set thread credentials will also set application-wide, which is the default for any new thread
            AssertAreCredentialsEquals(Auth.Credentials, credentials1);

            var thread = new Thread(() =>
            {
                // New thread should get the application-wide credentials
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
                // New thread should get the application-wide credentials
                AssertAreCredentialsEquals(Auth.Credentials, credentials1);

                Auth.Credentials = credentials2;
                AssertAreCredentialsEquals(Auth.Credentials, credentials2);
            });

            t2.Start();
            t2.Join();

            credentials2Set = true;

            thread.Join();

            AssertAreCredentialsEquals(Auth.Credentials, credentials2);
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