using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi;
using Tweetinvi.Controllers.Timeline;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;
using Xunit;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.ClientActions.TimelineClient
{
    public class TimelineQueryExecutorTests
    {
        public TimelineQueryExecutorTests()
        {
            _fakeBuilder = new FakeClassBuilder<TimelineQueryExecutor>();
            _fakeTimelineQueryGenerator = _fakeBuilder.GetFake<ITimelineQueryGenerator>().FakedObject;
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>().FakedObject;
        }

        private readonly FakeClassBuilder<TimelineQueryExecutor> _fakeBuilder;
        private readonly ITimelineQueryGenerator _fakeTimelineQueryGenerator;
        private readonly ITwitterAccessor _fakeTwitterAccessor;

        private TimelineQueryExecutor CreateTimelineQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }

        [Fact]
        public async Task GetHomeTimelineQuery_ReturnsTweets()
        {
            // Arrange
            var queryExecutor = CreateTimelineQueryExecutor();
            var expectedQuery = TestHelper.GenerateString();

            var parameters = new GetGetHomeTimelineParameters();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<ITweetDTO[]>>();

            A.CallTo(() => _fakeTimelineQueryGenerator.GetHomeTimelineQuery(parameters, It.IsAny<TweetMode?>())).Returns(expectedQuery);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequest<ITweetDTO[]>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetHomeTimeline(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, expectedQuery);
            Assert.Equal(HttpMethod.GET, request.Query.HttpMethod);
        }

        [Fact]
        public async Task GetRetweetsOfMeTimelineQuery_ReturnsTweets()
        {
            // Arrange
            var queryExecutor = CreateTimelineQueryExecutor();
            var expectedQuery = TestHelper.GenerateString();

            var parameters = new GetRetweetsOfMeTimelineParameters();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<ITweetDTO[]>>();

            A.CallTo(() => _fakeTimelineQueryGenerator.GetRetweetsOfMeTimelineQuery(parameters, It.IsAny<TweetMode?>())).Returns(expectedQuery);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequest<ITweetDTO[]>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetRetweetsOfMeTimeline(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, expectedQuery);
            Assert.Equal(HttpMethod.GET, request.Query.HttpMethod);
        }
    }
}