using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.TestObjects;
using Tweetinvi;
using Tweetinvi.Core.Exceptions;

namespace Testinvi.Tweetinvi
{
    [TestClass]
    public class SyncTests
    {
        #region Task ExecuteTaskAsync(Action action)

        [TestMethod]
        public async Task ExecuteTaskAsyncActionCallsAction()
        {
            // Arrange
            bool called = false;

            // Act
            await Sync.ExecuteTaskAsync(() => { called = true; });

            // Assert
            Assert.IsTrue(called);
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public async Task ExecuteTaskAsyncActionThrowsNonTwitterExceptionIfSwallowingDisabled()
        {
            // Arrange
            ExceptionHandler.SwallowWebExceptions = false;

            // Act
            await Sync.ExecuteTaskAsync(() => throw new TestException());
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public async Task ExecuteTaskAsyncActionThrowsNonTwitterExceptionIfSwallowingEnabled()
        {
            // Arrange
            ExceptionHandler.SwallowWebExceptions = true;

            // Act
            await Sync.ExecuteTaskAsync(() => throw new TestException());
        }

        [TestMethod]
        public async Task ExecuteTaskAsyncActionExceptionsWithinActionAvailableOnMainThreadContextIfSwallowingEnabled()
        {
            // Arrange
            ExceptionHandler.SwallowWebExceptions = true;
            ExceptionHandler.ClearLoggedExceptions();
            ITwitterException expectedException = A.Fake<ITwitterException>();
            ITwitterException[] expectedExceptions = new ITwitterException[] { expectedException };

            // Act
            await Sync.ExecuteTaskAsync(() => ExceptionHandler.AddTwitterException(expectedException));

            // Assert
            ITwitterException[] exceptions = ExceptionHandler.GetExceptions().ToArray();
            CollectionAssert.AreEqual(expectedExceptions, exceptions);
        }

        [TestMethod]
        public async Task ExecuteTaskAsyncActionExceptionsWithinActionsAreIndependent()
        {
            // Arrange
            ExceptionHandler.SwallowWebExceptions = true;
            ExceptionHandler.ClearLoggedExceptions();
            ITwitterException expectedException = A.Fake<ITwitterException>();
            ITwitterException[] expectedExceptions = new ITwitterException[] { expectedException };

            // Make one async call to add an exception
            await Sync.ExecuteTaskAsync(() => ExceptionHandler.AddTwitterException(expectedException));

            // Act: Make another call that doesn't add any exception
            await Sync.ExecuteTaskAsync(() => {  });

            // Assert
            ITwitterException[] exceptions = ExceptionHandler.GetExceptions().ToArray();
            CollectionAssert.AreEqual(expectedExceptions, exceptions);
        }

        [ThreadStatic]
        private static string strThreadStatic_ExecuteTaskAsyncActionCallsActionInAnotherThread;

        [TestMethod]
        public async Task ExecuteTaskAsyncActionCallsActionInAnotherThread()
        {
            // Arrange
            strThreadStatic_ExecuteTaskAsyncActionCallsActionInAnotherThread = "a";

            // Act
            await Sync.ExecuteTaskAsync(() =>
                // Assert (can be done within async due to regular exceptions still being thrown)
                Assert.AreNotEqual("a", strThreadStatic_ExecuteTaskAsyncActionCallsActionInAnotherThread));
        }

        [TestMethod]
        public async Task ExecuteTaskAsyncActionCarriesExceptionSwallowingSettingIntoNewThread()
        {
            // Arrange
            const bool expected = true;
            ExceptionHandler.SwallowWebExceptions = expected;

            // Act
            await Sync.ExecuteTaskAsync(() => 
                // Assert
                Assert.AreEqual(expected, ExceptionHandler.SwallowWebExceptions));
        }

        [TestMethod]
        public async Task ExecuteTaskAsyncActionWithNoExceptionHandlerOnCallingThreadStillGetsExceptionHandlerUpdatesFromInnerThread()
        {
            // Arrange
            ITwitterException exception = A.Fake<ITwitterException>();

            // Act
            await Sync.ExecuteTaskAsync(() => ExceptionHandler.AddTwitterException(exception));

            // Assert
            bool hasException = ExceptionHandler.TryPopException(out ITwitterException actual);
            Assert.IsTrue(hasException);
            Assert.AreEqual(exception, actual);
        }

        #endregion

        #region ExecuteTaskAsync<T>(Func<T> resultFunc)

        [TestMethod]
        public async Task ExecuteTaskAsyncFuncCallsFunc()
        {
            // Arrange
            bool called = false;
            const string expectedResult = "dummy value";

            // Act
            string actualResult = await Sync.ExecuteTaskAsync(() =>
            {
                called = true;
                return expectedResult;
            });

            // Assert
            Assert.IsTrue(called);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public async Task ExecuteTaskAsyncFuncExceptionsWithinActionsAreIndependent()
        {
            // Arrange
            ExceptionHandler.SwallowWebExceptions = true;
            ExceptionHandler.ClearLoggedExceptions();
            ITwitterException expectedException = A.Fake<ITwitterException>();
            ITwitterException[] expectedExceptions = new ITwitterException[] { expectedException };

            // Make one async call to add an exception
            await Sync.ExecuteTaskAsync(() =>
            {
                ExceptionHandler.AddTwitterException(expectedException);
                return 0;
            });

            // Act: Make another call that doesn't add any exception
            await Sync.ExecuteTaskAsync(() => 0);

            // Assert
            ITwitterException[] exceptions = ExceptionHandler.GetExceptions().ToArray();
            CollectionAssert.AreEqual(expectedExceptions, exceptions);
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public async Task ExecuteTaskAsyncFuncThrowsNonTwitterExceptionIfSwallowingDisabled()
        {
            // Arrange
            ExceptionHandler.SwallowWebExceptions = false;

            // Act
            await Sync.ExecuteTaskAsync(() =>
            {
                throw new TestException();
#pragma warning disable 162
                return 0;
#pragma warning restore 162
            });
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public async Task ExecuteTaskAsyncFuncThrowsNonTwitterExceptionIfSwallowingEnabled()
        {
            // Arrange
            ExceptionHandler.SwallowWebExceptions = true;

            // Act
            await Sync.ExecuteTaskAsync(() =>
            {
                throw new TestException();
#pragma warning disable 162
                return 0;
#pragma warning restore 162
            });
        }

        [TestMethod]
        public async Task ExecuteTaskAsyncFuncExceptionsWithinFuncAvailableOnMainThreadContextIfSwallowingEnabled()
        {
            // Arrange
            ExceptionHandler.SwallowWebExceptions = true;
            ExceptionHandler.ClearLoggedExceptions();
            ITwitterException expectedException = A.Fake<ITwitterException>();
            ITwitterException[] expectedExceptions = new ITwitterException[] { expectedException };

            // Act
            await Sync.ExecuteTaskAsync(() =>
            {
                ExceptionHandler.AddTwitterException(expectedException);
                return 0;
            });

            // Assert
            ITwitterException[] exceptions = ExceptionHandler.GetExceptions().ToArray();
            CollectionAssert.AreEqual(expectedExceptions, exceptions);
        }

        [ThreadStatic]
        private static string strThreadStatic_ExecuteTaskAsyncFuncCallsFuncInAnotherThread;

        [TestMethod]
        public async Task ExecuteTaskAsyncFuncCallsFuncInAnotherThread()
        {
            // Arrange
            strThreadStatic_ExecuteTaskAsyncFuncCallsFuncInAnotherThread = "a";
            ExceptionHandler.SwallowWebExceptions = false;

            // Act
            await Sync.ExecuteTaskAsync(() =>
            {
                // Assert (can be done within async due to regular exceptions still being thrown)
                Assert.AreNotEqual("a", strThreadStatic_ExecuteTaskAsyncFuncCallsFuncInAnotherThread);
                return 0;
            });
        }

        [TestMethod]
        public async Task ExecuteTaskAsyncFuncCarriesExceptionSwallowingSettingIntoNewThread()
        {
            // Arrange
            const bool expected = true;
            ExceptionHandler.SwallowWebExceptions = expected;

            // Act
            await Sync.ExecuteTaskAsync(() =>
            {
                // Assert
                Assert.AreEqual(expected, ExceptionHandler.SwallowWebExceptions);
                return 0;
            });

        }

        [TestMethod]
        public async Task ExecuteTaskAsyncFuncWithNoExceptionHandlerOnCallingThreadStillGetsExceptionHandlerUpdatesFromInnerThread()
        {
            // Arrange
            ITwitterException exception = A.Fake<ITwitterException>();

            // Act
            await Sync.ExecuteTaskAsync(() =>
            {
                ExceptionHandler.AddTwitterException(exception);

                return 0;
            });

            // Assert
            bool hasException = ExceptionHandler.TryPopException(out ITwitterException actual);
            Assert.IsTrue(hasException);
            Assert.AreEqual(exception, actual);
        }

        #endregion
    }
}
