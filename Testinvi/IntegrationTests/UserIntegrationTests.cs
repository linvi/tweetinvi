using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tweetinvi;
using Tweetinvi.Models;

namespace Testinvi.IntegrationTests
{
    [TestClass]
    public class UserIntegrationTests
    {
        [TestMethod]
        [Ignore]
        public async Task TestUsers ()
        {
            var credentials = new TwitterCredentials ("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_SECRET");

            var client = new TwitterClient (credentials);

            var authenticatedUser = await client.Users.GetAuthenticatedUser ();

            Assert.IsNotNull (authenticatedUser);
        }
    }
}