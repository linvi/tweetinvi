using System;
using Tweetinvi;
using Tweetinvi.Models;

namespace Examplinvi.NETStandard
{
    class Program
    {
        static void Main(string[] args)
        {
            TweetinviConfig.CurrentThreadSettings.TweetMode = TweetMode.Compat;
            Auth.SetUserCredentials("54aUlLwbmRN7SX3wswMJmqpQG", "Fid46uknw6665bA4NxCiOHPoIUv6KMsaxhul7P4MwHMecIytYw", "1693649419-Ubxt4bKWWGQiRY9Ko4BcvX03EJUm2BPcRbW6pPM", "wLa7UFyp4FEDR2MHPtr6SEy4E3iCBqqlNAXuampl2SXZ7");

            var tweet = Tweet.GetTweet(914635724758163456);
            var suffix = tweet.Suffix;

            var l = Tweet.Length(tweet.Text);

            //var authenticatedUser = User.GetAuthenticatedUser();

            var fs = Stream.CreateFilteredStream();

            fs.AddTrack("italia");
            fs.AddTrack("trump");
            fs.AddTrack("merkel");

            fs.MatchingTweetReceived += (sender, eventArgs) =>
            {
                if (eventArgs.Tweet.Language != Language.Undefined)
                {
                    Console.WriteLine(eventArgs.Json);
                }
            };

            fs.StartStreamMatchingAllConditions();

            //Console.WriteLine(authenticatedUser);
            Console.ReadLine();
        }
    }
}