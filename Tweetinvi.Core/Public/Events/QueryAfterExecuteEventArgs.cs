using System;
using System.Collections.Generic;
using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    public class QueryAfterExecuteEventArgs : QueryExecutionEventArgs
    {
        public QueryAfterExecuteEventArgs(
            ITwitterQuery twitterQuery, 
            string httpContent,
            Dictionary<string, IEnumerable<string>> httpHeaders)
            : base(twitterQuery)
        {
            HttpContent = httpContent;
            HttpHeaders = httpHeaders;
            CompletedDateTime = DateTime.Now;
        }

        /// <summary>
        /// Result returned by Twitter.
        /// </summary>
        public string HttpContent { get; private set; }

        /// <summary>
        /// Headers returned by Twitter.
        /// </summary>
        public Dictionary<string, IEnumerable<string>> HttpHeaders { get; private set; }

        /// <summary>
        /// Exact DateTime whent the request completed.
        /// </summary>
        public DateTime CompletedDateTime { get; set; }

        /// <summary>
        /// Whether the request has been successfull.
        /// </summary>
        public bool Success { get { return HttpContent != null; } }
    }
}