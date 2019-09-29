using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi;
using Tweetinvi.Client;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Models;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Testinvi.TweetinviLogic
{
    [TestClass]
    public class AuthenticatedUserTests
    {
        private FakeClassBuilder<AuthenticatedUser> _fakeBuilder;
        private Fake<ICredentialsAccessor> _fakeCredentialsAccessor;
        private Fake<ITimelineController> _fakeTimelineController;
        private Fake<ISavedSearchController> _fakeSavedSearchController;
        private Fake<IMessageController> _fakeMessageController;
        private Fake<IAccountController> _fakeAccountController;
        private Fake<ITwitterClient> _twitterClient;
        private Fake<ITweetsClient> _tweetsClient;

        private IAuthenticatedUser _authenticatedUser;
        private ITwitterCredentials _authenticatedUserCredentials;
        private ITwitterCredentials _currentCredentials;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<AuthenticatedUser>();
            _fakeCredentialsAccessor = _fakeBuilder.GetFake<ICredentialsAccessor>();
            _fakeTimelineController = _fakeBuilder.GetFake<ITimelineController>();
            _fakeSavedSearchController = _fakeBuilder.GetFake<ISavedSearchController>();
            _fakeMessageController = _fakeBuilder.GetFake<IMessageController>();
            _fakeAccountController = _fakeBuilder.GetFake<IAccountController>();

            InitData();
        }

        #region GetHomeTimeline

        [TestMethod]
        public async Task GetHomeTimeline_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var nbTweets = TestHelper.GenerateRandomInt();

            ITwitterCredentials startOperationWithCredentials = null;
            _fakeTimelineController.CallsTo(x => x.GetHomeTimeline(nbTweets)).ReturnsLazily(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
                return A.Fake<IEnumerable<ITweet>>();
            });

            // Act
            await _authenticatedUser.GetHomeTimeline(nbTweets);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        }

        #endregion

        #region GetMentionsTimeline

        [TestMethod]
        public async Task GetMentionsTimeline_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var nbTweets = TestHelper.GenerateRandomInt();

            ITwitterCredentials startOperationWithCredentials = null;
            _fakeTimelineController.CallsTo(x => x.GetMentionsTimeline(nbTweets)).ReturnsLazily(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
                return A.Fake<IEnumerable<IMention>>();
            });

            // Act
            await _authenticatedUser.GetMentionsTimeline(nbTweets);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        }

        #endregion

        #region GetSavedSearches

        [TestMethod]
        public async Task GetSavedSearches_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            ITwitterCredentials startOperationWithCredentials = null;
            _fakeSavedSearchController.CallsTo(x => x.GetSavedSearches()).ReturnsLazily(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
                return A.Fake<IEnumerable<ISavedSearch>>();
            });

            // Act
            await _authenticatedUser.GetSavedSearches();

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        }

        #endregion

        #region GetLatestMessages

        [TestMethod]
        public async Task GetLatestMessages_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var nbMessages = TestHelper.GenerateRandomInt();

            ITwitterCredentials startOperationWithCredentials = null;
            _fakeMessageController.CallsTo(x => x.GetLatestMessages(nbMessages)).ReturnsLazily(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
                return A.Fake<AsyncCursorResult<IEnumerable<IMessage>>>();
            });

            // Act
            await _authenticatedUser.GetLatestMessages(nbMessages);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        }

        #endregion

        #region PublishMessage

        [TestMethod]
        public async Task PublishMessage_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var parameters = A.Fake<IPublishMessageParameters>();

            ITwitterCredentials startOperationWithCredentials = null;
            _fakeMessageController.CallsTo(x => x.PublishMessage(parameters)).ReturnsLazily(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
                return A.Fake<IMessage>();
            });

            // Act
            await _authenticatedUser.PublishMessage(parameters);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        }

        #endregion

        #region GetAccountSettings

        [TestMethod]
        public async Task GetAccountSettings_CurrentCredentialsAreNotAuthenticatedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var eventDTO = A.Fake<IMessageEventDTO>();
            var message = A.Fake<IMessage>();
            message.CallsTo(x => x.MessageEventDTO).Returns(eventDTO);

            ITwitterCredentials startOperationWithCredentials = null;
            _fakeAccountController.CallsTo(x => x.GetAuthenticatedUserSettings()).ReturnsLazily(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
                return A.Fake<IAccountSettings>();
            });

            // Act
            await _authenticatedUser.GetAccountSettings();

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _authenticatedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        }

        #endregion

        private void InitData()
        {
            _authenticatedUserCredentials = A.Fake<ITwitterCredentials>();
            _fakeCredentialsAccessor.CallsTo(x => x.CurrentThreadCredentials).Returns(_authenticatedUserCredentials);

            _twitterClient = new Fake<ITwitterClient>();
            _tweetsClient = new Fake<ITweetsClient>();
            _twitterClient.CallsTo(x => x.Tweets).Returns(_tweetsClient.FakedObject);

            _authenticatedUser = CreateAuthenticatedUser();

            _currentCredentials = A.Fake<ITwitterCredentials>();
            _fakeCredentialsAccessor.CallsTo(x => x.CurrentThreadCredentials).Returns(_currentCredentials);
        }

        private AuthenticatedUser CreateAuthenticatedUser()
        {
            var authenticatedUser = _fakeBuilder.GenerateClass();

            authenticatedUser.Client = _twitterClient.FakedObject;

            return authenticatedUser;
        }
    }
}