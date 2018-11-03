using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Timeline;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Testinvi.TweetinviControllers.TimelineTests
{
    [TestClass]
    public class TimelineJsonControllerTests
    {
        private FakeClassBuilder<TimelineJsonController> _fakeBuilder;
        private ITimelineQueryGenerator _timelineQueryGenerator;
        private ITwitterAccessor _twitterAccessor;
        private IUserFactory _userFactory;
        private ITimelineQueryParameterGenerator _timelineQueryParameterGenerator;

        private IHomeTimelineParameters _homeTimelineParameters;
        private IUserTimelineParameters _userTimelineParameters;
        private IMentionsTimelineParameters _mentionsTimelineParameters;

        private IUserTimelineQueryParameters _userTimelineQueryParameters;

        private int _maximuNumberOfTweets;
        private string _expectedQuery;
        private string _expectedResult;
        private string _userName;
        private long _userId;

        private IUser _user;
        private IUserDTO _userDTO;
        private IUserIdentifier _userIdentifier;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<TimelineJsonController>();
            _timelineQueryGenerator = _fakeBuilder.GetFake<ITimelineQueryGenerator>().FakedObject;
            _twitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>().FakedObject;
            _userFactory = _fakeBuilder.GetFake<IUserFactory>().FakedObject;
            _timelineQueryParameterGenerator = _fakeBuilder.GetFake<ITimelineQueryParameterGenerator>().FakedObject;
        
            InitData();

            A.CallTo(() => _twitterAccessor.ExecuteGETQueryReturningJson(_expectedQuery)).Returns(_expectedResult);
            
            A.CallTo(() => _userFactory.GenerateUserIdentifierFromScreenName(_userName)).Returns(_userIdentifier);
            A.CallTo(() => _userFactory.GenerateUserIdentifierFromId(_userId)).Returns(_userIdentifier);

            A.CallTo(() =>
                    _timelineQueryParameterGenerator.CreateUserTimelineQueryParameters(It.IsAny<IUserIdentifier>(),
                        It.IsAny<IUserTimelineParameters>()))
                .Returns(_userTimelineQueryParameters);

            A.CallTo(() => _timelineQueryParameterGenerator.CreateHomeTimelineParameters())
                .Returns(_homeTimelineParameters);
            A.CallTo(() => _timelineQueryParameterGenerator.CreateUserTimelineParameters())
                .Returns(_userTimelineParameters);
            A.CallTo(() => _timelineQueryParameterGenerator.CreateMentionsTimelineParameters())
                .Returns(_mentionsTimelineParameters);
        }

        private void InitData()
        {
            _homeTimelineParameters = A.Fake<IHomeTimelineParameters>();
            _userTimelineParameters = A.Fake<IUserTimelineParameters>();
            _mentionsTimelineParameters = A.Fake<IMentionsTimelineParameters>();

            _userTimelineQueryParameters = A.Fake<IUserTimelineQueryParameters>();

            _maximuNumberOfTweets = TestHelper.GenerateRandomInt();
            _expectedQuery = TestHelper.GenerateString();
            _expectedResult = TestHelper.GenerateString();
            _userName = TestHelper.GenerateString();
            _userId = TestHelper.GenerateRandomLong();

            _user = A.Fake<IUser>();
            _userDTO = A.Fake<IUserDTO>();
            _userIdentifier = _userDTO;
        }

        #region GetHomeTimeline

        [TestMethod]
        public void GetHomeTimeline_FromMaximumNumber_ReturnsExpectedResult()
        {
            // Arrange
            var controller = CreateTimelineJsonController();

            A.CallTo(() => _timelineQueryGenerator.GetHomeTimelineQuery(_homeTimelineParameters))
                .Returns(_expectedQuery);

            // Act
            var result = controller.GetHomeTimeline(_maximuNumberOfTweets);

            // Assert
            Assert.AreEqual(result, _expectedResult);
            Assert.AreEqual(_homeTimelineParameters.MaximumNumberOfTweetsToRetrieve, _maximuNumberOfTweets);
        }

        [TestMethod]
        public void GetHomeTimeline_FromTimelineRequestParameter_ReturnsExpectedResult()
        {
            // Arrange
            var controller = CreateTimelineJsonController();

            A.CallTo(() => _timelineQueryGenerator.GetHomeTimelineQuery(_homeTimelineParameters))
                .Returns(_expectedQuery);

            // Act
            var result = controller.GetHomeTimeline(_homeTimelineParameters);

            // Assert
            Assert.AreEqual(result, _expectedResult);
        }

        #endregion

        #region GetMentionsTimeline

        [TestMethod]
        public void GetMentionsTimeline_ExcludeRepliesIsTrue_ReturnsTAResult()
        {
            // Arrange
            var controller = CreateTimelineJsonController();
            var maximumTweets = TestHelper.GenerateRandomInt();
            var expectedQuery = TestHelper.GenerateString();
            var expectedResult = GetQueryResult(expectedQuery);

            A.CallTo(() => _timelineQueryGenerator.GetMentionsTimelineQuery(_mentionsTimelineParameters))
                .Returns(expectedQuery);

            // Act
            var result = controller.GetMentionsTimeline(maximumTweets);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetMentionsTimeline_ExcludeRepliesIsFalse_ReturnsTAResult()
        {
            // Arrange
            var controller = CreateTimelineJsonController();
            var maximumTweets = TestHelper.GenerateRandomInt();
            var expectedQuery = TestHelper.GenerateString();
            var expectedResult = GetQueryResult(expectedQuery);

            A.CallTo(() => _timelineQueryGenerator.GetMentionsTimelineQuery(_mentionsTimelineParameters))
                .Returns(expectedQuery);

            // Act
            var result = controller.GetMentionsTimeline(maximumTweets);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region GetUserTimeline

        [TestMethod]
        public void GetUserTimelineWithUser_FromUser_ReturnsResult()
        {
            // Arrange
            var controller = CreateTimelineJsonController();

            A.CallTo(() => _timelineQueryGenerator.GetUserTimelineQuery(_userTimelineQueryParameters))
                .Returns(_expectedQuery);

            // Act
            var result = controller.GetUserTimeline(_user, _maximuNumberOfTweets);

            // Assert
            Assert.AreEqual(result, _expectedResult);
        }

        [TestMethod]
        public void GetUserTimelineWithUser_FromUserDTO_ReturnsResult()
        {
            // Arrange
            var controller = CreateTimelineJsonController();

            A.CallTo(() => _timelineQueryGenerator.GetUserTimelineQuery(_userTimelineQueryParameters))
                .Returns(_expectedQuery);

            // Act
            var result = controller.GetUserTimeline(_userDTO, _maximuNumberOfTweets);

            // Assert
            Assert.AreEqual(result, _expectedResult);
        }

        [TestMethod]
        public void GetUserTimelineWithUser_FromUserName_ReturnsResult()
        {
            // Arrange
            var controller = CreateTimelineJsonController();

            A.CallTo(() => _timelineQueryGenerator.GetUserTimelineQuery(_userTimelineQueryParameters))
                .Returns(_expectedQuery);

            // Act
            var result = controller.GetUserTimeline(_userName, _maximuNumberOfTweets);

            // Assert
            Assert.AreEqual(result, _expectedResult);
        }

        [TestMethod]
        public void GetUserTimelineWithUser_FromUserId_ReturnsResult()
        {
            // Arrange
            var controller = CreateTimelineJsonController();

            A.CallTo(() => _timelineQueryGenerator.GetUserTimelineQuery(_userTimelineQueryParameters))
                .Returns(_expectedQuery);

            // Act
            var result = controller.GetUserTimeline(_userId, _maximuNumberOfTweets);

            // Assert
            Assert.AreEqual(result, _expectedResult);
        }

        [TestMethod]
        public void GetUserTimeline_FromTimelineRequestParameter_ReturnsExpectedResult()
        {
            // Arrange
            var controller = CreateTimelineJsonController();

            A.CallTo(() => _timelineQueryGenerator.GetUserTimelineQuery(_userTimelineQueryParameters))
                .Returns(_expectedQuery);

            // Act
            var result = controller.GetUserTimeline(_userTimelineQueryParameters);

            // Assert
            Assert.AreEqual(result, _expectedResult);
        }

        #endregion

        private string GetQueryResult(string query)
        {
            var result = TestHelper.GenerateString();
            _twitterAccessor.ArrangeExecuteJsonGETQuery(query, result);
            return result;
        }

        public TimelineJsonController CreateTimelineJsonController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}