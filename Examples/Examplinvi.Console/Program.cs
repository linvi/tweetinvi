using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.V2.Responses;
using Tweetinvi.Parameters;
using Tweetinvi.Parameters.V2;

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

            var creds = new TwitterCredentials("gnUYV0Irv5caCwQbiaQSmt5xJ",
                "LCvHDn5v5DZ18fnPXP0qIrZImg9HhYfh4rJOzYFk3RPyuYhtAN",
                "1577389800-N1UDOnbaaDjF9TlcZZl1BKW8XvYaoaZUKH7nctv","nF7HjXB0xwaNzJsDeEDFyREGJF3Yrb73zIoRozN22eo3s")
            {
                BearerToken = "AAAAAAAAAAAAAAAAAAAAAFqqSQAAAAAABRtNASGJXtIVX1somRAmqhSj68o%3Dm3n0HLyG1OmZaFDsrLITnStpXHPU82RYr4HJAN1TdG9QpmEPky"
            };

            var appClient = new TwitterClient(new ReadOnlyConsumerCredentials(creds));
            var userClient = new TwitterClient(creds);

            TweetinviEvents.SubscribeToClientEvents(userClient);

            // var tweets = new List<ITweet>();
            // var iterator = userClient.Timelines.GetUserTimelineIterator(new GetUserTimelineParameters("tweetinviapi")
            // {
            //     PageSize = 5,
            //     ContinueMinMaxCursor = ContinueMinMaxCursor.UntilNoItemsReturned
            // });
            //
            // while (!iterator.Completed)
            // {
            //     var tweetsResponse = await iterator.NextPageAsync();
            //     var p = tweetsResponse.ToArray();
            //     tweets.AddRange(tweetsResponse);
            // }


            var searchResponses = new List<SearchTweetsResponseDTO>();
            var searchIterator = userClient.SearchV2.GetSearchTweetsV2Iterator(new SearchTweetsV2Parameters("tweetinvi")
            {
                PageSize = 10
            });

            while (!searchIterator.Completed)
            {
                var page = await searchIterator.NextPageAsync();
                searchResponses.Add(page.Content);
            }

            // ask the user for the pin code
            // var authenticatedUser = await client.Users.GetAuthenticatedUserAsync();

            // System.Console.WriteLine(authenticatedUser);
            System.Console.ReadLine();
        }
    }
}