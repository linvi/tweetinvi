using System;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.User;
using Tweetinvi.Core;
using Tweetinvi.Core.Interfaces.DTO;

namespace Testinvi.TweetinviControllers.UserTests
{
    [TestClass]
    public class UserQueryValidatorTests
    {
        private FakeClassBuilder<UserQueryValidator> _fakeBuilder;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<UserQueryValidator>();
        }

        #region CanUserBeIdentified

        [TestMethod]
        public void CanUserBeIdentified_UserIsNull_ReturnsFalse()
        {
            // Arrange
            var queryValidator = CreateUserQuerValidator();

            // Act
            var result = queryValidator.CanUserBeIdentified(null);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanUserBeIdentified_WithUser_ReturnsExpectedResult()
        {
            VerifyCanUserBeIdentified(false, false, false, true);
            VerifyCanUserBeIdentified(false, false, true, true);
            VerifyCanUserBeIdentified(false, true, false, true);
            VerifyCanUserBeIdentified(false, true, true, true);

            VerifyCanUserBeIdentified(true, false, false, true);
            VerifyCanUserBeIdentified(true, false, true, false);
            VerifyCanUserBeIdentified(true, true, false, false);
            VerifyCanUserBeIdentified(true, true, true, false);
        }

        private void VerifyCanUserBeIdentified(
            bool isUserIdDefault,
            bool isUserScreenNameNull,
            bool isUserScreenNameEmpty,
            bool expectedResult)
        {
            // Arrange
            var queryValidator = CreateUserQuerValidator();
            var user = GenerateUserDTO(isUserIdDefault, isUserScreenNameNull, isUserScreenNameEmpty);

            // Act
            var result = queryValidator.CanUserBeIdentified(user);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region IsScreenNameValid

        [TestMethod]
        public void IsScreenNameValid_Null_ReturnsFalse()
        {
            // Arrange
            var queryValidator = CreateUserQuerValidator();

            // Act
            var result = queryValidator.IsScreenNameValid(null);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsScreenNameValid_Empty_ReturnsFalse()
        {
            // Arrange
            var queryValidator = CreateUserQuerValidator();

            // Act
            var result = queryValidator.IsScreenNameValid(String.Empty);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsScreenNameValid_Any_ReturnsTrue()
        {
            // Arrange
            var queryValidator = CreateUserQuerValidator();

            // Act
            var result = queryValidator.IsScreenNameValid(TestHelper.GenerateString());

            // Assert
            Assert.IsTrue(result);
        }

        #endregion

        #region IsUserIdValid

        [TestMethod]
        public void IsUserIdValid_IsDefault_ReturnsFalse()
        {
            // Arrange
            var queryValidator = CreateUserQuerValidator();

            // Act
            var result = queryValidator.IsUserIdValid(TweetinviSettings.DEFAULT_ID);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsUserIdValid_Any_ReturnsTrue()
        {
            // Arrange
            var queryValidator = CreateUserQuerValidator();

            // Act
            var result = queryValidator.IsUserIdValid(TestHelper.GenerateRandomLong());

            // Assert
            Assert.IsTrue(result);
        }

        #endregion

        private IUserDTO GenerateUserDTO(bool isUserIdDefault, bool isUserScreenNameNull, bool isUserScreenNameEmpty)
        {
            var userDTO = A.Fake<IUserDTO>();
            var expectedScreenName = isUserScreenNameNull ? null : isUserScreenNameEmpty ? String.Empty : TestHelper.GenerateString();
            userDTO.CallsTo(x => x.Id).Returns(isUserIdDefault ? TweetinviSettings.DEFAULT_ID : TestHelper.GenerateRandomLong());
            userDTO.CallsTo(x => x.ScreenName).Returns(expectedScreenName);
            return userDTO;
        }

        public UserQueryValidator CreateUserQuerValidator()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}