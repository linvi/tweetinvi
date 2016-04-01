using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Logic;

namespace Testinvi.TweetinviLogic
{
    [TestClass]
    public class AuthenticatedUserTests
    {
        private FakeClassBuilder<AuthenticatedUser> _fakeBuilder;
        private Fake<ICredentialsAccessor> _fakeCredentialsAccessor;
        private Fake<ITimelineController> _fakeTimelineController;
        private Fake<IFriendshipController> _fakeFriendshipController;
        private Fake<ISavedSearchController> _fakeSavedSearchController;
        private Fake<IMessageController> _fakeMessageController;
        private Fake<ITweetController> _fakeTweetController;
        private Fake<IAccountController> _fakeAccountController;

        private IAuthenticatedUser _authenticatedUser;
        private ITwitterCredentials _authenticatedUserCredentials;
        private ITwitterCredentials _currentCredentials;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<AuthenticatedUser>();
            _fakeCredentialsAccessor = _fakeBuilder.GetFake<ICredentialsAccessor>();
            _fakeTimelineController = _fakeBuilder.GetFake<ITimelineController>();
            _fakeFriendshipController = _fakeBuilder.GetFake<IFriendshipController>();
            _fakeSavedSearchController = _fakeBuilder.GetFake<ISavedSearchController>();
            _fakeMessageController = _fakeBuilder.GetFake<IMessageController>();
            _fakeTweetController = _fakeBuilder.GetFake<ITweetController>();
            _fakeAccountController = _fakeBuilder.GetFake<IAccountController>();

            InitData();
        }
        
        #region GetHomeTimeline

        [TestMethod]
        public void GetHomeTimeline_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var nbTweets = TestHelper.GenerateRandomInt();

            ITwitterCredentials startOperationWithCredentials = null;
            _fakeTimelineController.CallsTo(x => x.GetHomeTimeline(nbTweets)).Invokes(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
            });

            // Act
            _authenticatedUser.GetHomeTimeline(nbTweets);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        }
        
        #endregion

        #region GetMentionsTimeline

        [TestMethod]
        public void GetMentionsTimeline_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var nbTweets = TestHelper.GenerateRandomInt();

            ITwitterCredentials startOperationWithCredentials = null;
            _fakeTimelineController.CallsTo(x => x.GetMentionsTimeline(nbTweets)).Invokes(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
            });

            // Act
            _authenticatedUser.GetMentionsTimeline(nbTweets);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        }
        
        #endregion

        #region GetUsersRequestingFriendship
        
        [TestMethod]
        public void GetUsersRequestingFriendship_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            ITwitterCredentials startOperationWithCredentials = null;
            var max = TestHelper.GenerateRandomInt();

            _fakeFriendshipController.CallsTo(x => x.GetUsersRequestingFriendship(max)).Invokes(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
            });

            // Act
            _authenticatedUser.GetUsersRequestingFriendship(max);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        } 

        #endregion

        #region GetUsersYouRequestedToFollow
        
        [TestMethod]
        public void GetUsersYouRequestedToFollow_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            ITwitterCredentials startOperationWithCredentials = null;
            var max = TestHelper.GenerateRandomInt();

            _fakeFriendshipController.CallsTo(x => x.GetUsersYouRequestedToFollow(max)).Invokes(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
            });

            // Act
            _authenticatedUser.GetUsersYouRequestedToFollow(max);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        } 

        #endregion

        #region FollowUser

        [TestMethod]
        public void FollowUser_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var user = A.Fake<IUser>();

            ITwitterCredentials startOperationWithCredentials = null;
            _fakeFriendshipController.CallsTo(x => x.CreateFriendshipWith(user)).ReturnsLazily(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
                return true;
            });
          
            // Act
            _authenticatedUser.FollowUser(user);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        } 

        #endregion

        #region FollowUser

        [TestMethod]
        public void UnFollowUser_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var user = A.Fake<IUser>();

            ITwitterCredentials startOperationWithCredentials = null;
            _fakeFriendshipController.CallsTo(x => x.DestroyFriendshipWith(user)).ReturnsLazily(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
                return true;
            });

            // Act
            _authenticatedUser.UnFollowUser(user);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        }

        #endregion

        #region UpdateRelationshipAuthorizationsWith

        [TestMethod]
        public void UpdateRelationshipAuthorizationsWith_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var user = A.Fake<IUser>();

            ITwitterCredentials startOperationWithCredentials = null;
            _fakeFriendshipController.CallsTo(x => x.UpdateRelationshipAuthorizationsWith((IUserIdentifier)user, true, true)).ReturnsLazily(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
                return true;
            });

            // Act
            _authenticatedUser.UpdateRelationshipAuthorizationsWith(user, true, true);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        }

        #endregion

        #region GetSavedSearches

        [TestMethod]
        public void GetSavedSearches_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            ITwitterCredentials startOperationWithCredentials = null;
            _fakeSavedSearchController.CallsTo(x => x.GetSavedSearches()).Invokes(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
            });

            // Act
            _authenticatedUser.GetSavedSearches();

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        }

        #endregion

        #region GetLatestMessagesReceived

        [TestMethod]
        public void GetLatestMessagesReceived_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var nbMessages = TestHelper.GenerateRandomInt();

            ITwitterCredentials startOperationWithCredentials = null;
            _fakeMessageController.CallsTo(x => x.GetLatestMessagesReceived(nbMessages)).Invokes(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
            });

            // Act
            _authenticatedUser.GetLatestMessagesReceived(nbMessages);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        }

        #endregion

        #region GetLatestMessagesSent

        [TestMethod]
        public void GetLatestMessagesSent_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var nbMessages = TestHelper.GenerateRandomInt();

            ITwitterCredentials startOperationWithCredentials = null;
            _fakeMessageController.CallsTo(x => x.GetLatestMessagesSent(nbMessages)).Invokes(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
            });

            // Act
            _authenticatedUser.GetLatestMessagesSent(nbMessages);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        }

        #endregion

        #region PublishMessage

        [TestMethod]
        public void PublishMessage_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var parameters = A.Fake<IPublishMessageParameters>();

            ITwitterCredentials startOperationWithCredentials = null;
            _fakeMessageController.CallsTo(x => x.PublishMessage(parameters)).Invokes(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
            });

            // Act
            _authenticatedUser.PublishMessage(parameters);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
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
            _fakeTweetController.CallsTo(x => x.PublishTweet(tweetText, parameters)).Invokes(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
            });

            // Act
            _authenticatedUser.PublishTweet(tweetText, parameters);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        }

        #endregion

        #region GetAccountSettings

        [TestMethod]
        public void GetAccountSettings_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var messageDTO = A.Fake<IMessageDTO>();
            var message = A.Fake<IMessage>();
            message.CallsTo(x => x.MessageDTO).Returns(messageDTO);

            ITwitterCredentials startOperationWithCredentials = null;
            _fakeAccountController.CallsTo(x => x.GetAuthenticatedUserSettings()).Invokes(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
            });

            // Act
            _authenticatedUser.GetAccountSettings();

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        }

        #endregion

        private void InitData()
        {
            _authenticatedUserCredentials = A.Fake<ITwitterCredentials>();
            _fakeCredentialsAccessor.CallsTo(x => x.CurrentThreadCredentials).Returns(_authenticatedUserCredentials);
            _authenticatedUser = CreateAuthenticatedUser();
            
            _currentCredentials = A.Fake<ITwitterCredentials>();
            _fakeCredentialsAccessor.CallsTo(x => x.CurrentThreadCredentials).Returns(_currentCredentials);
        }

        public AuthenticatedUser CreateAuthenticatedUser()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}