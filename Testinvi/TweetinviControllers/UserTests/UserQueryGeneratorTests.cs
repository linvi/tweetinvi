using System;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.User;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Testinvi.TweetinviControllers.UserTests
{
    [TestClass]
    public class UserQueryGeneratorTests
    {
        private FakeClassBuilder<UserQueryGenerator> _fakeBuilder;
        private Fake<IUserQueryParameterGenerator> _fakeUserQueryParameterGenerator;
        private Fake<IUserQueryValidator> _fakeUserQueryValidator;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<UserQueryGenerator>();
            _fakeUserQueryParameterGenerator = _fakeBuilder.GetFake<IUserQueryParameterGenerator>();
            _fakeUserQueryValidator = _fakeBuilder.GetFake<IUserQueryValidator>();
        }

        #region Friends

        [TestMethod]
        public void GetFriendIdsQuery_WithValidUserDTO_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userDTO = GenerateUserDTO(true);
            var maximumNumberOfFriends = TestHelper.GenerateRandomInt();
            var userIdParameter = UserQueryGeneratorHelper.GenerateParameterExpectedResult(userDTO);

            // Act
            var result = queryGenerator.GetFriendIdsQuery(userDTO, maximumNumberOfFriends);

            // Assert
            var expectedResult = string.Format(Resources.User_GetFriends, userIdParameter, maximumNumberOfFriends);
            Assert.AreEqual(result, expectedResult);

            _fakeUserQueryValidator.CallsTo(x => x.ThrowIfUserCannotBeIdentified(userDTO)).MustHaveHappened();
        }

        #endregion

        #region Followers

        [TestMethod]
        public void GetFollowerIdsQuery_WithValidUserDTO_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userDTO = GenerateUserDTO(true);
            var maximumNumberOfFollowers = TestHelper.GenerateRandomInt();
            var userIdParameter = UserQueryGeneratorHelper.GenerateParameterExpectedResult(userDTO);

            // Act
            var result = queryGenerator.GetFollowerIdsQuery(userDTO, maximumNumberOfFollowers);

            // Assert
            var expectedResult = string.Format(Resources.User_GetFollowers, userIdParameter, maximumNumberOfFollowers);
            Assert.AreEqual(result, expectedResult);

            _fakeUserQueryValidator.CallsTo(x => x.ThrowIfUserCannotBeIdentified(userDTO)).MustHaveHappened();
        }

        #endregion

        #region Favourites

        [TestMethod]
        public void GetFavouriteTweetsQuery_WithValidUserDTO_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            _fakeUserQueryValidator.CallsTo(x => x.CanUserBeIdentified(It.IsAny<IUserIdentifier>())).Returns(true);
            var userDTO = GenerateUserDTO(true);
            var maximumNumberOfFavourites = TestHelper.GenerateRandomInt();
            var userIdParameter = UserQueryGeneratorHelper.GenerateParameterExpectedResult(userDTO);

            var parameters = A.Fake<IGetUserFavoritesQueryParameters>();
            parameters.UserIdentifier = userDTO;
            parameters.Parameters.MaximumNumberOfTweetsToRetrieve = maximumNumberOfFavourites;
            parameters.Parameters.IncludeEntities = true;

            // Act
            var result = queryGenerator.GetFavoriteTweetsQuery(parameters);

            // Assert
            var expectedResult = string.Format("{0}user_id={1}", Resources.User_GetFavourites, userIdParameter);
            Assert.IsTrue(result.StartsWith(Resources.User_GetFavourites));
            Assert.IsTrue(result.Contains("count=" + maximumNumberOfFavourites));
            Assert.IsTrue(result.Contains("include_entities=true"));

            _fakeUserQueryValidator.CallsTo(x => x.ThrowIfUserCannotBeIdentified(userDTO)).MustHaveHappened();
        }

        #endregion

        #region Block

        [TestMethod]
        public void GetBlockUserQuery_WithValidUserDTO_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userDTO = GenerateUserDTO(true);
            var userIdParameter = UserQueryGeneratorHelper.GenerateParameterExpectedResult(userDTO);

            // Act
            var result = queryGenerator.GetBlockUserQuery(userDTO);

            // Assert
            var expectedResult = string.Format(Resources.User_Block_Create, userIdParameter);
            Assert.AreEqual(result, expectedResult);

            _fakeUserQueryValidator.CallsTo(x => x.ThrowIfUserCannotBeIdentified(userDTO)).MustHaveHappened();
        }

        #endregion

        #region Download Profile Image

        [TestMethod]
        public void DownloadProfileImageURL_WithBothURL_ReturnHttpsUrl()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userDTO = A.Fake<IUserDTO>();
            var profileImageUrlHttps = TestHelper.GenerateString();
            var profileImageUrlHttp = TestHelper.GenerateString();
            userDTO.CallsTo(x => x.ProfileImageUrl).Returns(profileImageUrlHttp);
            userDTO.CallsTo(x => x.ProfileImageUrlHttps).Returns(profileImageUrlHttps);

            // Act
            var result = queryGenerator.DownloadProfileImageURL(userDTO, ImageSize.bigger);

            // Assert
            Assert.AreEqual(result, profileImageUrlHttps);
        }

        [TestMethod]
        public void DownloadProfileImageURL_HttpsImageExist_ReturnHttpsUrl()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userDTO = A.Fake<IUserDTO>();
            var profileImageUrlHttps = TestHelper.GenerateString();
            userDTO.CallsTo(x => x.ProfileImageUrlHttps).Returns(profileImageUrlHttps);

            // Act
            var result = queryGenerator.DownloadProfileImageURL(userDTO, ImageSize.bigger);

            // Assert
            Assert.AreEqual(result, profileImageUrlHttps);
        }

        [TestMethod]
        public void DownloadProfileImageURL_HttpImageExist_ReturnHttpUrl()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userDTO = A.Fake<IUserDTO>();
            var profileImageUrl = TestHelper.GenerateString();
            userDTO.CallsTo(x => x.ProfileImageUrl).Returns(profileImageUrl);

            // Act
            var result = queryGenerator.DownloadProfileImageURL(userDTO, ImageSize.bigger);

            // Assert
            Assert.AreEqual(result, profileImageUrl);
        }

        [TestMethod]
        public void DownloadProfileImageURL_HttpsAndHttpImagesDoNotExist_ReturnNull()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userDTO = A.Fake<IUserDTO>();

            // Act
            var result = queryGenerator.DownloadProfileImageURL(userDTO, ImageSize.bigger);

            // Assert
            Assert.IsNull(result);
        }

        // Download Profile Image in HTTP
        [TestMethod]
        public void DownloadProfileImageInHttpURL_WithBothURL_ReturnHttpsUrl()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userDTO = A.Fake<IUserDTO>();
            var profileImageUrlHttps = TestHelper.GenerateString();
            var profileImageUrlHttp = TestHelper.GenerateString();
            userDTO.CallsTo(x => x.ProfileImageUrl).Returns(profileImageUrlHttp);
            userDTO.CallsTo(x => x.ProfileImageUrlHttps).Returns(profileImageUrlHttps);

            // Act
            var result = queryGenerator.DownloadProfileImageInHttpURL(userDTO, ImageSize.bigger);

            // Assert
            Assert.AreEqual(result, profileImageUrlHttp);
        }

        [TestMethod]
        public void DownloadProfileImageInHttpURL_HttpsImageExist_ReturnHttpsUrl()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userDTO = A.Fake<IUserDTO>();
            var profileImageUrlHttps = TestHelper.GenerateString();
            userDTO.CallsTo(x => x.ProfileImageUrlHttps).Returns(profileImageUrlHttps);

            // Act
            var result = queryGenerator.DownloadProfileImageInHttpURL(userDTO, ImageSize.bigger);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DownloadProfileImageInHttpURL_HttpImageExist_ReturnHttpUrl()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userDTO = A.Fake<IUserDTO>();
            var profileImageUrl = TestHelper.GenerateString();
            userDTO.CallsTo(x => x.ProfileImageUrl).Returns(profileImageUrl);

            // Act
            var result = queryGenerator.DownloadProfileImageInHttpURL(userDTO, ImageSize.bigger);

            // Assert
            Assert.AreEqual(result, profileImageUrl);
        }

        [TestMethod]
        public void DownloadProfileImageInHttpURL_HttpsAndHttpImagesDoNotExist_ReturnNull()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userDTO = A.Fake<IUserDTO>();

            // Act
            var result = queryGenerator.DownloadProfileImageInHttpURL(userDTO, ImageSize.bigger);

            // Assert
            Assert.IsNull(result);
        }

        #endregion

        #region Spam

        [TestMethod]
        public void GetReportUserForSpamQuery_WithValidUserDTO_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userDTO = GenerateUserDTO(true);
            var userIdParameter = UserQueryGeneratorHelper.GenerateParameterExpectedResult(userDTO);

            // Act
            var result = queryGenerator.GetReportUserForSpamQuery(userDTO);

            // Assert
            var expectedResult = string.Format(Resources.User_Report_Spam, userIdParameter);
            Assert.AreEqual(result, expectedResult);

            _fakeUserQueryValidator.CallsTo(x => x.ThrowIfUserCannotBeIdentified(userDTO)).MustHaveHappened();
        }

        #endregion

        private IUserDTO GenerateUserDTO(bool isValid)
        {
            var userDTO = A.Fake<IUserDTO>();
            _fakeUserQueryValidator.CallsTo(x => x.CanUserBeIdentified(userDTO)).Returns(isValid);

            if (!isValid)
            {
                _fakeUserQueryValidator.CallsTo(x => x.ThrowIfUserCannotBeIdentified(userDTO)).Throws(new ArgumentException());
                _fakeUserQueryValidator.CallsTo(x => x.ThrowIfUserCannotBeIdentified(userDTO, It.IsAny<string>())).Throws(new ArgumentException());
            }

            _fakeUserQueryParameterGenerator.ArrangeGenerateIdOrScreenNameParameter();
            return userDTO;
        }

        public UserQueryGenerator CreateUserQueryGenerator()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}