using System;
using System.Text;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Controllers.Timeline;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Testinvi.TweetinviControllers.TimelineTests
{
    [TestClass]
    public class TimelineQueryGeneratorTests
    {
        private FakeClassBuilder<TimelineQueryGenerator> _fakeBuilder;

        private IUserQueryValidator _userQueryValidator;
        private IUserQueryParameterGenerator _userQueryParameterGenerator;
        private IQueryParameterGenerator _queryParameterGenerator;
        private ITimelineQueryParameterGenerator _timelineQueryParameterGenerator;
        private IHomeTimelineParameters _homeTimelineParameters;
        private IUserTimelineParameters _userTimelineParameters;
        private IUserTimelineQueryParameters _userTimelineQueryParameters;
        private IMentionsTimelineParameters _mentionsTimelineParameters;

        private int _maximumNumberOfTweetsParameterValue;

        private IUserIdentifier _userIdentifier;
        private string _userParameter;
        private string _includeRTSParameter;
        private string _excludeRepliesParameter;
        private string _includeContributorDetailsParameter;
        private string _maximumNumberOfTweetsParameter;
        private string _trimUserParameter;
        private string _sinceIdParameter;
        private string _maxIdParameter;
        private string _includeDetailsParameter;

        private string _expectedTimelineQuery;
        private string _expectedUserTimelineQuery;
        private string _expectedMentionsTimelineQuery;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<TimelineQueryGenerator>();
            _userQueryParameterGenerator = _fakeBuilder.GetFake<IUserQueryParameterGenerator>().FakedObject;
            _queryParameterGenerator = _fakeBuilder.GetFake<IQueryParameterGenerator>().FakedObject;
            _userQueryValidator = _fakeBuilder.GetFake<IUserQueryValidator>().FakedObject;
            
            _timelineQueryParameterGenerator = _fakeBuilder.GetFake<ITimelineQueryParameterGenerator>().FakedObject;

            Init();

            A.CallTo(() => _timelineQueryParameterGenerator.GenerateExcludeRepliesParameter(It.IsAny<bool>()))
                .Returns(_excludeRepliesParameter);
            A.CallTo(
                    () => _timelineQueryParameterGenerator.GenerateIncludeContributorDetailsParameter(It.IsAny<bool>()))
                .Returns(_includeContributorDetailsParameter);

            A.CallTo(() =>
                    _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(_userIdentifier, "user_id",
                        "screen_name"))
                .Returns(_userParameter);
            A.CallTo(() => _timelineQueryParameterGenerator.GenerateIncludeRTSParameter(It.IsAny<bool>()))
                .Returns(_includeRTSParameter);

            A.CallTo(() => _queryParameterGenerator.GenerateCountParameter(_maximumNumberOfTweetsParameterValue))
                .Returns(_maximumNumberOfTweetsParameter);
            A.CallTo(() => _queryParameterGenerator.GenerateTrimUserParameter(It.IsAny<bool>()))
                .Returns(_trimUserParameter);
            A.CallTo(() => _queryParameterGenerator.GenerateSinceIdParameter(It.IsAny<long>()))
                .Returns(_sinceIdParameter);
            A.CallTo(() => _queryParameterGenerator.GenerateMaxIdParameter(It.IsAny<long>())).Returns(_maxIdParameter);
            A.CallTo(() => _queryParameterGenerator.GenerateIncludeEntitiesParameter(It.IsAny<bool>()))
                .Returns(_includeDetailsParameter);
        }

        private void Init()
        {
            _maximumNumberOfTweetsParameterValue = TestHelper.GenerateRandomInt();
            _userIdentifier = A.Fake<IUserIdentifier>();

            _homeTimelineParameters = A.Fake<IHomeTimelineParameters>();
            A.CallTo(() => _homeTimelineParameters.MaximumNumberOfTweetsToRetrieve).Returns(_maximumNumberOfTweetsParameterValue);


            _userTimelineParameters = A.Fake<IUserTimelineParameters>();
            A.CallTo(() => _userTimelineParameters.MaximumNumberOfTweetsToRetrieve)
                .Returns(_maximumNumberOfTweetsParameterValue);
            
            _userTimelineQueryParameters = A.Fake<IUserTimelineQueryParameters>();
            A.CallTo(() => _userTimelineQueryParameters.Parameters).Returns(_userTimelineParameters);
            A.CallTo(() => _userTimelineQueryParameters.UserIdentifier).Returns(_userIdentifier);

            _mentionsTimelineParameters = A.Fake<IMentionsTimelineParameters>();
            A.CallTo(() => _mentionsTimelineParameters.MaximumNumberOfTweetsToRetrieve)
                .Returns(_maximumNumberOfTweetsParameterValue);

            _userParameter = TestHelper.GenerateString();
            _includeRTSParameter = TestHelper.GenerateString();
            _excludeRepliesParameter = TestHelper.GenerateString();
            _includeContributorDetailsParameter = TestHelper.GenerateString();
            _maximumNumberOfTweetsParameter = TestHelper.GenerateString();
            _trimUserParameter = TestHelper.GenerateString();
            _sinceIdParameter = TestHelper.GenerateString();
            _maxIdParameter = TestHelper.GenerateString();
            _includeDetailsParameter = TestHelper.GenerateString();

            var queryParameterBuilder = new StringBuilder();
            
            queryParameterBuilder.Append(_includeContributorDetailsParameter);
            queryParameterBuilder.Append(_maximumNumberOfTweetsParameter);
            queryParameterBuilder.Append(_trimUserParameter);
            queryParameterBuilder.Append(_sinceIdParameter);
            queryParameterBuilder.Append(_maxIdParameter);
            queryParameterBuilder.Append(_includeDetailsParameter);

            var homeQueryParameter = _excludeRepliesParameter + queryParameterBuilder;
            var userQueryParameter = _userParameter + _includeRTSParameter + _excludeRepliesParameter + queryParameterBuilder;

            _expectedTimelineQuery = string.Format(Resources.Timeline_GetHomeTimeline, homeQueryParameter);
            _expectedUserTimelineQuery = string.Format(Resources.Timeline_GetUserTimeline, userQueryParameter);
            _expectedMentionsTimelineQuery = string.Format(Resources.Timeline_GetMentionsTimeline, queryParameterBuilder);
        }

        #region GetHomeTimelineQuery

        [TestMethod]
        public void GetHomeTimelineQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTimelineQueryGenerator();

            // Act
            var result = queryGenerator.GetHomeTimelineQuery(_homeTimelineParameters);

            // Assert
            Assert.AreEqual(result, _expectedTimelineQuery);
        }

        #endregion

        #region GetUserTimelineQuery

        [TestMethod]
        public void GetUserTimelineFromNullParameter_ThrowArgumentException()
        {
            // Arrange
            var queryGenerator = CreateTimelineQueryGenerator();

            // Act
            try
            {
                queryGenerator.GetUserTimelineQuery(null);
            }
            catch (ArgumentException)
            {
                return;
            }

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        public void GetUserTimelineFromExistingUserIdentifier_ReturnsQuery()
        {
            // Arrange
            var queryGenerator = CreateTimelineQueryGenerator();
            _userQueryValidator.ArrangeCanUserBeIdentified(_userIdentifier, true);

            // Act
            var result = queryGenerator.GetUserTimelineQuery(_userTimelineQueryParameters);

            // Assert
            Assert.AreEqual(result, _expectedUserTimelineQuery);

            A.CallTo(() => _userQueryValidator.ThrowIfUserCannotBeIdentified(_userIdentifier)).MustHaveHappened();
        }

        #endregion

        #region GetMentionsTimelineQuery

        [TestMethod]
        public void GetMentionsTimelineQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTimelineQueryGenerator();

            // Act
            var result = queryGenerator.GetMentionsTimelineQuery(_mentionsTimelineParameters);

            // Assert
            Assert.AreEqual(result, _expectedMentionsTimelineQuery);
        }

        #endregion

        public TimelineQueryGenerator CreateTimelineQueryGenerator()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}