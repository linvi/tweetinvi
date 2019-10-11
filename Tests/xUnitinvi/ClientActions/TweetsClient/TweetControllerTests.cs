using System;
using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;
using Xunit;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.ClientActions.TweetsClient
{
    public class TweetControllerTests
    {
        public TweetControllerTests()
        {
            _fakeBuilder = new FakeClassBuilder<TweetController>();
            _fakeTweetQueryExecutor = _fakeBuilder.GetFake<ITweetQueryExecutor>().FakedObject;
            _fakePageCursorIteratorFactories = _fakeBuilder.GetFake<IPageCursorIteratorFactories>().FakedObject;
        }

        private readonly FakeClassBuilder<TweetController> _fakeBuilder;
        private readonly ITweetQueryExecutor _fakeTweetQueryExecutor;
        private readonly IPageCursorIteratorFactories _fakePageCursorIteratorFactories;

        private TweetController CreateTweetController()
        {
            return _fakeBuilder.GenerateClass();
        }

        [Fact]
        public async Task GetTweet_ReturnsQueryExecutorResult()
        {
            // Arrange
            var controller = CreateTweetController();
            var parameters = new GetTweetParameters(42);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<ITweetDTO>>();

            A.CallTo(() => _fakeTweetQueryExecutor.GetTweet(parameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.GetTweet(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }
        
        [Fact]
        public async Task PublishTweet_ReturnsQueryExecutorResult()
        {
            // Arrange
            var controller = CreateTweetController();
            var parameters = new PublishTweetParameters("hello");
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<ITweetDTO>>();

            A.CallTo(() => _fakeTweetQueryExecutor.PublishTweet(parameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.PublishTweet(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }
        
        [Fact]
        public void GetFavoriteTweets_ReturnsFromPageCursorIteratorFactories()
        {
            // arrange
            var parameters = new GetFavoriteTweetsParameters("username") { PageSize = 2 };
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?>>();

            A.CallTo(() => _fakePageCursorIteratorFactories.Create(parameters, It.IsAny<Func<long?, Task<ITwitterResult<ITweetDTO[]>>>>()))
                .Returns(expectedResult);

            var controller = CreateTweetController();
            var friendIdsIterator = controller.GetFavoriteTweets(parameters, request);

            // assert
            Assert.Equal(friendIdsIterator, expectedResult);
        }
        
        [Fact]
        public async Task GetRetweets_ReturnsQueryExecutorResult()
        {
            // Arrange
            var controller = CreateTweetController();
            var parameters = new GetRetweetsParameters(42);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<ITweetDTO[]>>();

            A.CallTo(() => _fakeTweetQueryExecutor.GetRetweets(parameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.GetRetweets(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }
        
        [Fact]
        public async Task PublishRetweet_ReturnsQueryExecutorResult()
        {
            // Arrange
            var controller = CreateTweetController();
            var parameters = new PublishRetweetParameters(42);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<ITweetDTO>>();

            A.CallTo(() => _fakeTweetQueryExecutor.PublishRetweet(parameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.PublishRetweet(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }
        
        [Fact]
        public async Task DestroyRetweet_ReturnsQueryExecutorResult()
        {
            // Arrange
            var controller = CreateTweetController();
            var parameters = new DestroyRetweetParameters(42);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<ITweetDTO>>();

            A.CallTo(() => _fakeTweetQueryExecutor.DestroyRetweet(parameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.DestroyRetweet(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }
    }
}