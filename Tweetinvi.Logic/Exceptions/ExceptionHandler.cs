using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Web;
using Tweetinvi.Events;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;

namespace Tweetinvi.Logic.Exceptions
{
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly ITwitterExceptionFactory _twitterExceptionFactory;
        private readonly ConcurrentStack<ITwitterException> _exceptions;

        public event EventHandler<GenericEventArgs<ITwitterException>> WebExceptionReceived;
        public bool SwallowWebExceptions { get; set; }
        public bool LogExceptions { get; set; }

        public ExceptionHandler(ITwitterExceptionFactory twitterExceptionFactory)
        {
            _twitterExceptionFactory = twitterExceptionFactory;
            _exceptions = new ConcurrentStack<ITwitterException>();
            SwallowWebExceptions = true;
            LogExceptions = true;
        }

        public IEnumerable<ITwitterException> ExceptionInfos => _exceptions;

        [Obsolete("Maintained for backwards compatibility. Use TryPeekException")]
        public ITwitterException LastExceptionInfos => TryPeekException(out ITwitterException e) ? e : null;

        public bool TryPopException(out ITwitterException e) => _exceptions.TryPop(out e);
        public bool TryPeekException(out ITwitterException e) => _exceptions.TryPeek(out e);

        public void ClearLoggedExceptions() => _exceptions.Clear();

        public TwitterException AddWebException(WebException webException, ITwitterQuery twitterQuery)
        {
            var twitterException = GenerateTwitterException(webException, twitterQuery);

            AddTwitterException(twitterException);

            return twitterException;
        }

        public TwitterException TryLogWebException(WebException webException, ITwitterQuery twitterQuery)
        {
            var twitterException = GenerateTwitterException(webException, twitterQuery);

            if (LogExceptions)
            {
                AddTwitterException(twitterException);
            }

            return twitterException;
        }

        public TwitterException TryLogExceptionInfos(ITwitterExceptionInfo[] exceptionInfos, string url)
        {
            var twitterException = _twitterExceptionFactory.Create(exceptionInfos, url);

            if (LogExceptions)
            {
                AddTwitterException(twitterException);
            }

            return twitterException;
        }

        public TwitterException GenerateTwitterException(WebException webException, ITwitterQuery twitterQuery)
        {
            return GenerateTwitterException(webException, twitterQuery, -1);
        }

        public TwitterException AddFailedWebRequestResult(IWebRequestResult webRequestResult, ITwitterQuery twitterQuery)
        {
            var twitterException = GenerateTwitterException(webRequestResult, twitterQuery);

            AddTwitterException(twitterException);
            
            return twitterException;
        }

        public TwitterException TryLogFailedWebRequestResult(IWebRequestResult webRequestResult, ITwitterQuery twitterQuery)
        {
            var twitterException = GenerateTwitterException(webRequestResult, twitterQuery);

            if (LogExceptions)
            {
                AddTwitterException(twitterException);
            }

            return twitterException;
        }

        public TwitterException GenerateTwitterException(ITwitterExceptionInfo[] exceptionInfos, ITwitterQuery twitterQuery)
        {
            return _twitterExceptionFactory.Create(exceptionInfos, twitterQuery);
        }

        public TwitterException GenerateTwitterException(
			WebException webException, 
			ITwitterQuery twitterQuery,
            int statusCode)
        {
            return _twitterExceptionFactory.Create(webException, twitterQuery, statusCode);
        }

        public TwitterException GenerateTwitterException(IWebRequestResult webRequestResult, ITwitterQuery twitterQuery)
        {
            return _twitterExceptionFactory.Create(webRequestResult, twitterQuery);
        }

        public void AddTwitterException(ITwitterException twitterException)
        {
            _exceptions.Push(twitterException);

            this.Raise(WebExceptionReceived, twitterException);
        }
    }
}