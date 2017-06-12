using System;
using Tweetinvi;

namespace Examplinvi.Standard
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