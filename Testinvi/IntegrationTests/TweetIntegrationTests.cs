using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tweetinvi;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;

namespace Testinvi.IntegrationTests
{
    [TestClass]
    public class TweetIntegrationTests
    {
        [TestMethod]
        public async Task TestTweets()
        {
            var credentials = new TwitterCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_SECRET");
            Auth.SetCredentials(credentials);
            var client = new TwitterClient(credentials);

            var tweet = await client.Tweets.PublishTweet("hello from tweetinvi");

            Assert.IsNotNull(tweet);

            var checkTweet = await client.Tweets.GetTweet(tweet.Id);

            Assert.AreEqual(checkTweet.Id, tweet.Id);

            var deleteSuccessful = await Tweet.DestroyTweet(tweet);

            Assert.AreEqual(deleteSuccessful, true);

            try
            {
                await client.Tweets.GetTweet(tweet.Id);
            }
            catch (TwitterRequestException e)
            {
                Console.WriteLine(e);
            }

        }
    }
}
