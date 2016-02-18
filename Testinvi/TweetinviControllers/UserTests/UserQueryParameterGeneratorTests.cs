using System;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.User;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.QueryValidators;

namespace Testinvi.TweetinviControllers.UserTests
{
    [TestClass]
    public class UserQueryParameterGeneratorTests
    {
        private FakeClassBuilder<UserQueryParameterGenerator> _fakeBuilder;
        private Fake<IUserQueryValidator> _fakeUserQueryValidator;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<UserQueryParameterGenerator>();
            _fakeUserQueryValidator = _fakeBuilder.GetFake<IUserQueryValidator>();
        }

        #region Generate UserId Parameter

        [TestMethod]
        public void GenerateUserIdParameter_UserIdIsNotValid_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryParameterGenerator();
            var userId = TestHelper.GenerateRandomLong();
            var parameterName = TestHelper.GenerateString();

            _fakeUserQueryValidator.CallsTo(x => x.IsUserIdValid(userId)).Returns(false);

            // Act
            var result = queryGenerator.GenerateUserIdParameter(userId, parameterName);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GenerateUserIdParameter_UserIdIsValid_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryParameterGenerator();
            var userId = TestHelper.GenerateRandomLong();
            var parameterName = TestHelper.GenerateString();

            _fakeUserQueryValidator.CallsTo(x => x.IsUserIdValid(userId)).Returns(true);

            // Act
            var result = queryGenerator.GenerateUserIdParameter(userId, parameterName);

