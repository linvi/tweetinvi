using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Web;
using Tweetinvi.Exceptions;
using Tweetinvi.Logic.DTO;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;
using Tweetinvi.WebLogic;
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
        }

        private readonly FakeClassBuilder<TweetController> _fakeBuilder;
        private readonly ITweetQueryExecutor _fakeTweetQueryExecutor;
        private TwitterResult<ITweetDTO[]> _firstResult;
        private TwitterResult<ITweetDTO[]> _secondResult;

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
        
        private void InitializeTweetsIterator()
        {
            var page1UserDTOs = new ITweetDTO[]
            {
                new TweetDTO { Id = 42 },
                new TweetDTO { Id = 43 }
            };

            var page2UserDTOs = new ITweetDTO[]
            {
                new TweetDTO { Id = 44 }
            };

            var jsonConverter = A.Fake<IJsonObjectConverter>();

            _firstResult = new TwitterResult<ITweetDTO[]>(A.Fake<IJsonObjectConverter>())
            {
                DataTransferObject = page1UserDTOs,
                Response = new TwitterResponse { Text = "json1" }
            };

            _secondResult = new TwitterResult<ITweetDTO[]>(A.Fake<IJsonObjectConverter>())
            {
                DataTransferObject = page2UserDTOs,
                Response = A.Fake<ITwitterResponse>()
            };

            A.CallTo(() => jsonConverter.DeserializeObject<ITweetDTO[]>(_firstResult.Response.Text, null)).Returns(page1UserDTOs);
            A.CallTo(() => jsonConverter.DeserializeObject<ITweetDTO[]>(_secondResult.Response.Text, null)).Returns(page2UserDTOs);
        }
        
        [Fact]
        public async Task GetFavoriteTweets_MoveToNextPage_ReturnsAllPages()
        {
            InitializeTweetsIterator();
            
            // arrange
            var parameters = new GetFavoriteTweetsParameters("username") { PageSize = 2 };
            var request = A.Fake<ITwitterRequest>();

            A.CallTo(() => _fakeTweetQueryExecutor.GetFavoriteTweets(It.IsAny<IGetFavoriteTweetsParameters>(), It.IsAny<ITwitterRequest>()))
                .ReturnsNextFromSequence(_firstResult, _secondResult);

            var controller = CreateTweetController();
            var friendIdsIterator = controller.GetFavoriteTweets(parameters, request);

            // act
            var page1 = await friendIdsIterator.MoveToNextPage();
            var page2 = await friendIdsIterator.MoveToNextPage();

            // assert
            Assert.Equal(page1.Content, _firstResult);
            Assert.False(page1.IsLastPage);

            Assert.Equal(page2.Content, _secondResult);
            Assert.True(page2.IsLastPage);
        }

        [Fact]
        public async Task GetFavoriteTweets_MoveToNextPage_ThrowsIfCompleted()
        {
            InitializeTweetsIterator();
            
            // arrange
            var parameters = new GetFavoriteTweetsParameters("username") { PageSize = 2 };
            var request = A.Fake<ITwitterRequest>();

            A.CallTo(() => _fakeTweetQueryExecutor.GetFavoriteTweets(It.IsAny<IGetFavoriteTweetsParameters>(), It.IsAny<ITwitterRequest>()))
                .ReturnsNextFromSequence(_firstResult, _secondResult);

            var controller = CreateTweetController();
            var friendIdsIterator = controller.GetFavoriteTweets(parameters, request);

            await friendIdsIterator.MoveToNextPage();
            await friendIdsIterator.MoveToNextPage();

            // act
            await Assert.ThrowsAsync<TwitterIteratorAlreadyCompletedException>(() => friendIdsIterator.MoveToNextPage());
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