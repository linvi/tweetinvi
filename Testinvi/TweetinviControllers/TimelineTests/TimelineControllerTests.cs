using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.Timeline;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;
// ReSharper disable CollectionNeverUpdated.Local

namespace Testinvi.TweetinviControllers.TimelineTests
{
    [TestClass]
    public class TimelineControllerTests
    {
        private FakeClassBuilder<TimelineController> _fakeBuilder;
        private Fake<ITweetFactory> _fakeTweetFactory;
        private Fake<IUserFactory> _fakeUserFactory;
        private Fake<ITimelineQueryExecutor> _fakeTimelineQueryExecutor;
        private Fake<ITimelineQueryParameterGenerator> _fakeTimelineQueryParameterGenerator;

        private IHomeTimelineParameters _fakeHomeTimelineParameters;
        private IUserTimelineParameters _fakeUserTimelineParameters;
        private IMentionsTimelineParameters _fakeMentionsTimelineParameters;

        private IUserTimelineQueryParameters _fakeUserTimelineQueryParameters;

        private int _maximumNumberOfTweets;
        private ITweetDTO[] _resultDTO;
        private ITweet[] _result;
        private string _userName;
        private long _userId;

        private IUser _fakeUser;
        private IUserDTO _fakeUserDTO;
        private IUserIdentifier _fakeUserIdentifier;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<TimelineController>();
            _fakeTweetFactory = _fakeBuilder.GetFake<ITweetFactory>();
            _fakeUserFactory = _fakeBuilder.GetFake<IUserFactory>();
            _fakeTimelineQueryExecutor = _fakeBuilder.GetFake<ITimelineQueryExecutor>();
            _fakeTimelineQueryParameterGenerator = _fakeBuilder.GetFake<ITimelineQueryParameterGenerator>();

            InitData();

            _fakeUserFactory.CallsTo(x => x.GenerateUserIdentifierFromScreenName(_userName)).Returns(_fakeUserIdentifier);
            _fakeUserFactory.CallsTo(x => x.GenerateUserIdentifierFromId(_userId)).Returns(_fakeUserIdentifier);

            _fakeTimelineQueryParameterGenerator.CallsTo(x => x.CreateUserTimelineQueryParameters(_fakeUser, _fakeUserTimelineParameters)).Returns(_fakeUserTimelineQueryParameters);
            _fakeTimelineQueryParameterGenerator.CallsTo(x => x.CreateUserTimelineQueryParameters(_fakeUserIdentifier, _fakeUserTimelineParameters)).Returns(_fakeUserTimelineQueryParameters);
            _fakeTimelineQueryParameterGenerator.CallsTo(x => x.CreateUserTimelineParameters()).Returns(_fakeUserTimelineParameters);
            _fakeTimelineQueryParameterGenerator.CallsTo(x => x.CreateMentionsTimelineParameters()).Returns(_fakeMentionsTimelineParameters);
        }

        private void InitData()
        {
            _fakeHomeTimelineParameters = A.Fake<IHomeTimelineParameters>();
            _fakeUserTimelineParameters = A.Fake<IUserTimelineParameters>();
            _fakeMentionsTimelineParameters = A.Fake<IMentionsTimelineParameters>();

            _fakeUserTimelineQueryParameters = A.Fake<IUserTimelineQueryParameters>();

            _maximumNumberOfTweets = TestHelper.GenerateRandomInt();
            _resultDTO = new ITweetDTO[0];
            _result = new ITweet[0];
            _userName = TestHelper.GenerateString();
            _userId = TestHelper.GenerateRandomLong();

            _fakeUser = A.Fake<IUser>();
            _fakeUserDTO = A.Fake<IUserDTO>();
            _fakeUserIdentifier = _fakeUserDTO;
        }

        #region Get HomeTimeline

        [TestMethod]
        public async Task GetHomeTimeline_WithTimelineRequestParameter_ReturnsGeneratedObjectsFromQueryExecutorDTOs()
        {
            // Arrange
            var controller = CreateTimelineController();

            _fakeTimelineQueryExecutor.CallsTo(x => x.GetHomeTimeline(_fakeHomeTimelineParameters)).Returns(_resultDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetsFromDTO(_resultDTO, null, null)).Returns(_result);

            // Act
            var result = await controller.GetHomeTimeline(_fakeHomeTimelineParameters);

            // Assert
            Assert.AreEqual(result, _result);
        }

        #endregion

        #region Get UserTimeline

        [TestMethod]
        public async Task GetUserTimelineWithUser_FromUser_ReturnsResult()
        {
            // Arrange
            var controller = CreateTimelineController();

            _fakeTimelineQueryExecutor.CallsTo(x => x.GetUserTimeline(_fakeUserTimelineQueryParameters)).Returns(_resultDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetsFromDTO(_resultDTO, null, null)).Returns(_result);

            // Act
            var result = await controller.GetUserTimeline(_fakeUser, _maximumNumberOfTweets);

            // Assert
            Assert.AreEqual(result, _result);
        }

        [TestMethod]
        public async Task GetUserTimelineWithUser_FromUserDTO_ReturnsResult()
        {
            // Arrange
            var controller = CreateTimelineController();

            _fakeTimelineQueryExecutor.CallsTo(x => x.GetUserTimeline(_fakeUserTimelineQueryParameters)).Returns(_resultDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetsFromDTO(_resultDTO, null, null)).Returns(_result);

            // Act
            var result = await controller.GetUserTimeline(_fakeUserDTO, _maximumNumberOfTweets);

            // Assert
            Assert.AreEqual(result, _result);
        }

        [TestMethod]
        public async Task GetUserTimelineWithUser_FromUserName_ReturnsResult()
        {
            // Arrange
            var controller = CreateTimelineController();

            _fakeTimelineQueryExecutor.CallsTo(x => x.GetUserTimeline(_fakeUserTimelineQueryParameters)).Returns(_resultDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetsFromDTO(_resultDTO, null, null)).Returns(_result);

            // Act
            var result = await controller.GetUserTimeline(_userName, _maximumNumberOfTweets);

            // Assert
            Assert.AreEqual(result, _result);
        }

        [TestMethod]
        public async Task GetUserTimelineWithUser_FromUserId_ReturnsResult()
        {
            // Arrange
            var controller = CreateTimelineController();

            _fakeTimelineQueryExecutor.CallsTo(x => x.GetUserTimeline(_fakeUserTimelineQueryParameters)).Returns(_resultDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetsFromDTO(_resultDTO, null, null)).Returns(_result);

            // Act
            var result = await controller.GetUserTimeline(_userId, _maximumNumberOfTweets);

            // Assert
            Assert.AreEqual(result, _result);
        }

        #endregion

        #region Get MentionsTimeline

        [TestMethod]
        public async Task VerifyGetMentionsTimeline_ExcludeReplies_ReturnsGeneratedObjectsFromQueryExecutorDTOs()
        {
            // Arrange
            var controller = CreateTimelineController();
            var nbTweetsLimit = TestHelper.GenerateRandomInt();
            var expectedDTOResult = new List<ITweetDTO>();
            var expectedResult = new List<IMention>();

            _fakeTimelineQueryExecutor.CallsTo(x => x.GetMentionsTimeline(_fakeMentionsTimelineParameters)).Returns(expectedDTOResult);
            _fakeTweetFactory.CallsTo(x => x.GenerateMentionsFromDTO(expectedDTOResult)).Returns(expectedResult);

            // Act
            var result = await controller.GetMentionsTimeline(nbTweetsLimit);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        private TimelineController CreateTimelineController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}