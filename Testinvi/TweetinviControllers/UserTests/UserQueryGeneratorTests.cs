using System;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.User;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Interfaces.QueryValidators;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Parameters.QueryParameters;

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
        }

        [TestMethod]
        public void GetFriendIdsQuery_WithInValidUserDTO_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userDTO = GenerateUserDTO(false);
            var maximumNumberOfFriends = TestHelper.GenerateRandomInt();

            // Act
            var result = queryGenerator.GetFriendIdsQuery(userDTO, maximumNumberOfFriends);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetFriendIdsQuery_WithValidUserScreenName_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userScreenName = TestHelper.GenerateString();
            var maximumNumberOfFriends = TestHelper.GenerateRandomInt();
            var userIdParameter = UserQueryGeneratorHelper.GenerateParameterExpectedResult(userScreenName);

            _fakeUserQueryValidator.CallsTo(x => x.IsScreenNameValid(userScreenName)).Returns(true);
            _fakeUserQueryParameterGenerator.ArrangeGenerateScreenNameParameter();

            // Act
            var result = queryGenerator.GetFriendIdsQuery(userScreenName, maximumNumberOfFriends);

            // Assert
            var expectedResult = string.Format(Resources.User_GetFriends, userIdParameter, maximumNumberOfFriends);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetFriendIdsQuery_WithInValidUserScreenName_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userScreenName = TestHelper.GenerateString();
            var maximumNumberOfFriends = TestHelper.GenerateRandomInt();

            _fakeUserQueryValidator.CallsTo(x => x.IsScreenNameValid(userScreenName)).Returns(false);
            _fakeUserQueryParameterGenerator.ArrangeGenerateScreenNameParameter();

            // Act
            var result = queryGenerator.GetFriendIdsQuery(userScreenName, maximumNumberOfFriends);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetFriendIdsQuery_WithValidUserId_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userId = TestHelper.GenerateRandomLong();
            var maximumNumberOfFriends = TestHelper.GenerateRandomInt();
            var userIdParameter = UserQueryGeneratorHelper.GenerateParameterExpectedResult(userId);

            _fakeUserQueryValidator.CallsTo(x => x.IsUserIdValid(userId)).Returns(true);
            _fakeUserQueryParameterGenerator.ArrangeGenerateIdParameter();

            // Act
            var result = queryGenerator.GetFriendIdsQuery(userId, maximumNumberOfFriends);

            // Assert
            var expectedResult = string.Format(Resources.User_GetFriends, userIdParameter, maximumNumberOfFriends);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetFriendIdsQuery_WithInValidUserId_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userId = TestHelper.GenerateRandomLong();
            var maximumNumberOfFriends = TestHelper.GenerateRandomInt();

            _fakeUserQueryValidator.CallsTo(x => x.IsUserIdValid(userId)).Returns(false);
            _fakeUserQueryParameterGenerator.ArrangeGenerateIdParameter();

            // Act
            var result = queryGenerator.GetFriendIdsQuery(userId, maximumNumberOfFriends);

            // Assert
            Assert.AreEqual(result, null);
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
        }

        [TestMethod]
        public void GetFollowerIdsQuery_WithInValidUserDTO_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userDTO = GenerateUserDTO(false);
            var maximumNumberOfFollowers = TestHelper.GenerateRandomInt();

            // Act
            var result = queryGenerator.GetFollowerIdsQuery(userDTO, maximumNumberOfFollowers);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetFollowerIdsQuery_WithValidUserScreenName_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userScreenName = TestHelper.GenerateString();
            var maximumNumberOfFollowers = TestHelper.GenerateRandomInt();
            var userIdParameter = UserQueryGeneratorHelper.GenerateParameterExpectedResult(userScreenName);

            _fakeUserQueryValidator.CallsTo(x => x.IsScreenNameValid(userScreenName)).Returns(true);
            _fakeUserQueryParameterGenerator.ArrangeGenerateScreenNameParameter();

            // Act
            var result = queryGenerator.GetFollowerIdsQuery(userScreenName, maximumNumberOfFollowers);

            // Assert
            var expectedResult = string.Format(Resources.User_GetFollowers, userIdParameter, maximumNumberOfFollowers);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetFollowerIdsQuery_WithInValidUserScreenName_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userScreenName = TestHelper.GenerateString();
            var maximumNumberOfFollowers = TestHelper.GenerateRandomInt();

            _fakeUserQueryValidator.CallsTo(x => x.IsScreenNameValid(userScreenName)).Returns(false);
            _fakeUserQueryParameterGenerator.ArrangeGenerateScreenNameParameter();

            // Act
            var result = queryGenerator.GetFollowerIdsQuery(userScreenName, maximumNumberOfFollowers);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetFollowerIdsQuery_WithValidUserId_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userId = TestHelper.GenerateRandomLong();
            var maximumNumberOfFollowers = TestHelper.GenerateRandomInt();
            var userIdParameter = UserQueryGeneratorHelper.GenerateParameterExpectedResult(userId);

            _fakeUserQueryValidator.CallsTo(x => x.IsUserIdValid(userId)).Returns(true);
            _fakeUserQueryParameterGenerator.ArrangeGenerateIdParameter();

            // Act
            var result = queryGenerator.GetFollowerIdsQuery(userId, maximumNumberOfFollowers);

            // Assert
            var expectedResult = string.Format(Resources.User_GetFollowers, userIdParameter, maximumNumberOfFollowers);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetFollowerIdsQuery_WithInValidUserId_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userId = TestHelper.GenerateRandomLong();
            var maximumNumberOfFollowers = TestHelper.GenerateRandomInt();

            _fakeUserQueryValidator.CallsTo(x => x.IsUserIdValid(userId)).Returns(false);
            _fakeUserQueryParameterGenerator.ArrangeGenerateIdParameter();

            // Act
            var result = queryGenerator.GetFollowerIdsQuery(userId, maximumNumberOfFollowers);

            // Assert
            Assert.AreEqual(result, null);
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
            parameters.UserIdentifier = new UserIdentifier(userIdParameter);
            parameters.Parameters.MaximumNumberOfTweetsToRetrieve = maximumNumberOfFavourites;
            parameters.Parameters.IncludeEntities = true;

            // Act
            var result = queryGenerator.GetFavoriteTweetsQuery(parameters);

            // Assert
            var expectedResult = string.Format("{0}user_id={1}", Resources.User_GetFavourites, userIdParameter);
            Assert.IsTrue(result.StartsWith(Resources.User_GetFavourites));
            Assert.IsTrue(result.Contains("count=" + maximumNumberOfFavourites));
            Assert.IsTrue(result.Contains("include_entities=true"));
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
        }

        [TestMethod]
        public void GetBlockUserQuery_WithInValidUserDTO_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userDTO = GenerateUserDTO(false);

            // Act
            var result = queryGenerator.GetBlockUserQuery(userDTO);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetBlockUserQuery_WithValidUserScreenName_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userScreenName = TestHelper.GenerateString();
            var userIdParameter = UserQueryGeneratorHelper.GenerateParameterExpectedResult(userScreenName);

            _fakeUserQueryValidator.CallsTo(x => x.IsScreenNameValid(userScreenName)).Returns(true);
            _fakeUserQueryParameterGenerator.ArrangeGenerateScreenNameParameter();

            // Act
            var result = queryGenerator.GetBlockUserQuery(userScreenName);

            // Assert
            var expectedResult = string.Format(Resources.User_Block_Create, userIdParameter);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetBlockUserQuery_WithInValidUserScreenName_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userScreenName = TestHelper.GenerateString();

            _fakeUserQueryValidator.CallsTo(x => x.IsScreenNameValid(userScreenName)).Returns(false);
            _fakeUserQueryParameterGenerator.ArrangeGenerateScreenNameParameter();

            // Act
            var result = queryGenerator.GetBlockUserQuery(userScreenName);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetBlockUserQuery_WithValidUserId_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userId = TestHelper.GenerateRandomLong();
            var userIdParameter = UserQueryGeneratorHelper.GenerateParameterExpectedResult(userId);

            _fakeUserQueryValidator.CallsTo(x => x.IsUserIdValid(userId)).Returns(true);
            _fakeUserQueryParameterGenerator.ArrangeGenerateIdParameter();

            // Act
            var result = queryGenerator.GetBlockUserQuery(userId);

            // Assert
            var expectedResult = string.Format(Resources.User_Block_Create, userIdParameter);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetBlockUserQuery_WithInValidUserId_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userId = TestHelper.GenerateRandomLong();

            _fakeUserQueryValidator.CallsTo(x => x.IsUserIdValid(userId)).Returns(false);
            _fakeUserQueryParameterGenerator.ArrangeGenerateIdParameter();

            // Act
            var result = queryGenerator.GetBlockUserQuery(userId);

            // Assert
            Assert.AreEqual(result, null);
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
        }

        [TestMethod]
        public void GetReportUserForSpamQuery_WithInValidUserDTO_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userDTO = GenerateUserDTO(false);

            // Act
            var result = queryGenerator.GetReportUserForSpamQuery(userDTO);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetReportUserForSpamQuery_WithValidUserScreenName_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userScreenName = TestHelper.GenerateString();
            var userIdParameter = UserQueryGeneratorHelper.GenerateParameterExpectedResult(userScreenName);

            _fakeUserQueryValidator.CallsTo(x => x.IsScreenNameValid(userScreenName)).Returns(true);
            _fakeUserQueryParameterGenerator.ArrangeGenerateScreenNameParameter();

            // Act
            var result = queryGenerator.GetReportUserForSpamQuery(userScreenName);

            // Assert
            var expectedResult = string.Format(Resources.User_Report_Spam, userIdParameter);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetReportUserForSpamQuery_WithInValidUserScreenName_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userScreenName = TestHelper.GenerateString();

            _fakeUserQueryValidator.CallsTo(x => x.IsScreenNameValid(userScreenName)).Returns(false);
            _fakeUserQueryParameterGenerator.ArrangeGenerateScreenNameParameter();

            // Act
            var result = queryGenerator.GetReportUserForSpamQuery(userScreenName);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetReportUserForSpamQuery_WithValidUserId_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userId = TestHelper.GenerateRandomLong();
            var userIdParameter = UserQueryGeneratorHelper.GenerateParameterExpectedResult(userId);

            _fakeUserQueryValidator.CallsTo(x => x.IsUserIdValid(userId)).Returns(true);
            _fakeUserQueryParameterGenerator.ArrangeGenerateIdParameter();

            // Act
            var result = queryGenerator.GetReportUserForSpamQuery(userId);

            // Assert
            var expectedResult = string.Format(Resources.User_Report_Spam, userIdParameter);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetReportUserForSpamQuery_WithInValidUserId_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var userId = TestHelper.GenerateRandomLong();

            _fakeUserQueryValidator.CallsTo(x => x.IsUserIdValid(userId)).Returns(false);
            _fakeUserQueryParameterGenerator.ArrangeGenerateIdParameter();

            // Act
            var result = queryGenerator.GetReportUserForSpamQuery(userId);

            // Assert
            Assert.AreEqual(result, null);
        }

        #endregion

        private IUserDTO GenerateUserDTO(bool isValid)
        {
            var userDTO = A.Fake<IUserDTO>();
            _fakeUserQueryValidator.CallsTo(x => x.CanUserBeIdentified(userDTO)).Returns(isValid);
            _fakeUserQueryParameterGenerator.ArrangeGenerateIdOrScreenNameParameter();
            return userDTO;
        }

        public UserQueryGenerator CreateUserQueryGenerator()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}