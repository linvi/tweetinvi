using Tweetinvi.Models;

namespace Examplinvi.ASP.NET
{
    public static class MyCredentials
    {
        public static string CONSUMER_KEY = "54aUlLwbmRN7SX3wswMJmqpQG";
        public static string CONSUMER_SECRET = "Fid46uknw6665bA4NxCiOHPoIUv6KMsaxhul7P4MwHMecIytYw";
        public static string ACCESS_TOKEN = "1693649419-Ubxt4bKWWGQiRY9Ko4BcvX03EJUm2BPcRbW6pPM";
        public static string ACCESS_TOKEN_SECRET = "wLa7UFyp4FEDR2MHPtr6SEy4E3iCBqqlNAXuampl2SXZ7";

        public static ITwitterCredentials GenerateCredentials()
        {
            return new TwitterCredentials(CONSUMER_KEY, CONSUMER_SECRET, ACCESS_TOKEN, ACCESS_TOKEN_SECRET);
        }
    }
}