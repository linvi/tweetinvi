using System;
using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    /// <summary>
    /// Event raised to inform that a request is starting its execution
    /// </summary>
    public class BeforeExecutingRequestEventArgs : QueryExecutionEventArgs
    {
        public BeforeExecutingRequestEventArgs(ITwitterQuery twitterQuery) : base(twitterQuery)
        {
            BeforeExecutingDateTime = DateTime.Now;
        }

        public DateTime BeforeExecutingDateTime { get; set; }

        /// <summary>
        /// If set to true this query won't be executed.
        /// </summary>
        public bool Cancel { get; set; }
    }
}