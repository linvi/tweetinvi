using System;
using Tweetinvi;
using Tweetinvi.Controllers.Account;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Parameters;
using Xunit;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.ClientActions.AccountsClient
{
    public class AccountQueryGeneratorTests
    {
        public AccountQueryGeneratorTests()
        {
            _fakeBuilder = new FakeClassBuilder<AccountQueryGenerator>();
        }

        private readonly FakeClassBuilder<AccountQueryGenerator> _fakeBuilder;

        private AccountQueryGenerator CreateAccountQueryGenerator()
        {
            return _fakeBuilder.GenerateClass(
                new ConstructorNamedParameter("queryParameterGenerator", TweetinviContainer.Resolve<IQueryParameterGenerator>()),
                new ConstructorNamedParameter("userQueryParameterGenerator", TweetinviContainer.Resolve<IUserQueryParameterGenerator>()));
        }

        [Fact]
        public void GetBlockedUserIdsQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateAccountQueryGenerator();

            var parameters = new GetUserIdsRequestingFriendshipParameters
            {
                PageSize = 42,
                Cursor = "start_cursor",
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetUserIdsRequestingFriendshipQuery(parameters);

            // Assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/friendships/incoming.json?count=42&cursor=start_cursor&hello=world");
        }
    }
}