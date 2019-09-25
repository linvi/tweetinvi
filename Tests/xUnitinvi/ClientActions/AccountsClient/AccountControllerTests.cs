using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi.Controllers.Account;
using Tweetinvi.Core.DTO.Cursor;
using Tweetinvi.Core.Web;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;
using Tweetinvi.WebLogic;
using Xunit;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.ClientActions.AccountsClient
{
    public class AccountControllerTests
    {
        public AccountControllerTests()
        {
            _fakeBuilder = new FakeClassBuilder<AccountController>();
            _fakeAccountQueryExecutor = _fakeBuilder.GetFake<IAccountQueryExecutor>().FakedObject;
        }
        
        private readonly FakeClassBuilder<AccountController> _fakeBuilder;
        private readonly IAccountQueryExecutor _fakeAccountQueryExecutor;

        private AccountController CreateAccountController()
        {
            return _fakeBuilder.GenerateClass();
        }

        [Fact]
        public async Task GetUserIdsRequestingFriendship_MoveToNextPage_ReturnsAllPages()
        {
            // arrange
            var accountController = CreateAccountController();
            var parameters = new GetUserIdsRequestingFriendshipParameters();

            ITwitterResult<IIdsCursorQueryResultDTO>[] results = {
                new TwitterResult<IIdsCursorQueryResultDTO>(null)
                {
                    DataTransferObject = new IdsCursorQueryResultDTO
                    {
                        Ids = new long[] { 42, 43 },
                        NextCursorStr = "cursor_to_page_2",
                    },
                    Response = new TwitterResponse()
                },
                new TwitterResult<IIdsCursorQueryResultDTO>(null)
                {
                    DataTransferObject = new IdsCursorQueryResultDTO
                    {
                        Ids = new long[] { 44, 45 },
                        NextCursorStr = "0",
                    },
                    Response = new TwitterResponse()
                }
            };

            A.CallTo(() => _fakeAccountQueryExecutor.GetUserIdsRequestingFriendship(It.IsAny<IGetUserIdsRequestingFriendshipParameters>(), It.IsAny<ITwitterRequest>()))
                .ReturnsNextFromSequence(results);
            
            var result = accountController.GetUserIdsRequestingFriendship(parameters, A.Fake<ITwitterRequest>());

            // act
            var page1 = await result.MoveToNextPage();
            var page2 = await result.MoveToNextPage();

            // assert
            Assert.Equal(page1.Content, results[0]);
            Assert.False(page1.IsLastPage);
            
            Assert.Equal(page2.Content, results[1]);
            Assert.True(page2.IsLastPage);

            await Assert.ThrowsAsync<TwitterIteratorAlreadyCompletedException>(() => result.MoveToNextPage());
        }
    }
}