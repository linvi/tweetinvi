using System;
using Tweetinvi;

namespace Examplinvi.NETStandard
{
    class Program
    {
        static void Main()
        {
            Auth.SetUserCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");

            var authenticatedUser = User.GetAuthenticatedUser();

            var tweet = Tweet.GetTweet(1).Result;

            RateLimit.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;

            Console.WriteLine(authenticatedUser);
            Console.ReadLine();
        }
    }
}