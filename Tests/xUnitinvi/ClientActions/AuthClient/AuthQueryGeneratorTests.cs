using Tweetinvi.Controllers.Auth;
using Tweetinvi.Parameters.Auth;
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

        [Fact]
        public void StartAuthProcess_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateQueryGenerator();
            var parameters = new RequestUrlAuthUrlParameters("url")
            {
                AuthAccessType = AuthAccessType.Read
            };

            // act
            var result = queryGenerator.GetRequestAuthUrlQuery(parameters);

            // assert
            Assert.Equal(result, $"https://api.twitter.com/oauth/request_token?x_auth_access_type=read");
        }
    }
}