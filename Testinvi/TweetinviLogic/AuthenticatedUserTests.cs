using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Logic;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Testinvi.TweetinviLogic
{
    [TestClass]
    public class AuthenticatedUserTests
    {
        private FakeClassBuilder<AuthenticatedUser> _fakeBuilder;

        private ICredentialsAccessor _credentialsAccessor;
        private ITimelineController _timelineController;
        private IFriendshipController _friendshipController;
        private ISavedSearchController _savedSearchController;
        private IMessageController _messageController;
        private ITweetController _tweetController;
        private IAccountController _accountController;
        private IAuthenticatedUser _authenticatedUser;
        private ITwitterCredentials _authenticatedUserCredentials;
        private ITwitterCredentials _currentCredentials;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<AuthenticatedUser>();
            _credentialsAccessor = _fakeBuilder.GetFake<ICredentialsAccessor>().FakedObject;
            _timelineController = _fakeBuilder.GetFake<ITimelineController>().FakedObject;
            _friendshipController = _fakeBuilder.GetFake<IFriendshipController>().FakedObject;
            _savedSearchController = _fakeBuilder.GetFake<ISavedSearchController>().FakedObject;
            _messageController = _fakeBuilder.GetFake<IMessageController>().FakedObject;
            _tweetController = _fakeBuilder.GetFake<ITweetController>().FakedObject;
            _accountController = _fakeBuilder.GetFake<IAccountController>().FakedObject;

            InitData();
        }
        
        #region GetHomeTimeline

        [TestMethod]
        public void GetHomeTimeline_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var nbTweets = TestHelper.GenerateRandomInt();

            ITwitterCredentials startOperationWithCredentials = null;
            A.CallTo(() => _timelineController.GetHomeTimeline(nbTweets)).Invokes(() =>
            {
                startOperationWithCredentials = _credentialsAccessor.CurrentThreadCredentials;
            });

            // Act
            _authenticatedUser.GetHomeTimeline(nbTweets);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_credentialsAccessor.CurrentThreadCredentials, _currentCredentials);
        }
        
        #endregion

        #region GetMentionsTimeline

        [TestMethod]
        public void GetMentionsTimeline_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var nbTweets = TestHelper.GenerateRandomInt();

            ITwitterCredentials startOperationWithCredentials = null;
            A.CallTo(() => _timelineController.GetMentionsTimeline(nbTweets)).Invokes(() =>
            {
                startOperationWithCredentials = _credentialsAccessor.CurrentThreadCredentials;
            });

            // Act
            _authenticatedUser.GetMentionsTimeline(nbTweets);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_credentialsAccessor.CurrentThreadCredentials, _currentCredentials);
        }
        
        #endregion

        #region GetUsersRequestingFriendship
        
        [TestMethod]
        public void GetUsersRequestingFriendship_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            ITwitterCredentials startOperationWithCredentials = null;
            var max = TestHelper.GenerateRandomInt();

            A.CallTo(() => _friendshipController.GetUsersRequestingFriendship(max)).Invokes(() =>
            {
                startOperationWithCredentials = _credentialsAccessor.CurrentThreadCredentials;
            });

            // Act
            _authenticatedUser.GetUsersRequestingFriendship(max);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_credentialsAccessor.CurrentThreadCredentials, _currentCredentials);
        } 

        #endregion

        #region GetUsersYouRequestedToFollow
        
        [TestMethod]
        public void GetUsersYouRequestedToFollow_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            ITwitterCredentials startOperationWithCredentials = null;
            var max = TestHelper.GenerateRandomInt();

            A.CallTo(() => _friendshipController.GetUsersYouRequestedToFollow(max)).Invokes(() =>
            {
                startOperationWithCredentials = _credentialsAccessor.CurrentThreadCredentials;
            });

            // Act
            _authenticatedUser.GetUsersYouRequestedToFollow(max);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_credentialsAccessor.CurrentThreadCredentials, _currentCredentials);
        } 

        #endregion

        #region FollowUser

        [TestMethod]
        public void FollowUser_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var user = A.Fake<IUser>();

            ITwitterCredentials startOperationWithCredentials = null;
            A.CallTo(() => _friendshipController.CreateFriendshipWith(user)).ReturnsLazily(() =>
            {
                startOperationWithCredentials = _credentialsAccessor.CurrentThreadCredentials;
                return true;
            });
          
            // Act
            _authenticatedUser.FollowUser(user);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_credentialsAccessor.CurrentThreadCredentials, _currentCredentials);
        } 

        #endregion

        #region FollowUser

        [TestMethod]
        public void UnFollowUser_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var user = A.Fake<IUser>();

            ITwitterCredentials startOperationWithCredentials = null;
            A.CallTo(() => _friendshipController.DestroyFriendshipWith(user)).ReturnsLazily(() =>
            {
                startOperationWithCredentials = _credentialsAccessor.CurrentThreadCredentials;
                return true;
            });

            // Act
            _authenticatedUser.UnFollowUser(user);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_credentialsAccessor.CurrentThreadCredentials, _currentCredentials);
        }

        #endregion

        #region UpdateRelationshipAuthorizationsWith

        [TestMethod]
        public void UpdateRelationshipAuthorizationsWith_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var user = A.Fake<IUser>();

            ITwitterCredentials startOperationWithCredentials = null;
            A.CallTo(() => _friendshipController.UpdateRelationshipAuthorizationsWith(user, true, true)).ReturnsLazily(
                () =>
                {
                    startOperationWithCredentials = _credentialsAccessor.CurrentThreadCredentials;
                    return true;
                });

            // Act
            _authenticatedUser.UpdateRelationshipAuthorizationsWith(user, true, true);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_credentialsAccessor.CurrentThreadCredentials, _currentCredentials);
        }

        #endregion

        #region GetSavedSearches

        [TestMethod]
        public void GetSavedSearches_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            ITwitterCredentials startOperationWithCredentials = null;
            A.CallTo(() => _savedSearchController.GetSavedSearches()).Invokes(() =>
            {
                startOperationWithCredentials = _credentialsAccessor.CurrentThreadCredentials;
            });

            // Act
            _authenticatedUser.GetSavedSearches();

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_credentialsAccessor.CurrentThreadCredentials, _currentCredentials);
        }

        #endregion

        #region GetLatestMessages

        [TestMethod]
        public void GetLatestMessages_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var nbMessages = TestHelper.GenerateRandomInt();

            ITwitterCredentials startOperationWithCredentials = null;
            A.CallTo(() => _messageController.GetLatestMessages(nbMessages)).Invokes(() =>
            {
                startOperationWithCredentials = _credentialsAccessor.CurrentThreadCredentials;
            });

            // Act
            _authenticatedUser.GetLatestMessages(nbMessages);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_credentialsAccessor.CurrentThreadCredentials, _currentCredentials);
        }

        #endregion

        #region PublishMessage

        [TestMethod]
        public void PublishMessage_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var parameters = A.Fake<IPublishMessageParameters>();

            ITwitterCredentials startOperationWithCredentials = null;
            A.CallTo(() => _messageController.PublishMessage(parameters)).Invokes(() =>
            {
                startOperationWithCredentials = _credentialsAccessor.CurrentThreadCredentials;
            });

            // Act
            _authenticatedUser.PublishMessage(parameters);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_credentialsAccessor.CurrentThreadCredentials, _currentCredentials);
        }

        #endregion

        #region PublishTweet

        [TestMethod]
        public void PublishTweetText_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var tweetText = TestHelper.GenerateString();
            var parameters = A.Fake<IPublishTweetOptionalParameters>();

            ITwitterCredentials startOperationWithCredentials = null;
            A.CallTo(() => _tweetController.PublishTweet(tweetText, parameters)).Invokes(() =>
            {
                startOperationWithCredentials = _credentialsAccessor.CurrentThreadCredentials;
            });

            // Act
            _authenticatedUser.PublishTweet(tweetText, parameters);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_credentialsAccessor.CurrentThreadCredentials, _currentCredentials);
        }

        #endregion

        #region GetAccountSettings

        [TestMethod]
        public void GetAccountSettings_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var eventDTO = A.Fake<IEventDTO>();
            var message = A.Fake<IMessage>();
            A.CallTo(() => message.EventDTO).Returns(eventDTO);

            ITwitterCredentials startOperationWithCredentials = null;
            A.CallTo(() => _accountController.GetAuthenticatedUserSettings()).Invokes(() =>
            {
                startOperationWithCredentials = _credentialsAccessor.CurrentThreadCredentials;
            });

            // Act
            _authenticatedUser.GetAccountSettings();

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_credentialsAccessor.CurrentThreadCredentials, _currentCredentials);
        }

        #endregion

        private void InitData()
        {
            _authenticatedUserCredentials = A.Fake<ITwitterCredentials>();
            A.CallTo(() => _credentialsAccessor.CurrentThreadCredentials).Returns(_authenticatedUserCredentials);

            _currentCredentials = _authenticatedUserCredentials;
            A.CallTo(() => _credentialsAccessor.CurrentThreadCredentials).Returns(_currentCredentials);

            _authenticatedUser = CreateAuthenticatedUser();
        }

        public AuthenticatedUser CreateAuthenticatedUser()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}