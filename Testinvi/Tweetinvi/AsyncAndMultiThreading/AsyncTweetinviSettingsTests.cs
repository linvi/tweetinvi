using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tweetinvi;

namespace Testinvi.Tweetinvi.AsyncAndMultiThreading
{
    [TestClass]
    public class AsyncTweetinviSettingsTests
    {
        #region Async/Await

        [TestMethod]
        public async Task AsyncLocalSettingsAreCarriedOutToChild()
        {
            // Arrange
            Debug.WriteLine("Before 42");
            TweetinviConfig.CurrentThreadSettings.HttpRequestTimeout = 42;

            Debug.WriteLine("After 42");

            // Act
            await Sync.ExecuteTaskAsync(() =>
            {
                Debug.WriteLine("Inside async");
                // Assert
                Assert.AreEqual(TweetinviConfig.CurrentThreadSettings.HttpRequestTimeout, 42);
                Debug.WriteLine("Inside async 2");
            });
        }

        [TestMethod]
        public async Task AsyncLocalSettingsAreNotCarriedOutToParent()
        {
            // Arrange
            TweetinviConfig.CurrentThreadSettings.HttpRequestTimeout = 42;

            // Act
            Debug.WriteLine("BEFORE TASK ASTBC");
            await Sync.ExecuteTaskAsync(() =>
            {
                Debug.WriteLine("INSIDE BEFORE TASK ASTBC");
                TweetinviConfig.CurrentThreadSettings.HttpRequestTimeout = 43;

                // Assert
                Assert.AreEqual(TweetinviConfig.CurrentThreadSettings.HttpRequestTimeout, 43);
            });


            Debug.WriteLine("AFTER TASK ASTBC");
            Assert.AreEqual(TweetinviConfig.CurrentThreadSettings.HttpRequestTimeout, 42);
        }

        [TestMethod]
        public async Task ExecuteIsolatedTaskAsyncActionGetsOwnSettings()
        {
            // Act
            await Sync.ExecuteIsolatedTaskAsync(() =>
            {
                TweetinviConfig.CurrentThreadSettings.HttpRequestTimeout = 42;
            });

            // Assert
            Assert.AreEqual(TweetinviConfig.CurrentThreadSettings.HttpRequestTimeout, TweetinviConsts.DEFAULT_HTTP_REQUEST_TIMEOUT);
        }

        [TestMethod]
        public async Task ExecuteIsolatedTaskAsyncFuncGetsOwnSettings()
        {
            // Act
            await Sync.ExecuteIsolatedTaskAsync(() =>
            {
                TweetinviConfig.CurrentThreadSettings.HttpRequestTimeout = 42;
                return 0;
            });

            // Assert
            Assert.AreEqual(TweetinviConfig.CurrentThreadSettings.HttpRequestTimeout, TweetinviConsts.DEFAULT_HTTP_REQUEST_TIMEOUT);
        }

        #endregion

        #region MultiThreading

        [TestMethod]
        public void ThreadSettingsAreCarriedOutToChild()
        {
            // Arrange
            TweetinviConfig.CurrentThreadSettings.HttpRequestTimeout = 42;

            // Act
            Exception threadException = null;
            var thread = new Thread(() =>
            {
                try
                {
                    // Assert
                    Assert.AreEqual(TweetinviConfig.CurrentThreadSettings.HttpRequestTimeout, 42);
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
        public void ThreadSettingsAreNotCarriedOutToParent()
        {
            // Arrange
            TweetinviConfig.CurrentThreadSettings.HttpRequestTimeout = 42;

            // Act
            Exception threadException = null;
            var thread = new Thread(() =>
            {
                try
                {
                    TweetinviConfig.CurrentThreadSettings.HttpRequestTimeout = 43;
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

            Assert.AreEqual(TweetinviConfig.CurrentThreadSettings.HttpRequestTimeout, 42);
        }

        #endregion
    }
}
