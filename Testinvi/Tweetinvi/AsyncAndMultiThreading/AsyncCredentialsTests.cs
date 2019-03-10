using System;
using System.Configuration;
using System.Diagnostics;
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
            var creds = new TwitterCredentials("a", "a", "a", "a");
            Auth.SetCredentials(creds);

            // Act
            await Sync.ExecuteTaskAsync(() =>
            {
                // Assert
                Assert.AreNotEqual(Auth.Credentials, creds);
                AssertAreCredentialsEquals(Auth.Credentials, creds);
            });
        }

        [TestMethod]
        public async Task AsyncLocalCredsAreNotCarriedOutToParent()
        {
            await AsyncLocalCredsAreNotCarriedOutToParentRecursively(0);
        }

        [TestMethod]
        public async Task AsyncLocalCredsAreNotCarriedOutToParentMultipleLevels()
        {
            await AsyncLocalCredsAreNotCarriedOutToParentRecursively(5);
        }


        public async Task AsyncLocalCredsAreNotCarriedOutToParentRecursively(int i)
        {
            // Arrange
            var parentCreds = new TwitterCredentials($"parent{i}", $"parent{i}", $"parent{i}", $"parent{i}");
            var childCreds = new TwitterCredentials($"child{i}", $"child{i}", $"child{i}", $"child{i}");
            
            Debug.WriteLine($"[{i}] SetCredentials");
            Auth.SetCredentials(parentCreds);

            // Act
            Debug.WriteLine($"[{i}] Task Async");
            await Sync.ExecuteTaskAsync(() =>
            {
                Assert.AreNotEqual(Auth.Credentials, parentCreds);
                AssertAreCredentialsEquals(Auth.Credentials, parentCreds);

                Debug.WriteLine($"[{i}] SetCredentials child");
                Auth.SetCredentials(childCreds);

                // Assert
                Debug.WriteLine($"[{i}] Before accessing child creds");
                Assert.AreEqual(Auth.Credentials, childCreds);
                Debug.WriteLine($"[{i}] After accessing child creds");
                AssertAreCredentialsEquals(Auth.Credentials, childCreds);

                Assert.AreEqual(Auth.Credentials, childCreds);
            });

            Assert.AreEqual(Auth.Credentials, parentCreds);

            if (i > 0)
            {
                await AsyncLocalCredsAreNotCarriedOutToParentRecursively(i - 1);
            }
        }


        [TestMethod]
        public async Task ExecuteIsolatedTaskAsyncActionGetsOwnCredentials()
        {
            var credentials = new TwitterCredentials("a", "a", "a", "a");

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
            var credentials = new TwitterCredentials("a", "a", "a", "a");

            // Act
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
            var creds = new TwitterCredentials("a", "a", "a", "a");
            Auth.SetCredentials(creds);

            // Act
            Exception threadException = null;
            var thread = new Thread(() =>
            {
                try
                {
                    // Assert
                    Assert.AreEqual(Auth.Credentials, creds);
                    AssertAreCredentialsEquals(Auth.Credentials, creds);
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
            var parentCreds = new TwitterCredentials("a", "a", "a", "a");
            var childCreds = new TwitterCredentials("b", "b", "a", "b");

            Auth.SetCredentials(parentCreds);

            // Act
            Exception threadException = null;
            var thread = new Thread(() =>
            {
                try
                {
                    Assert.AreEqual(Auth.Credentials, parentCreds);

                    Auth.SetCredentials(childCreds);


                    // Assert
                    Assert.AreEqual(Auth.Credentials, childCreds);
                    AssertAreCredentialsEquals(Auth.Credentials, childCreds);
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

        #endregion

         private void AssertAreCredentialsEquals(ITwitterCredentials credentials1, ITwitterCredentials credentials2)
        {
            Assert.AreEqual(credentials1.AccessToken, credentials2.AccessToken);
            Assert.AreEqual(credentials1.AccessTokenSecret, credentials2.AccessTokenSecret);
            Assert.AreEqual(credentials1.ConsumerKey, credentials2.ConsumerKey);
            Assert.AreEqual(credentials1.ConsumerSecret, credentials2.ConsumerSecret);
        }
    }
}
