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

        TwitterException AddFailedWebRequestResult(ITwitterResponse twitterResponse, ITwitterQuery twitterQuery);
        TwitterException TryLogFailedWebRequestResult(ITwitterResponse twitterResponse, ITwitterQuery twitterQuery);

        TwitterException TryLogExceptionInfos(ITwitterExceptionInfo[] exceptionInfos, string url);

        TwitterException GenerateTwitterException(WebException webException, ITwitterQuery twitterQuery);
        TwitterException GenerateTwitterException(WebException webException, ITwitterQuery twitterQuery, int statusCode);
			
        TwitterException GenerateTwitterException(ITwitterResponse twitterResponse, ITwitterQuery twitterQuery);
        void AddTwitterException(ITwitterException twitterException);
    }
}