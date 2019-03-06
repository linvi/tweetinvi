using System.Linq;
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
        public async Task ExceptionHandlerAsyncContextCarriesToChildTask()
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
                var actualExceptions = ExceptionHandler.GetExceptions().ToArray();
                CollectionAssert.AreEqual(expectedExceptions, actualExceptions);
            });
        }

        [TestMethod]
        public async Task ExceptionHandlerConfiguredFromAnotherAsyncContextIsProperlyPopulated()
        {
            // Arrange
            ExceptionHandler.OnTwitterExceptionReturnNull = true;
            ExceptionHandler.ClearLoggedExceptions();
            ITwitterException expectedException = A.Fake<ITwitterException>();
            ITwitterException[] expectedExceptions = { expectedException };

            // Act
            await Sync.ExecuteTaskAsync(() => ExceptionHandler.AddTwitterException(expectedException));

            // Assert
            ITwitterException[] exceptions = ExceptionHandler.GetExceptions().ToArray();
            CollectionAssert.AreEqual(expectedExceptions, exceptions);
        }

        [TestMethod]
        public async Task ExecuteTaskAsyncActionCarriesExceptionSwallowingSettingIntoNewThread()
        {
            // Arrange
            const bool expected = true;
            ExceptionHandler.OnTwitterExceptionReturnNull = expected;

            // Act
            await Sync.ExecuteTaskAsync(() =>
            {
                // Assert
                Assert.AreEqual(expected, ExceptionHandler.OnTwitterExceptionReturnNull);
            });
        }
    }
}