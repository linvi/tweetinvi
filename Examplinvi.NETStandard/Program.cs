using System;
using Tweetinvi;
using Tweetinvi.Models;

namespace Examplinvi.NETStandard
{
    class Program
    {
        static void Main(string[] args)
        {
            Auth.SetUserCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");

            var authenticatedUser = User.GetAuthenticatedUser();

            Console.WriteLine(authenticatedUser);
            Console.ReadLine();

        }
    }
}