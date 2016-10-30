using System;
using Tweetinvi;
using Examplinvi.NETCore.Examples;

namespace Examplinvi.NETCore
{
    public class Program
    {
        private static ITimelineClient Timeline;

        public static void Main(string[] args)
        {
            Timeline = new TimelineClient();

            Console.WriteLine("Authenticating with API");
            //ManualAuthentication();
            ManagedAuthentication();

            Console.WriteLine("Running example Tweetinvi calls to API");
            RunExamples();
        }

        /// <summary>
        /// Paste your credentials below to authenticate with the Twitter API.
        /// </summary>
        private static void ManualAuthentication()
        {
            Auth.SetUserCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
        }

        /// <summary>
        /// Set your credentials as environment variables to authenticate with the Twitter API.
        /// </summary>
        private static void ManagedAuthentication()
        {
            var credentials = new APICredentials();
            Auth.SetUserCredentials(credentials.ConsumerKey, credentials.ConsumerSecret, credentials.AccessToken, credentials.AccessTokenSecret);
        }

        private static void RunExamples()
        {
            var homeTimeline = Timeline.GetHomeTimeline();
            var mentionsTimeline = Timeline.GetMentionsTimeline();
            var userTimeline = Timeline.GetUserTimeline("Microsoft");
        }
    }
}
