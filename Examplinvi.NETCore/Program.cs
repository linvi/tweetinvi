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

            RunInThread();

            Run().Wait();

            Console.WriteLine(Auth.Credentials.ConsumerKey);

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

            Console.WriteLine("hello");
        }

        public static void RunInThread()
        {
            Console.WriteLine("With thread");

            // Exec 1
            // stack:
            // creds: ptr* 1654
            Auth.SetUserCredentials("xxx2", "xxx2", "xxx2", "xxx2");

            // Heap
            // 1654: object
            //      str: xxx5151

            Thread t = new Thread(() =>
            {
                // Exec 2
                // stack:
                //  creds: ptr* 1654
                Auth.SetUserCredentials("xxx5151", "xxx", "xxx", "xxxx");
            });

            t.Start();

            t.Join();
        }
    }
}