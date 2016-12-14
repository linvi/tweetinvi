using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;

namespace Examplinvi.NETCore
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Console.WriteLine("Authenticating with API");
            ManualAuthentication();

            Console.WriteLine("Running example Tweetinvi calls to API");
            RunExamples();

            Console.ReadKey();
        }

        /// <summary>
        /// Paste your credentials below to authenticate with the Twitter API.
        /// </summary>
        private static void ManualAuthentication()
        {
            Auth.SetUserCredentials("mj8tSmfpsQ1nXz1hKFCPc0mue", "3trmDs85i8y1xxRw5jRRWqeYiHVW1MsbGKWdxhLL9Q0iEaf7CT", "1693649419-Ubxt4bKWWGQiRY9Ko4BcvX03EJUm2BPcRbW6pPM", "wLa7UFyp4FEDR2MHPtr6SEy4E3iCBqqlNAXuampl2SXZ7");
        }

        private static void RunExamples()
        {
            IUser authenticatedUser = User.GetAuthenticatedUser();
            IEnumerable<ITweet> authenticatedUserTimeline = authenticatedUser.GetUserTimeline();
            IEnumerable<ITweet> microsoftTimeline = Timeline.GetUserTimeline("Microsoft");

            foreach (ITweet tweet in microsoftTimeline)
            {
                Console.WriteLine(tweet);
            }
        }
    }
}
