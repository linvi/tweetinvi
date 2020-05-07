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

namespace xUnitinvi.ClientActions.TimelinesClient
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
        public async Task GetHomeTimelineQuery_ReturnsTweetsAsync()
        {
            // Arrange
            var queryExecutor = CreateTimelineQueryExecutor();
            var expectedQuery = TestHelper.GenerateString();

            var parameters = new GetHomeTimelineParameters();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<ITweetDTO[]>>();

            A.CallTo(() => _fakeTimelineQueryGenerator.GetHomeTimelineQuery(parameters, It.IsAny<TweetMode?>())).Returns(expectedQuery);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<ITweetDTO[]>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetHomeTimelineAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, expectedQuery);
            Assert.Equal(HttpMethod.GET, request.Query.HttpMethod);
        }

        [Fact]
        public async Task GetMentionsTimelineQuery_ReturnsTweetsAsync()
        {
            // Arrange
            var queryExecutor = CreateTimelineQueryExecutor();
            var expectedQuery = TestHelper.GenerateString();

            var parameters = new GetMentionsTimelineParameters();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<ITweetDTO[]>>();

            A.CallTo(() => _fakeTimelineQueryGenerator.GetMentionsTimelineQuery(parameters, It.IsAny<TweetMode?>())).Returns(expectedQuery);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<ITweetDTO[]>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetMentionsTimelineAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, expectedQuery);
            Assert.Equal(HttpMethod.GET, request.Query.HttpMethod);
        }

        [Fact]
        public async Task GetUserTimelineQuery_ReturnsTweetsAsync()
        {
            // Arrange
            var queryExecutor = CreateTimelineQueryExecutor();
            var expectedQuery = TestHelper.GenerateString();

            var parameters = new GetUserTimelineParameters("linvi");
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<ITweetDTO[]>>();

            A.CallTo(() => _fakeTimelineQueryGenerator.GetUserTimelineQuery(parameters, It.IsAny<TweetMode?>())).Returns(expectedQuery);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<ITweetDTO[]>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetUserTimelineAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, expectedQuery);
            Assert.Equal(HttpMethod.GET, request.Query.HttpMethod);
        }

        [Fact]
        public async Task GetRetweetsOfMeTimelineQuery_ReturnsTweetsAsync()
        {
            // Arrange
            var queryExecutor = CreateTimelineQueryExecutor();
            var expectedQuery = TestHelper.GenerateString();

            var parameters = new GetRetweetsOfMeTimelineParameters();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<ITweetDTO[]>>();

            A.CallTo(() => _fakeTimelineQueryGenerator.GetRetweetsOfMeTimelineQuery(parameters, It.IsAny<TweetMode?>())).Returns(expectedQuery);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<ITweetDTO[]>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetRetweetsOfMeTimelineAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, expectedQuery);
            Assert.Equal(HttpMethod.GET, request.Query.HttpMethod);
        }
    }
}