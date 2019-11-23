using Tweetinvi.Controllers.Auth;
using Xunit;

namespace xUnitinvi.ClientActions.AuthClient
{
    public class AuthQueryGeneratorTests
    {
        private static AuthQueryGenerator CreateQueryGenerator()
        {
            return new AuthQueryGenerator();
        }

        [Fact]
        public void CreateBearerToken_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateQueryGenerator();


            // act
            var result = queryGenerator.GetCreateBearerTokenQuery();

            // assert
            Assert.Equal(result, $"https://api.twitter.com/oauth2/token");
        }
    }
}