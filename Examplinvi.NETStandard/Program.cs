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
            var credentials = new TwitterCredentials("A", "B", "C", "D");
            var client = new TwitterClient(credentials);

            var authenticatedUser = await client.Users.GetAuthenticatedUser();

            Console.WriteLine(authenticatedUser);
            Console.ReadLine();
        }
    }
}