using System;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Examplinvi.NETStandard
{
    class Program
    {
        static void Main()
        {
            Run().Wait();
        }

        static async Task Run()
        {
            TweetinviEvents.BeforeWaitingForRequestRateLimits += (sender, args) =>
            {
                Console.WriteLine(args.Url);
            };

            var credentials = new TwitterCredentials("A", "B", "C", "D");
            var client = new TwitterClient(credentials);

            var authenticatedUser = await client.Users.GetAuthenticatedUser();

            var iterator = client.Timelines.GetUserTimelineIterator(new GetUserTimelineParameters(authenticatedUser)
            {
                PageSize = 5,
            });

            while (!iterator.Completed)
            {
                var page1 = await iterator.MoveToNextPage();
                Console.WriteLine(page1);
            }

            Console.WriteLine(authenticatedUser);
            Console.ReadLine();
        }
    }
}