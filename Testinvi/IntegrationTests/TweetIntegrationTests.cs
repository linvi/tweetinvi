using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tweetinvi;
using Tweetinvi.Models;

namespace Testinvi.IntegrationTests
{
    [TestClass]
    public class TweetIntegrationTests
    {
        [TestMethod]
        [Ignore]
        public async Task TestTweets()
        {
            var credentials = new TwitterCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_SECRET");
            Auth.SetCredentials(credentials);
            var client = new TwitterClient(credentials);
            client.Config.ErrorHandlerType = ErrorHandlerType.ReturnNull;

            var tweet = await client.Tweets.PublishTweet("hello from tweetinvi 42");

            Assert.IsNotNull(tweet);

            var checkTweet = await client.Tweets.GetTweet(tweet.Id);

            Assert.AreEqual(checkTweet.Id, tweet.Id);

            var deleteSuccessful = await client.Tweets.DestroyTweet(tweet.Id);

            Assert.AreEqual(deleteSuccessful, true);

            // expected failures
            var secondDeleteSuccess = await client.Tweets.DestroyTweet(tweet.Id);

            Assert.AreEqual(secondDeleteSuccess, false);

            var deletedTweet = await client.Tweets.GetTweet(tweet.Id);

            Assert.IsNull(deletedTweet);
        }
    }
}
