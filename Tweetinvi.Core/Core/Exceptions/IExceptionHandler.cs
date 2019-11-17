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

        TwitterException AddWebException(WebException webException, ITwitterRequest request);
        TwitterException TryLogWebException(WebException webException, ITwitterRequest request);

        TwitterException AddFailedWebRequestResult(ITwitterResponse twitterResponse, ITwitterRequest request);
        TwitterException TryLogFailedWebRequestResult(ITwitterResponse twitterResponse, ITwitterRequest request);

        TwitterException TryLogExceptionInfos(ITwitterExceptionInfo[] exceptionInfos, string url);

        TwitterException GenerateTwitterException(WebException webException, ITwitterRequest request);
        TwitterException GenerateTwitterException(WebException webException, ITwitterRequest request, int statusCode);
			
        TwitterException GenerateTwitterException(ITwitterResponse twitterResponse, ITwitterRequest twitterQuery);
        void AddTwitterException(ITwitterException twitterException);
    }
}