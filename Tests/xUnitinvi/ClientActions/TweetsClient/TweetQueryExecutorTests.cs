using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
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
            _fakeTweetQueryGenerator = _fakeBuilder.GetFake<ITweetQueryGenerator>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();
        }

        private readonly FakeClassBuilder<TweetQueryExecutor> _fakeBuilder;
        private readonly Fake<ITweetQueryGenerator> _fakeTweetQueryGenerator;
        private readonly Fake<ITwitterAccessor> _fakeTwitterAccessor;

        private TweetQueryExecutor CreateUserQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }

        [Fact]
        public async Task GetFavoriteTweets_ReturnsFavoritedTweets()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var userDTO = A.Fake<IUserDTO>();
            var expectedQuery = TestHelper.GenerateString();

            var parameters = new GetFavoriteTweetsParameters(userDTO);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<ITweetDTO[]>>();

            A.CallTo(() => _fakeTweetQueryGenerator.FakedObject.GetFavoriteTweetsQuery(parameters, TweetMode.Extended)).Returns(expectedQuery);
            A.CallTo(() => _fakeTwitterAccessor.FakedObject.ExecuteRequest<ITweetDTO[]>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetFavoriteTweets(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }
    }
}