using System.Collections.Generic;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Timeline;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Testinvi.TweetinviControllers.TimelineTests
{
    [TestClass]
    public class TimelineQueryExecutorTests
    {
        private FakeClassBuilder<TimelineQueryExecutor> _fakeBuilder;

        private ITimelineQueryGenerator _timelineQueryGenerator;
        private ITwitterAccessor _twitterAccessor;
        private IHomeTimelineParameters _homeTimelineParameters;
        private IUserTimelineQueryParameters _userTimelineQueryParameters;
        private IMentionsTimelineParameters _mentionsTimelineParameters;

        private string _expectedQuery;
        private IEnumerable<ITweetDTO> _expectedResult;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<TimelineQueryExecutor>();
            _timelineQueryGenerator = _fakeBuilder.GetFake<ITimelineQueryGenerator>().FakedObject;
            _twitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>().FakedObject;

            InitData();

            // Do not initialize the query result for the different timeline from the ctor as it could false the result
            // Do not do : _timelineQueryGenerator.CallsTo(x => x.GetUserTimelineQuery(_fakeUserTimelineRequestParameters)).Returns(_expectedQuery);
        }

        private void InitData()
        {
            _expectedQuery = TestHelper.GenerateString();
            _expectedResult = GetQueryResult<IEnumerable<ITweetDTO>>(_expectedQuery);

            _homeTimelineParameters = A.Fake<IHomeTimelineParameters>();
            _userTimelineQueryParameters = A.Fake<IUserTimelineQueryParameters>();
            _mentionsTimelineParameters = A.Fake<IMentionsTimelineParameters>();
        }

        #region GetHomeTimeline

        [TestMethod]
        public void GetHomeTimeline_FromHomeTimelineRequest_ReturnsExpectedResult()
        {
            // Arrange
            var queryExecutor = CreateTimelineQueryExecutor();

            A.CallTo(() => _timelineQueryGenerator.GetHomeTimelineQuery(_homeTimelineParameters))
                .Returns(_expectedQuery);

            // Act
            var result = queryExecutor.GetHomeTimeline(_homeTimelineParameters);

            // Assert
            Assert.AreEqual(result, _expectedResult);
        }

        #endregion

        #region GetMentionsTimeline

        [TestMethod]
        public void GetMentionsTimeline_FromHomeTimelineRequest_ReturnsExpectedResult()
        {
            // Arrange
            var queryExecutor = CreateTimelineQueryExecutor();

            A.CallTo(() => _timelineQueryGenerator.GetMentionsTimelineQuery(_mentionsTimelineParameters))
                .Returns(_expectedQuery);

            // Act
            var result = queryExecutor.GetMentionsTimeline(_mentionsTimelineParameters);

            // Assert
            Assert.AreEqual(result, _expectedResult);
        }

        #endregion

        #region GetUserTimeline

        [TestMethod]
        public void GetUserTimeline_FromUserTimelineRequest_ReturnsExpectedResult()
        {
            // Arrange
            var queryExecutor = CreateTimelineQueryExecutor();

            A.CallTo(() => _timelineQueryGenerator.GetUserTimelineQuery(_userTimelineQueryParameters))
                .Returns(_expectedQuery);

            // Act
            var result = queryExecutor.GetUserTimeline(_userTimelineQueryParameters);

            // Assert
            Assert.AreEqual(result, _expectedResult);
        }

        #endregion

        private T GetQueryResult<T>(string query) where T : class
        {
            var result = A.Fake<T>();
            _twitterAccessor.ArrangeExecuteGETQuery(query, result);
            return result;
        }

        public TimelineQueryExecutor CreateTimelineQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}