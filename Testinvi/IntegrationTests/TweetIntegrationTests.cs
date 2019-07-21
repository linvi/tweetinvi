using System.Linq;
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

            var client = new TwitterClient(credentials);

            client.Config.ErrorHandlerType = ErrorHandlerType.ReturnNull;

            // Publish Tweet
            var tweet = await client.Tweets.PublishTweet("hello from tweetinvi 42");

            Assert.IsNotNull(tweet);

            var retrievedTweet = await client.Tweets.GetTweet(tweet.Id);

            // Retweets
            Assert.AreEqual(retrievedTweet.Id, tweet.Id);

            var retweet = await client.Tweets.PublishRetweet(tweet);
            var retweets = await client.Tweets.GetRetweets(tweet);

            Assert.IsTrue(retweets.Any(x => x.Id == retweet.Id));
            Assert.AreEqual(retweet.RetweetedTweet.Id, tweet.Id);

            var retweetDeleteSuccess = await retweet.Destroy();

            Assert.AreEqual(retweetDeleteSuccess, true);

            var retweet2 = await tweet.PublishRetweet();

            Assert.AreEqual(retweet2.RetweetedTweet.Id, tweet.Id);

            // Tweet Delete
            var retweet2DeleteSuccess = await retweet2.Destroy();

            Assert.AreEqual(retweet2DeleteSuccess, true);

            var tweetDeletedSuccess = await tweet.Destroy();

            Assert.AreEqual(tweet.IsTweetDestroyed, true);
            Assert.AreEqual(tweetDeletedSuccess, true);

            // expected failures
            var secondDeleteSuccess = await client.Tweets.DestroyTweet(tweet);

            Assert.AreEqual(secondDeleteSuccess, false);

            var deletedTweet = await client.Tweets.GetTweet(tweet.Id);

            Assert.IsNull(deletedTweet);
        }
    }
}
