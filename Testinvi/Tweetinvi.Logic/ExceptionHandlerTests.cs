using System.Linq;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Exceptions;
using ExceptionHandler = Tweetinvi.Logic.Exceptions.ExceptionHandler;

namespace Testinvi.Tweetinvi.Logic
{
    [TestClass]
    public class ExceptionHandlerTests
    {
        [TestMethod]
        public void NewExceptionHandlerHasNoExceptions()
        {
            // Arrange
            ITwitterExceptionFactory exceptionFactory = A.Fake<ITwitterExceptionFactory>();
            ExceptionHandler exceptionHandler = new ExceptionHandler(exceptionFactory);

            // Assert
            Assert.IsFalse(exceptionHandler.ExceptionInfos.Any());
        }

        [TestMethod]
        public void AddTwitterExceptionStoresException()
        {
            // Arrange
            ITwitterExceptionFactory exceptionFactory = A.Fake<ITwitterExceptionFactory>();
            ExceptionHandler exceptionHandler = new ExceptionHandler(exceptionFactory);
            ITwitterException expectedException = A.Fake<ITwitterException>();
            ITwitterException[] expectedExceptions = new ITwitterException[] { expectedException };

            // Act
            exceptionHandler.AddTwitterException(expectedException);

            // Assert
            ITwitterException[] actualExceptions = exceptionHandler.ExceptionInfos.ToArray();
            CollectionAssert.AreEqual(expectedExceptions, actualExceptions);
        }

        [TestMethod]
        public void ClearLoggedExceptionsLeavesNoExceptions()
        {
            // Arrange
            ITwitterExceptionFactory exceptionFactory = A.Fake<ITwitterExceptionFactory>();
            ExceptionHandler exceptionHandler = new ExceptionHandler(exceptionFactory);
            exceptionHandler.AddTwitterException(A.Fake<ITwitterException>());

            // Act
            exceptionHandler.ClearLoggedExceptions();

            // Assert
            Assert.IsFalse(exceptionHandler.ExceptionInfos.Any());
        }

        [TestMethod]
        public void GetExceptionsReturnsInTheExpectedOrder()
        {
            // Arrange
            ITwitterExceptionFactory exceptionFactory = A.Fake<ITwitterExceptionFactory>();
            ExceptionHandler exceptionHandler = new ExceptionHandler(exceptionFactory);

            var exception1 = A.Fake<ITwitterException>();
            var exception2 = A.Fake<ITwitterException>();
            var exception3 = A.Fake<ITwitterException>();

            exceptionHandler.AddTwitterException(exception1);
            exceptionHandler.AddTwitterException(exception2);
            exceptionHandler.AddTwitterException(exception3);

            // Act
            var lastException = exceptionHandler.LastExceptionInfos;
            var exceptions = exceptionHandler.ExceptionInfos.ToArray();

            // Assert
            Assert.AreEqual(lastException, exception3);
            Assert.AreEqual(exceptions[0],  exception1);
            Assert.AreEqual(exceptions[1], exception2);
            Assert.AreEqual(exceptions[2], exception3);
        }
    }
}
