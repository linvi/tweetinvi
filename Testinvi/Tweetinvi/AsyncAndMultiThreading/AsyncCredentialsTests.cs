using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tweetinvi;
using Tweetinvi.Models;

namespace Testinvi.Tweetinvi.AsyncAndMultiThreading
{
    [TestClass]
    public class AsyncCredentialsTests
    {
        #region Async/Await

        [TestMethod]
        public async Task AsyncLocalCredsAreCarriedOutToChild()
        {
            // Arrange
            var creds = new TwitterCredentials();
            Auth.SetCredentials(creds);

            // Act
            await Sync.ExecuteTaskAsync(() =>
            {
                // Assert
                Assert.AreEqual(Auth.Credentials, creds);
            });
        }

        [TestMethod]
        public async Task AsyncLocalCredsAreNotCarriedOutToParent()
        {
            // Arrange
            var parentCreds = new TwitterCredentials();
            var childCreds = new TwitterCredentials();

            Auth.SetCredentials(parentCreds);

            // Act
            await Sync.ExecuteTaskAsync(() =>
            {
                Auth.SetCredentials(childCreds);

                // Assert
                Assert.AreEqual(Auth.Credentials, childCreds);
            });

            Assert.AreNotEqual(Auth.Credentials, childCreds);
        }

        [TestMethod]
        public async Task ExecuteIsolatedTaskAsyncActionGetsOwnExceptionHandler()
        {
            var credentials = new TwitterCredentials();

            // Act: Ensure we have an Exception Handler on the calling context
            Sync.PrepareForAsync();

            await Sync.ExecuteIsolatedTaskAsync(() =>
            {
                // Set the credentials from the parent ExecutionContext
                Auth.SetCredentials(credentials);
            });

            // Assert
            Assert.IsNull(Auth.Credentials);
        }

        [TestMethod]
        public async Task ExecuteIsolatedTaskAsyncFuncGetsOwnCredentials()
        {
            var credentials = new TwitterCredentials();

            // Arrange: Ensure we have an Exception Handler on the calling context
            Sync.PrepareForAsync();

            // Act: Use the Exception Handler within ExecuteIsolatedTaskAsync
            await Sync.ExecuteIsolatedTaskAsync(() =>
            {
                Auth.SetCredentials(credentials);
                return 0;
            });

            // Assert
            Assert.IsNull(Auth.Credentials);
        }
	   
        #endregion

        #region MultiThreading

        [TestMethod]
        public void ThreadCredsAreCarriedOutToChild()
        {
            // Arrange
            var creds = new TwitterCredentials();
            Auth.SetCredentials(creds);

            // Act
            Exception threadException = null;
            var thread = new Thread(() =>
            {
                try
                {
                    // Assert
                    Assert.AreEqual(Auth.Credentials, creds);
                }
                catch (Exception e)
                {
                    threadException = e;
                }
            });

            thread.Start();
            thread.Join();

            if (threadException != null)
            {
                throw threadException;
            }
        }

        [TestMethod]
        public void ThreadCredsAreNotCarriedOutToParent()
        {
            // Arrange
            var parentCreds = new TwitterCredentials();
            var childCreds = new TwitterCredentials();

            Auth.SetCredentials(parentCreds);

            // Act
            Exception threadException = null;
            var thread = new Thread(() =>
            {
                try
                {
                    Auth.SetCredentials(childCreds);


                    // Assert
                    Assert.AreEqual(Auth.Credentials, childCreds);
                }
                catch (Exception e)
                {
                    threadException = e;
                }
            });

            thread.Start();
            thread.Join();

            if (threadException != null)
            {
                throw threadException;
            }

            Assert.AreEqual(Auth.Credentials, parentCreds);
        }

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

        private void AssertAreCredentialsEquals(ITwitterCredentials credentials1, ITwitterCredentials credentials2)
        {
            Assert.AreEqual(credentials1.AccessToken, credentials2.AccessToken);
            Assert.AreEqual(credentials1.AccessTokenSecret, credentials2.AccessTokenSecret);
            Assert.AreEqual(credentials1.ConsumerKey, credentials2.ConsumerKey);
            Assert.AreEqual(credentials1.ConsumerSecret, credentials2.ConsumerSecret);
        }

        #endregion
    }
}
