using System;
using Tweetinvi;
using Tweetinvi.Models;

namespace Examplinvi.NETStandard
{
    class Program
    {
        static void Main()
        {
            var credentials = new TwitterCredentials("5EpUsp9mbMMRMJ0zqsug", "cau8CExOCUordXMJeoGfW0QoPTp6bUAOrqUELKk1CSM", "1577389800-c8ecF1YWfYJjFraEohBHxqv37xXDnsAOoQOP4vX", "YZ3wcpMDX7ydZ8IPVkbBpcUWIyRnTqTnudyjD9Fm8g");
            var client = new TwitterClient(credentials);

            var tweet = client.Tweets.GetTweet(979753598446948353).Result;

            Console.WriteLine(tweet);

            //Auth.SetUserCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");

            //var authenticatedUser = User.GetAuthenticatedUser();

            //var tweet = Tweet.GetTweet(1).Result;

            //RateLimit.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;

            //Console.WriteLine(authenticatedUser);
            Console.ReadLine();
        }
    }
}