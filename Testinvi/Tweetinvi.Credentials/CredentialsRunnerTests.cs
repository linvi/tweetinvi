using System.Diagnostics;
using System.Threading;
using AutoFixture;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.TestObjects;
using Tweetinvi;
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
            Auth.CredentialsRunner.ExecuteOperationWithCredentials(execAsCredentials, () =>
            {
                Assert.AreEqual(Auth.Credentials, execAsCredentials);
            });

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
            Debug.WriteLine("SetCredentials");
            Auth.SetCredentials(callingCredentials);

            ITwitterCredentials execAsCredentials = fixture.Create<TwitterCredentials>();

            // Act
            Debug.WriteLine("Before ExecuteOperationWithCredentials");
            Auth.CredentialsRunner.ExecuteOperationWithCredentials(execAsCredentials, () => { });

            // Assert

            Debug.WriteLine("After ExecuteOperationWithCredentials");
            var currentCredentials = Auth.Credentials;
            Assert.AreEqual(callingCredentials, currentCredentials);
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
