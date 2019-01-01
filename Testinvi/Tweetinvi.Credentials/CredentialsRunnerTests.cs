using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.TestObjects;
using Tweetinvi;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Models;

namespace Testinvi.Tweetinvi.Credentials
{
    [TestClass]
    public class CredentialsRunnerTests
    {
        #region ExecuteOperationWithCredentials<T>(ITwitterCredentials, Func<T>)

        [TestMethod]
        public void ExecuteOperationWithCredentialsFuncCallsFunc()
        {
            // Arrange
            bool called = false;
            ITwitterCredentials credentials = A.Fake<ITwitterCredentials>();

            // Act
            Auth.CredentialsRunner.ExecuteOperationWithCredentials(credentials, () =>
            {
                called = true;
                return 0;
            });

            // Assert
            Assert.IsTrue(called);
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void ExecuteOperationWithCredentialsFuncPropagatesExceptions()
        {
            // Arrange
            ITwitterCredentials credentials = A.Fake<ITwitterCredentials>();

            // Act
            Auth.CredentialsRunner.ExecuteOperationWithCredentials(credentials, () =>
            {
                throw new TestException();
#pragma warning disable 162
                return 0;
#pragma warning restore 162
            });
        }

        [TestMethod]
        public void ExecuteOperationWithCredentialsFuncHasCredentialsSet()
        {
            // Arrange
            ITwitterCredentials credentials = new Fixture().Create<TwitterCredentials>();

            // Act
            Auth.CredentialsRunner.ExecuteOperationWithCredentials(credentials, () =>
            {
                // Assert
                ITwitterCredentials actual = Auth.Credentials;
                Assert.AreEqual(credentials, actual);
                return 0;
            });
        }

        [TestMethod]
        public void ExecuteOperationWithCredentialsFuncDoesNotUpdateCredentialsInCallingContext()
        {
            // Arrange
            Fixture fixture = new Fixture();

            ITwitterCredentials callingCredentials = fixture.Create<TwitterCredentials>();
            Auth.SetCredentials(callingCredentials);

            ITwitterCredentials execAsCredentials = fixture.Create<TwitterCredentials>();

            // Act
            Auth.CredentialsRunner.ExecuteOperationWithCredentials(execAsCredentials, () => 0);

            // Assert
            ITwitterCredentials actual = Auth.Credentials;
            Assert.AreEqual(callingCredentials, actual);
        }

        [TestMethod]
        public void ExecuteOperationWithCredentialsFuncKeepsExecutionContext()
        {
            // Arrange
            string expected = new Fixture().Create<string>();
            AsyncLocal<string> al = new AsyncLocal<string> { Value = expected };
            ITwitterCredentials credentials = A.Fake<ITwitterCredentials>();

            // Act
            Auth.CredentialsRunner.ExecuteOperationWithCredentials(credentials, () =>
            {
                // Assert
                string actual = al.Value;
                Assert.AreEqual(expected, actual);
                return 0;
            });
        }

        [TestMethod]
        public void ExecuteOperationWithCredentialsFuncExceptionSwallowedAvailableInCallingContext()
        {
            // Arrange
            ITwitterCredentials credentials = A.Fake<ITwitterCredentials>();
            ITwitterException exception = A.Fake<ITwitterException>();

            // Act
            Auth.CredentialsRunner.ExecuteOperationWithCredentials(credentials,
                () =>
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

        #region ExecuteOperationWithCredentials<T>(ITwitterCredentials, Action)

        [TestMethod]
        public void ExecuteOperationWithCredentialsActionCallsAction()
        {
            // Arrange
            bool called = false;
            ITwitterCredentials credentials = A.Fake<ITwitterCredentials>();

            // Act
            Auth.CredentialsRunner.ExecuteOperationWithCredentials(credentials, () => { called = true; });

            // Assert
            Assert.IsTrue(called);
        }

        [TestMethod]
        [ExpectedException(typeof(TestException))]
        public void ExecuteOperationWithCredentialsActionPropagatesExceptions()
        {
            // Arrange
            ITwitterCredentials credentials = A.Fake<ITwitterCredentials>();

            // Act
            Auth.CredentialsRunner.ExecuteOperationWithCredentials(credentials, () => throw new TestException());
        }

        [TestMethod]
        public void ExecuteOperationWithCredentialsActionHasCredentialsSet()
        {
            // Arrange
            ITwitterCredentials credentials = new Fixture().Create<TwitterCredentials>();

            // Act
            Auth.CredentialsRunner.ExecuteOperationWithCredentials(credentials, () =>
            {
                // Assert
                ITwitterCredentials actual = Auth.Credentials;
                Assert.AreEqual(credentials, actual);
            });
        }

        [TestMethod]
        public void ExecuteOperationWithCredentialsActionDoesNotUpdateCredentialsInCallingContext()
        {
            // Arrange
            Fixture fixture = new Fixture();

            ITwitterCredentials callingCredentials = fixture.Create<TwitterCredentials>();
            Auth.SetCredentials(callingCredentials);

            ITwitterCredentials execAsCredentials = fixture.Create<TwitterCredentials>();

            // Act
            Auth.CredentialsRunner.ExecuteOperationWithCredentials(execAsCredentials, () => {  });

            // Assert
            ITwitterCredentials actual = Auth.Credentials;
            Assert.AreEqual(callingCredentials, actual);
        }

        [TestMethod]
        public void ExecuteOperationWithCredentialsActionKeepsExecutionContext()
        {
            // Arrange
            string expected = new Fixture().Create<string>();
            AsyncLocal<string> al = new AsyncLocal<string> { Value = expected };
            ITwitterCredentials credentials = A.Fake<ITwitterCredentials>();

            // Act
            Auth.CredentialsRunner.ExecuteOperationWithCredentials(credentials, () =>
            {
                // Assert
                string actual = al.Value;
                Assert.AreEqual(expected, actual);
            });
        }

        [TestMethod]
        public void ExecuteOperationWithCredentialsActionExceptionSwallowedAvailableInCallingContext()
        {
            // Arrange
            ITwitterCredentials credentials = A.Fake<ITwitterCredentials>();
            ITwitterException exception = A.Fake<ITwitterException>();

            // Act
            Auth.CredentialsRunner.ExecuteOperationWithCredentials(credentials,
                () => ExceptionHandler.AddTwitterException(exception));

            // Assert
            bool hasException = ExceptionHandler.TryPopException(out ITwitterException actual);
            Assert.IsTrue(hasException);
            Assert.AreEqual(exception, actual);
        }

        #endregion
    }
}
