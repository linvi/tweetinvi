using System;
using System.Collections.Generic;
using System.Net;
using Tweetinvi.Core.Web;
using Tweetinvi.Events;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Exceptions
{
    public interface IExceptionHandler
    {
        event EventHandler<GenericEventArgs<ITwitterException>> WebExceptionReceived;

        bool SwallowWebExceptions { get; set; }
        bool LogExceptions { get; set; }

        IEnumerable<ITwitterException> ExceptionInfos { get; }
        ITwitterException LastExceptionInfos { get; }

        void ClearLoggedExceptions();

        TwitterException AddWebException(WebException webException, string url, ITwitterCredentials credentials);
        TwitterException TryLogWebException(WebException webException, string url, ITwitterCredentials credentials);

        TwitterException AddFailedWebRequestResult(IWebRequestResult webRequestResult, ITwitterCredentials credentials);
        TwitterException TryLogFailedWebRequestResult(IWebRequestResult webRequestResult, ITwitterCredentials credentials);

        TwitterException TryLogExceptionInfos(ITwitterExceptionInfo[] exceptionInfos, string url,
            ITwitterCredentials credentials);

        TwitterException GenerateTwitterException(WebException webException, string url,
            ITwitterCredentials credentials);
        TwitterException GenerateTwitterException(IWebRequestResult webRequestResult, ITwitterCredentials credentials);
        void AddTwitterException(ITwitterException twitterException);
    }
}