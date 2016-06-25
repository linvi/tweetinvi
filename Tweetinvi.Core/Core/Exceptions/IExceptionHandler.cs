using System;
using System.Collections.Generic;
using System.Net;
using Tweetinvi.Core.Web;
using Tweetinvi.Events;
using Tweetinvi.Exceptions;

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

        TwitterException AddWebException(WebException webException, string url);
        TwitterException TryLogWebException(WebException webException, string url);

        TwitterException AddFailedWebRequestResult(IWebRequestResult webRequestResult);
        TwitterException TryLogFailedWebRequestResult(IWebRequestResult webRequestResult);

        TwitterException TryLogExceptionInfos(ITwitterExceptionInfo[] exceptionInfos, string url);

        TwitterException GenerateTwitterException(WebException webException, string url);
        TwitterException GenerateTwitterException(IWebRequestResult webRequestResult);
        void AddTwitterException(ITwitterException twitterException);
    }
}