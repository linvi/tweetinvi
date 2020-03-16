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
            TweetinviEvents.BeforeWaitingForRequestRateLimits += (sender, args) =>
            {
                Console.WriteLine(args.Url);
            };

            // var credentials = new TwitterCredentials("A", "B", "C", "D");
            // var client = new TwitterClient(credentials);

            var credentials = new TwitterCredentials("bXm1V8Nv8eGMStB8NTysH4i8J", "dLRAwipXIfb7v7bdhmDgovfCEBtHBq51oLgM08LUzG0yOemfXI", "1577389800-Ncrm3GYQIaWGdGSpWtzFnPYDZDdGI96ysHctf9v",
                "DlAGYw4Pd5dXcggopDybmR9v78jl1jCd72M5K8vgSnwad");
            var authenticationClient = new TwitterClient(credentials);
            var authenticationRequest = await authenticationClient.Auth.RequestAuthenticationUrl().ConfigureAwait(false);
            var authUrl = authenticationRequest.AuthorizationURL;

            // ask the user for the pin code
            var verifierCode = Console.ReadLine();
            var userCredentials = await authenticationClient.Auth.RequestCredentialsFromVerifierCode(verifierCode, authenticationRequest).ConfigureAwait(false);
            var authenticatedClient = new TwitterClient(userCredentials);
            var authenticatedUser = await authenticatedClient.Users.GetAuthenticatedUser().ConfigureAwait(false);


            // var iterator = client.Timelines.GetUserTimelineIterator(new GetUserTimelineParameters(authenticatedUser)
            // {
            //     PageSize = 5,
            // });
            //
            // while (!iterator.Completed)
            // {
            //     var page1 = await iterator.MoveToNextPage();
            //     Console.WriteLine(page1);
            // }

            Console.WriteLine(authenticatedUser);
            Console.ReadLine();
        }
    }
}