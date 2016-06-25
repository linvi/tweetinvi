using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Models;

namespace Tweetinvi.Exceptions
{
    public interface ITwitterTimeoutException : ITwitterException
    {
    }

    public class TwitterTimeoutException : TwitterException, ITwitterTimeoutException
    {
        public TwitterTimeoutException(ITwitterQuery twitterQuery) 
            : base(twitterQuery.QueryURL, string.Format("{0} web request timed out.", twitterQuery.QueryURL))
        {
            Timeout = twitterQuery.Timeout;
            TwitterDescription = string.Format("Twitter was not able to perform your query within the Timeout limit of {0} ms.", twitterQuery.Timeout.TotalMilliseconds);
            CreationDate = DateTime.Now;
        }

        public TimeSpan Timeout { get; private set; }

        public override WebException WebException { get { return null; } }
        public override int StatusCode { get { return 408; } }
        public override IEnumerable<ITwitterExceptionInfo> TwitterExceptionInfos
        {
            get
            {
                return Enumerable.Empty<ITwitterExceptionInfo>();
            }
        }
    }
}