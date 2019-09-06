using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Xunit;

namespace xUnitinvi.IntegrationTests
{
    public class UserIntegrationTests
    {
        //[Fact]
        [Fact(Skip = "IntegrationTests")]
        public async Task TestUsers()
        {
            var credentials = new TwitterCredentials("pjvWONiGoHqJ17spTmzB7Rhds", "veS3SpLIyNqVj9epi9w64tAWFAufojoZ3gTVXCZeAxH1WxzPCb", "1577389800-sGNIxwIIfqJWXJMGhCcQTe8CHCPpguvlAdSJd6D", "3rvNBzaBEn0Iwxk3qTpzpJOxmGsz81WSJfwP1sJ3yqIbw");

            var client = new TwitterClient(credentials);

            // act
            var authenticatedUser = await client.Users.GetAuthenticatedUser();
            var tweetinviUser = await client.Users.GetUser("tweetinviapi");
            var user = await client.Users.GetFriendIds("tweetinviapi");

            // assert
            Assert.Equal(tweetinviUser.Id, 1577389800);
            Assert.NotNull(authenticatedUser);
            Assert.Contains(1693649419, user.Items);
        }
    }
}
