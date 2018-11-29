using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tweetinvi;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Exceptions;
using Tweetinvi.Logic.Exceptions;
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

            // New exception handler 
            Assert.IsFalse(exceptionHandler.ExceptionInfos.Any());
        }
    }
}
