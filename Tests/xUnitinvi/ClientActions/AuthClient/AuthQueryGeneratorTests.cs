using System;
using Tweetinvi.Controllers.Auth;
using Tweetinvi.Credentials.Models;
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
            var parameters = new CreateBearerTokenParameters
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var result = queryGenerator.GetCreateBearerTokenQuery(parameters);

            // assert
            Assert.Equal(result, $"https://api.twitter.com/oauth2/token?hello=world");
        }

        [Fact]
        public void GetRequestAuthUrlQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateQueryGenerator();
            var parameters = new RequestUrlAuthUrlParameters("url")
            {
                AuthAccessType = AuthAccessType.Read,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var result = queryGenerator.GetRequestAuthUrlQuery(parameters);

            // assert
            Assert.Equal(result, $"https://api.twitter.com/oauth/request_token?x_auth_access_type=read&hello=world");
        }

        [Fact]
        public void GetRequestCredentialsQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateQueryGenerator();
            var parameters = new RequestCredentialsParameters("verifier_code", new AuthenticationRequest())
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var result = queryGenerator.GetRequestCredentialsQuery(parameters);

            // assert
            Assert.Equal(result, $"https://api.twitter.com/oauth/access_token?hello=world");
        }

        [Fact]
        public void GetInvalidateBearerTokenQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateQueryGenerator();
            var parameters = new InvalidateBearerTokenParameters
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var result = queryGenerator.GetInvalidateBearerTokenQuery(parameters);

            // assert
            Assert.Equal(result, $"https://api.twitter.com/oauth2/invalidate_token?hello=world");
        }

        [Fact]
        public void GetInvalidateAccessTokenQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateQueryGenerator();
            var parameters = new InvalidateAccessTokenParameters
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var result = queryGenerator.GetInvalidateAccessTokenQuery(parameters);

            // assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/oauth/invalidate_token?hello=world");
        }
    }
}