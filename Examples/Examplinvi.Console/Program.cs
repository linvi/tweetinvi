using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;

namespace Examplinvi.Console
{
    class Program
    {
        static async Task Main()
        {
            var credentials = new TwitterCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
            var client = new TwitterClient(credentials);

            // ask the user for the pin code
            var authenticatedUser = await client.Users.GetAuthenticatedUserAsync();

            System.Console.WriteLine(authenticatedUser);
            System.Console.ReadLine();
        }
    }
}