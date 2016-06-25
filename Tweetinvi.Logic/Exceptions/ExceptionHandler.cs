using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Web;
using Tweetinvi.Events;
using Tweetinvi.Exceptions;

namespace Tweetinvi.Logic.Exceptions
{
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly ITwitterExceptionFactory _twitterExceptionFactory;

        private readonly object _lockExceptionInfos = new object();
        private readonly List<ITwitterException> _exceptionInfos;
        public event EventHandler<GenericEventArgs<ITwitterException>> WebExceptionReceived;
        public bool SwallowWebExceptions { get; set; }
        public bool LogExceptions { get; set; }

        public ExceptionHandler(ITwitterExceptionFactory twitterExceptionFactory)
        {
            _twitterExceptionFactory = twitterExceptionFactory;
            _exceptionInfos = new List<ITwitterException>();
            SwallowWebExceptions = true;
            LogExceptions = true;
        }

        public IEnumerable<ITwitterException> ExceptionInfos
        {
            get { return _exceptionInfos; }
        }

        public ITwitterException LastExceptionInfos
        {
            get
            {
                lock (_lockExceptionInfos)
                {
                    return _exceptionInfos.LastOrDefault();
                }
            }
        }

        public void ClearLoggedExceptions()
        {
            lock (_lockExceptionInfos)
            {
                _exceptionInfos.Clear();
            }
        }

        public TwitterException AddWebException(WebException webException, string url)
        {
            var twitterException = GenerateTwitterException(webException, url);

            AddTwitterException(twitterException);

            // Cannot throw from an interface :(
            return twitterException;
        }

        public TwitterException TryLogWebException(WebException webException, string url)
        {
            var twitterException = GenerateTwitterException(webException, url);

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

        public TwitterException AddFailedWebRequestResult(IWebRequestResult webRequestResult)
        {
            var twitterException = GenerateTwitterException(webRequestResult);

            AddTwitterException(twitterException);
            
            return twitterException;
        }

        public TwitterException TryLogFailedWebRequestResult(IWebRequestResult webRequestResult)
        {
            var twitterException = GenerateTwitterException(webRequestResult);

            if (LogExceptions)
            {
                AddTwitterException(twitterException);
            }

            return twitterException;
        }

        public TwitterException GenerateTwitterException(ITwitterExceptionInfo[] exceptionInfos, string url)
        {
            return _twitterExceptionFactory.Create(exceptionInfos, url);
        }

        public TwitterException GenerateTwitterException(WebException webException, string url)
        {
            return _twitterExceptionFactory.Create(webException, url);
        }

        public TwitterException GenerateTwitterException(IWebRequestResult webRequestResult)
        {
            return _twitterExceptionFactory.Create(webRequestResult);
        }

        public void AddTwitterException(ITwitterException twitterException)
        {
            lock (_lockExceptionInfos)
            {
                _exceptionInfos.Add(twitterException);
            }

            this.Raise(WebExceptionReceived, twitterException);
        }
    }
}