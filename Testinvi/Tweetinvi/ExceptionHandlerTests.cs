using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tweetinvi;
using Tweetinvi.Core.Exceptions;

namespace Testinvi.Tweetinvi
{
    [TestClass]
    public class ExceptionHandlerTests
    {
        [TestMethod]
        public async Task CurrentThreadExceptionHandlerCarriesToCalledTask()
        {
            // Arrange
            ExceptionHandler.ClearLoggedExceptions();
            ITwitterException expectedException = A.Fake<ITwitterException>();
            ExceptionHandler.AddTwitterException(expectedException);
            ITwitterException[] expectedExceptions = new ITwitterException[] { expectedException };

            // Act
            await Task.Run(() =>
            {
                // Assert
                ITwitterException[] actualExceptions = ExceptionHandler.GetExceptions().ToArray();
                CollectionAssert.AreEqual(expectedExceptions, actualExceptions);
            });
        }

        [TestMethod]
        public async Task CurrentThreadExceptionHandlerIsSpecificToExecutionContext()
        {
            // Arrange
            ExceptionHandler.ClearLoggedExceptions();
            ExceptionHandler.AddTwitterException(A.Fake<ITwitterException>());

            // Prevent execution context from being passed over to the new thread
            Task t;
            using (ExecutionContext.SuppressFlow())
            {
                // Act
                t = Task.Run(() =>
                    // Assert
                    Assert.IsFalse(ExceptionHandler.GetExceptions().Any()));
            }
            await t;
        }
    }
}
