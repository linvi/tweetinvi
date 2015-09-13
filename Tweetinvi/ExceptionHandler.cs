using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Interfaces.Exceptions;
using Tweetinvi.Logic.Exceptions;

namespace Tweetinvi
{
    public static class ExceptionHandler
    {
        [ThreadStatic]
        private static IExceptionHandler _exceptionHandler;
        public static IExceptionHandler CurrentThreadExceptionHandler
        {
            get
            {
                if (_exceptionHandler == null)
                {
                    Initialise();
                }

                return _exceptionHandler;
            }
        }

        public static event EventHandler<GenericEventArgs<ITwitterException>> WebExceptionReceived
        {
            add { CurrentThreadExceptionHandler.WebExceptionReceived += value; }
            remove { CurrentThreadExceptionHandler.WebExceptionReceived -= value; }
        }

        static ExceptionHandler()
        {
            Initialise();
        }

        private static void Initialise()
        {
            _exceptionHandler = TweetinviContainer.Resolve<IExceptionHandler>();
        }

        public static bool SwallowWebExceptions
        {
            get { return CurrentThreadExceptionHandler.SwallowWebExceptions; }
            set { CurrentThreadExceptionHandler.SwallowWebExceptions = value; }
        }

        public static bool LogExceptions
        {
            get { return CurrentThreadExceptionHandler.LogExceptions; }
            set { CurrentThreadExceptionHandler.LogExceptions = value; }
        }

        public static IEnumerable<ITwitterException> GetExceptions()
        {
            return CurrentThreadExceptionHandler.ExceptionInfos;
        }

        public static ITwitterException GetLastException()
        {
            return CurrentThreadExceptionHandler.ExceptionInfos.LastOrDefault();
        }

        public static TwitterException AddWebException(WebException webException, string url)
        {
            return CurrentThreadExceptionHandler.AddWebException(webException, url);
        }

        public static void AddTwitterException(ITwitterException twitterException)
        {
            CurrentThreadExceptionHandler.AddTwitterException(twitterException);
        }

        public static void ClearLoggedExceptions()
        {
            CurrentThreadExceptionHandler.ClearLoggedExceptions();
        }

        public static ITwitterException GenerateTwitterException(WebException webException, string url)
        {
            return CurrentThreadExceptionHandler.GenerateTwitterException(webException, url);
        }

        public static string GetLifetimeExceptionDetails()
        {
            StringBuilder strBuilder = new StringBuilder();
            foreach (var twitterException in _exceptionHandler.ExceptionInfos)
            {
                strBuilder.Append(twitterException);
                strBuilder.Append("---");
            }
            return strBuilder.ToString();
        }
    }
}