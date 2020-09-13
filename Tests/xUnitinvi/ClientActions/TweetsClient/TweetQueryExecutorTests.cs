using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;
using Xunit;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.ClientActions.TweetsClient
{
    public class TweetQueryExecutorTests
    {
        public TweetQueryExecutorTests()
        {
            _fakeBuilder = new FakeClassBuilder<TweetQueryExecutor>();
            _fakeTweetQueryGenerator = _fakeBuilder.GetFake<ITweetQueryGenerator>().FakedObject;
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>().FakedObject;
        }

        private readonly FakeClassBuilder<TweetQueryExecutor> _fakeBuilder;
        private readonly ITweetQueryGenerator _fakeTweetQueryGenerator;
        private readonly ITwitterAccessor _fakeTwitterAccessor;

        private TweetQueryExecutor CreateUserQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }

        [Fact]
        public async Task GetTweet_ReturnsFavoritedTweetsAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var expectedQuery = TestHelper.GenerateString();

            var parameters = new GetTweetParameters(42);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<ITweetDTO>>();

            A.CallTo(() => _fakeTweetQueryGenerator.GetTweetQuery(parameters, It.IsAny<ComputedTweetMode>())).Returns(expectedQuery);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<ITweetDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetTweetAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, expectedQuery);
            Assert.Equal(HttpMethod.GET, request.Query.HttpMethod);
        }

        [Fact]
        public async Task PublishTweet_ReturnsFavoritedTweetsAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var expectedQuery = TestHelper.GenerateString();

            var parameters = new PublishTweetParameters("hello");
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<ITweetDTO>>();

            A.CallTo(() => _fakeTweetQueryGenerator.GetPublishTweetQuery(parameters, It.IsAny<ComputedTweetMode>())).Returns(expectedQuery);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<ITweetDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.PublishTweetAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, expectedQuery);
            Assert.Equal(HttpMethod.POST, request.Query.HttpMethod);
        }

        [Fact]
        public async Task GetFavoriteTweets_ReturnsFavoritedTweetsAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var expectedQuery = TestHelper.GenerateString();

            var parameters = new GetUserFavoriteTweetsParameters(42);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<ITweetDTO[]>>();

            A.CallTo(() => _fakeTweetQueryGenerator.GetFavoriteTweetsQuery(parameters, It.IsAny<ComputedTweetMode>())).Returns(expectedQuery);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<ITweetDTO[]>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetFavoriteTweetsAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, expectedQuery);
            Assert.Equal(HttpMethod.GET, request.Query.HttpMethod);
        }

        [Fact]
        public async Task GetRetweets_ReturnsFavoritedTweetsAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var expectedQuery = TestHelper.GenerateString();

            var parameters = new GetRetweetsParameters(42);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<ITweetDTO[]>>();

            A.CallTo(() => _fakeTweetQueryGenerator.GetRetweetsQuery(parameters, It.IsAny<ComputedTweetMode>())).Returns(expectedQuery);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<ITweetDTO[]>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetRetweetsAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, expectedQuery);
            Assert.Equal(HttpMethod.GET, request.Query.HttpMethod);
        }

        [Fact]
        public async Task PublishRetweet_ReturnsFavoritedTweetsAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var expectedQuery = TestHelper.GenerateString();

            var parameters = new PublishRetweetParameters(42);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<ITweetDTO>>();

            A.CallTo(() => _fakeTweetQueryGenerator.GetPublishRetweetQuery(parameters, It.IsAny<ComputedTweetMode>())).Returns(expectedQuery);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<ITweetDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.PublishRetweetAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, expectedQuery);
            Assert.Equal(HttpMethod.POST, request.Query.HttpMethod);
        }

        [Fact]
        public async Task DestroyRetweet_ReturnsFavoritedTweetsAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var expectedQuery = TestHelper.GenerateString();

            var parameters = new DestroyRetweetParameters(42);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<ITweetDTO>>();

            A.CallTo(() => _fakeTweetQueryGenerator.GetDestroyRetweetQuery(parameters, It.IsAny<ComputedTweetMode>())).Returns(expectedQuery);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<ITweetDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.DestroyRetweetAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, expectedQuery);
            Assert.Equal(HttpMethod.POST, request.Query.HttpMethod);
        }

        [Fact]
        public async Task GetRetweeterIds_ReturnsUserIdsAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var expectedQuery = TestHelper.GenerateString();

            var parameters = new GetRetweeterIdsParameters(42);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IIdsCursorQueryResultDTO>>();

            A.CallTo(() => _fakeTweetQueryGenerator.GetRetweeterIdsQuery(parameters)).Returns(expectedQuery);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<IIdsCursorQueryResultDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetRetweeterIdsAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, expectedQuery);
            Assert.Equal(HttpMethod.GET, request.Query.HttpMethod);
        }
    }
}