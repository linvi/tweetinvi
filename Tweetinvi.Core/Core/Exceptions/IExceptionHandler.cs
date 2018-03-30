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

        TwitterException AddWebException(WebException webException, ITwitterQuery twitterQuery);
        TwitterException TryLogWebException(WebException webException, ITwitterQuery twitterQuery);

        TwitterException AddFailedWebRequestResult(IWebRequestResult webRequestResult, ITwitterQuery twitterQuery);
        TwitterException TryLogFailedWebRequestResult(IWebRequestResult webRequestResult, ITwitterQuery twitterQuery);

        TwitterException TryLogExceptionInfos(ITwitterExceptionInfo[] exceptionInfos, string url);

        TwitterException GenerateTwitterException(WebException webException, ITwitterQuery twitterQuery);
        TwitterException GenerateTwitterException(WebException webException, ITwitterQuery twitterQuery, int statusCode);
			
        TwitterException GenerateTwitterException(IWebRequestResult webRequestResult, ITwitterQuery twitterQuery);
        void AddTwitterException(ITwitterException twitterException);
    }
}