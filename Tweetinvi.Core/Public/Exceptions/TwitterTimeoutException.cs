using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Models.Interfaces;

namespace Tweetinvi.Exceptions
{
    public interface ITwitterTimeoutException : ITwitterException
    {
    }

    public class TwitterTimeoutException : TwitterException, ITwitterTimeoutException
    {
        public TwitterTimeoutException(ITwitterRequest request)
            : base(request, $"{request.Query.Url} request timed out.")
        {
            Timeout = request.Query.Timeout;
            TwitterDescription = $"Twitter was not able to perform your query within the Timeout limit of {request.Query.Timeout.TotalMilliseconds} ms.";
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