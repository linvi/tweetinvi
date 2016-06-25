using System;
using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    public class QueryBeforeExecuteEventArgs : QueryExecutionEventArgs
    {
        public QueryBeforeExecuteEventArgs(ITwitterQuery twitterQuery) : base(twitterQuery)
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