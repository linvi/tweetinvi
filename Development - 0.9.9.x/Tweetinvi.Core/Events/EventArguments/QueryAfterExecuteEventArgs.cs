using System;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Events.EventArguments
{
    public class QueryAfterExecuteEventArgs : QueryExecutionEventArgs
    {
        public QueryAfterExecuteEventArgs(ITwitterQuery twitterQuery, string jsonResult)
            : base(twitterQuery)
        {
            JsonResult = jsonResult;
            CompletedDateTime = DateTime.Now;
        }

        public string JsonResult { get; private set; }
        public DateTime CompletedDateTime { get; set; }

        public bool Success { get { return JsonResult != null; } }
    }
}
