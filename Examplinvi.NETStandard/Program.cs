using System;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;

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
            TweetinviEvents.QueryBeforeExecute += (sender, args) =>
            {
                Console.WriteLine(args.Url);
            };

            var credentials = new TwitterCredentials("A", "B", "C", "D");
            var client = new TwitterClient(credentials);

            var authenticatedUser = await client.Account.GetAuthenticatedUser();

            Console.WriteLine(authenticatedUser);
            Console.ReadLine();
        }
    }
}