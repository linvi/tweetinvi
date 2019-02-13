using System;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Public.Models.Authentication;
using Tweetinvi.Models;

namespace Examplinvi.NETCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Auth.SetUserCredentials("xxx", "xxx", "xxx", "xxxx");

            Console.WriteLine(Auth.Credentials.ConsumerKey);



            Run().Wait();

            Console.WriteLine(Auth.Credentials.ConsumerKey);

            ExceptionHandler.TryPeekException(out var exception);



            var tweet = Tweet.GetTweet(1);

            TweetinviEvents.QueryBeforeExecute += (sender, eventArgs) =>
            {
                Console.WriteLine(eventArgs);
            };

            RateLimit.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;

            //Console.WriteLine(authenticatedUser);
            Console.ReadLine();
        }

        public static async Task Run()
        {

            Console.WriteLine(Auth.Credentials.ConsumerKey);

            //var authenticatedUser = User.GetAuthenticatedUser();
            Auth.SetUserCredentials("xxx2", "xxx2", "xxx2", "xxx2");

            Console.WriteLine(Auth.Credentials.ConsumerKey);

            ITweet theTweet = await TweetAsync.PublishTweet("hello");

            ExceptionHandler.TryPeekException(out var exception);
            ExceptionHandler.TryPeekException(out var exception2);

            Console.WriteLine("hello");
        }
    }
}