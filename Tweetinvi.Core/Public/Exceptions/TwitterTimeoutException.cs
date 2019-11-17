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
        public TwitterTimeoutException(ITwitterRequest request) 
            : base(request, $"{request.Query.Url} request timed out.")
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            TwitterDescription = $"Twitter was not able to perform your query within the Timeout limit of {request.Query.Timeout.TotalMilliseconds} ms.";
        }

        public override WebException WebException => null;
        public override int StatusCode => 408;

        public override IEnumerable<ITwitterExceptionInfo> TwitterExceptionInfos => Enumerable.Empty<ITwitterExceptionInfo>();
    }
}