using System;
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

        public TwitterException AddWebException(WebException webException, string url, ITwitterCredentials credentials)
        {
            var twitterException = GenerateTwitterException(webException, url, credentials);

            AddTwitterException(twitterException);

            // Cannot throw from an interface :(
            return twitterException;
        }

        public TwitterException TryLogWebException(WebException webException, string url,
            ITwitterCredentials credentials)
        {
            var twitterException = GenerateTwitterException(webException, url, credentials);

            if (LogExceptions)
            {
                AddTwitterException(twitterException);
            }

            return twitterException;
        }

        public TwitterException TryLogExceptionInfos(ITwitterExceptionInfo[] exceptionInfos, string url,
            ITwitterCredentials credentials)
        {
            var twitterException = _twitterExceptionFactory.Create(exceptionInfos, url, credentials);

            if (LogExceptions)
            {
                AddTwitterException(twitterException);
            }

            return twitterException;
        }

        public TwitterException AddFailedWebRequestResult(IWebRequestResult webRequestResult,
            ITwitterCredentials credentials)
        {
            var twitterException = GenerateTwitterException(webRequestResult, credentials);

            AddTwitterException(twitterException);
            
            return twitterException;
        }

        public TwitterException TryLogFailedWebRequestResult(IWebRequestResult webRequestResult,
            ITwitterCredentials credentials)
        {
            var twitterException = GenerateTwitterException(webRequestResult, credentials);

            if (LogExceptions)
            {
                AddTwitterException(twitterException);
            }

            return twitterException;
        }

        public TwitterException GenerateTwitterException(ITwitterExceptionInfo[] exceptionInfos, string url,
            ITwitterCredentials credentials)
        {
            return _twitterExceptionFactory.Create(exceptionInfos, url, credentials);
        }

        public TwitterException GenerateTwitterException(WebException webException, string url,
            ITwitterCredentials credentials)
        {
            return _twitterExceptionFactory.Create(webException, url, credentials);
        }

        public TwitterException GenerateTwitterException(IWebRequestResult webRequestResult,
            ITwitterCredentials credentials)
        {
            return _twitterExceptionFactory.Create(webRequestResult, credentials);
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