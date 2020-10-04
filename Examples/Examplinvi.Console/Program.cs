using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;

namespace Examplinvi.Console
{
    class Program
    {
        static async Task Main()
        {
            TweetinviEvents.BeforeExecutingRequest += (sender, args) =>
            {
                System.Console.WriteLine(args.Url);
            };

            var credentials = new TwitterCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
            var client = new TwitterClient(credentials);

            TweetinviEvents.SubscribeToClientEvents(client);

            var authenticatedUser = await client.Users.GetAuthenticatedUserAsync();
            System.Console.WriteLine(authenticatedUser);
            System.Console.ReadLine();
        }
    }
}