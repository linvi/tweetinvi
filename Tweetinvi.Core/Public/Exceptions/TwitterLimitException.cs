using System;

namespace Tweetinvi.Exceptions
{
    public class TwitterLimitException : Exception
    {
        public string Rule { get; }

        public TwitterLimitException(string message, string rule) : base(message)
        {
            Rule = rule;
        }

        public string Note
        {
            get => "Limits can be changed in the TweetinviSettings.Limits";
        }
    }
}
