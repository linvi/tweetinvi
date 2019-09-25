using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi.Controllers.Account;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;
using Xunit;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.ClientActions.AccountsClient
{
    public class AccountQueryExecutorTests
    {
        public AccountQueryExecutorTests()
        {
            _fakeBuilder = new FakeClassBuilder<AccountQueryExecutor>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>().FakedObject;
        }

        private readonly FakeClassBuilder<AccountQueryExecutor> _fakeBuilder;
        private readonly ITwitterAccessor _fakeTwitterAccessor;

        private AccountQueryExecutor CreateAccountQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }

        [Fact]
        public async Task GetFavoriteTweets_ReturnsFavoritedTweets()
        {
            // Arrange
            var queryExecutor = CreateAccountQueryExecutor();
            var parameters = new GetUserIdsRequestingFriendshipParameters();
            
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IIdsCursorQueryResultDTO>>();

            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequest<IIdsCursorQueryResultDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetUserIdsRequestingFriendship(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }
    }
}