            // Assert
            var expectedQuery = string.Format("{0}={1}", parameterName, userId);
            Assert.AreEqual(result, expectedQuery);
        }
        #endregion

        #region Generate UserScreenName Parameter

        [TestMethod]
        public void GenerateUserScreenNameParameter_UserScreenNameIsNotValid_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateUserQueryParameterGenerator();
            var userScreenName = TestHelper.GenerateString();
            var parameterName = TestHelper.GenerateString();

            _fakeUserQueryValidator.CallsTo(x => x.IsScreenNameValid(userScreenName)).Returns(false);

            // Act
            var result = queryGenerator.GenerateScreenNameParameter(userScreenName, parameterName);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GenerateUserScreenNameParameter_UserScreenNameIsValid_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryParameterGenerator();
            var userScreenName = TestHelper.GenerateString();
            var parameterName = TestHelper.GenerateString();

            _fakeUserQueryValidator.CallsTo(x => x.IsScreenNameValid(userScreenName)).Returns(true);

            // Act
            var result = queryGenerator.GenerateScreenNameParameter(userScreenName, parameterName);

            // Assert
            var expectedQuery = string.Format("{0}={1}", parameterName, userScreenName);
            Assert.AreEqual(result, expectedQuery);
        }

        #endregion

        #region Generate UserIdentifier Parameter

        [TestMethod]
        public void GenerateIdOrScreenNameParameter_UserDTOIsNull_ThrowsArgumentException()
        {
            // Arrange
            var queryGenerator = CreateUserQueryParameterGenerator();
            var idParameterName = TestHelper.GenerateString();
            var screenNameParameterName = TestHelper.GenerateString();

            // Act
            try
            {
                queryGenerator.GenerateIdOrScreenNameParameter(null, idParameterName, screenNameParameterName);
            }
            catch (ArgumentException)
            {
                return;
            }

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        public void GenerateIdOrScreenNameParameter_UserDTOIsNotNullButInvalid_ThrowsArgumentException()
        {
            // Arrange
            var queryGenerator = CreateUserQueryParameterGenerator();
            var userDTO = A.Fake<IUserDTO>();
            var idParameterName = TestHelper.GenerateString();
            var screenNameParameterName = TestHelper.GenerateString();

            _fakeUserQueryValidator.CallsTo(x => x.CanUserBeIdentified(userDTO)).Returns(false);

            // Act
            try
            {
                queryGenerator.GenerateIdOrScreenNameParameter(userDTO, idParameterName, screenNameParameterName);
            }
            catch (ArgumentException)
            {
                return;
            }

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        public void GenerateIdOrScreenNameParameter_UserDTOHasValidWithUserId_ReturnsUserIdParameter()
        {
            // Arrange
            var queryGenerator = CreateUserQueryParameterGenerator();
            var userId = TestHelper.GenerateRandomLong();
            var screenName = TestHelper.GenerateString();
            var userDTO = A.Fake<IUserDTO>();
            userDTO.CallsTo(x => x.Id).Returns(userId);
            userDTO.CallsTo(x => x.ScreenName).Returns(screenName);
            var idParameterName = TestHelper.GenerateString();
            var screenNameParameterName = TestHelper.GenerateString();

            _fakeUserQueryValidator.CallsTo(x => x.CanUserBeIdentified(userDTO)).Returns(true);
            _fakeUserQueryValidator.CallsTo(x => x.IsUserIdValid(userId)).Returns(true);
            _fakeUserQueryValidator.CallsTo(x => x.IsScreenNameValid(screenName)).Returns(false);

            // Act
            var result = queryGenerator.GenerateIdOrScreenNameParameter(userDTO, idParameterName, screenNameParameterName);

            // Assert
            var expectedParameter = string.Format("{0}={1}", idParameterName, userId);
            Assert.AreEqual(result, expectedParameter);
        }

        [TestMethod]
        public void GenerateIdOrScreenNameParameter_UserDTOHasValidScreenName_ReturnsUserScreenNameParameter()
        {
            var queryGenerator = CreateUserQueryParameterGenerator();
            var userId = TestHelper.GenerateRandomLong();
            var screenName = TestHelper.GenerateString();
            var userDTO = A.Fake<IUserDTO>();
            userDTO.CallsTo(x => x.Id).Returns(userId);
            userDTO.CallsTo(x => x.ScreenName).Returns(screenName);
            var idParameterName = TestHelper.GenerateString();
            var screenNameParameterName = TestHelper.GenerateString();

            _fakeUserQueryValidator.CallsTo(x => x.CanUserBeIdentified(userDTO)).Returns(true);
            _fakeUserQueryValidator.CallsTo(x => x.IsUserIdValid(userId)).Returns(false);
            _fakeUserQueryValidator.CallsTo(x => x.IsScreenNameValid(screenName)).Returns(true);

            // Act
            var result = queryGenerator.GenerateIdOrScreenNameParameter(userDTO, idParameterName, screenNameParameterName);

            // Assert
            var expectedParameter = string.Format("{0}={1}", screenNameParameterName, screenName);
            Assert.AreEqual(result, expectedParameter);
        }

        [TestMethod]
        public void GenerateIdOrScreenNameParameter_UserDTOHasIdAndValidScreenName_ReturnsUserIdParameter()
        {
            var queryGenerator = CreateUserQueryParameterGenerator();
            var userId = TestHelper.GenerateRandomLong();
            var screenName = TestHelper.GenerateString();
            var userDTO = A.Fake<IUserDTO>();
            userDTO.CallsTo(x => x.Id).Returns(userId);
            userDTO.CallsTo(x => x.ScreenName).Returns(screenName);
            var idParameterName = TestHelper.GenerateString();
            var screenNameParameterName = TestHelper.GenerateString();

            _fakeUserQueryValidator.CallsTo(x => x.CanUserBeIdentified(userDTO)).Returns(true);
            _fakeUserQueryValidator.CallsTo(x => x.IsUserIdValid(userId)).Returns(true);
            _fakeUserQueryValidator.CallsTo(x => x.IsScreenNameValid(screenName)).Returns(true);

            // Act
            var result = queryGenerator.GenerateIdOrScreenNameParameter(userDTO, idParameterName, screenNameParameterName);

            // Assert
            var expectedParameter = string.Format("{0}={1}", idParameterName, userId);
            Assert.AreEqual(result, expectedParameter);
        }

        #endregion

        public UserQueryParameterGenerator CreateUserQueryParameterGenerator()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}