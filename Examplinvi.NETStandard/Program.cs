using System;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Examplinvi.NETStandard
{
    class Program
    {
        static void Main(string[] args)
        {
            Auth.SetUserCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");

            var authenticatedUser = User.GetAuthenticatedUser();

            var tweet = Tweet.GetTweet(1);


            RateLimit.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;

            Console.WriteLine(authenticatedUser);
            Console.ReadLine();
        }
    }
}