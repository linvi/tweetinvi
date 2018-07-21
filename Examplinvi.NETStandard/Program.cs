using System;
using Tweetinvi;
using Tweetinvi.Models;

namespace Examplinvi.NETStandard
{
    class Program
    {
        static void Main(string[] args)
        {
            Auth.SetUserCredentials("5EpUsp9mbMMRMJ0zqsug", "cau8CExOCUordXMJeoGfW0QoPTp6bUAOrqUELKk1CSM", "1577389800-c8ecF1YWfYJjFraEohBHxqv37xXDnsAOoQOP4vX", "YZ3wcpMDX7ydZ8IPVkbBpcUWIyRnTqTnudyjD9Fm8g");

            var authenticatedUser = User.GetAuthenticatedUser();
            var messages = Message.GetLatestMessages();

            Console.WriteLine(authenticatedUser);
            Console.ReadLine();
        }
    }
}