using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Events;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;

namespace Tweetinvi
{
    /// <summary>
    /// Create, Logs and Handle Twitter WebExceptions
    /// </summary>
    public static class ExceptionHandler
    {
        private static AsyncLocal<IExceptionHandler> _exceptionHandler = new AsyncLocal<IExceptionHandler>();

        /// <summary>
        /// Current Thread ExceptionHandler
        /// </summary>
        public static IExceptionHandler CurrentThreadExceptionHandler
        {
            get
            {
                if (_exceptionHandler.Value == null)
                {
                    _exceptionHandler.Value = TweetinviContainer.Resolve<IExceptionHandler>();
                }

                return _exceptionHandler.Value;
            }
        }

        /// <summary>
        /// Notify that a WebException has been received and that the ExceptionHandler is going to process it.
        /// </summary>
        public static event EventHandler<GenericEventArgs<ITwitterException>> WebExceptionReceived
        {
            add { CurrentThreadExceptionHandler.WebExceptionReceived += value; }
            remove { CurrentThreadExceptionHandler.WebExceptionReceived -= value; }
        }

        /// <summary>
        /// DEFAULT VALUE : TRUE. If set to true, WebException will no longer throw. Instead you will receive a result of null/false/0.
        /// </summary>
        public static bool SwallowWebExceptions
        {
            get { return CurrentThreadExceptionHandler.SwallowWebExceptions; }
            set { CurrentThreadExceptionHandler.SwallowWebExceptions = value; }
        }

        /// <summary>
        /// DEFAULT VALUE : TRUE. If set to true, Exceptions will be Logged and you will be able to access them from the ExceptionHandler.
        /// </summary>
        public static bool LogExceptions
        {
            get { return CurrentThreadExceptionHandler.LogExceptions; }
            set { CurrentThreadExceptionHandler.LogExceptions = value; }
        }

        /// <summary>
        /// Returns all the Logged Exceptions.
        /// </summary>
        public static IEnumerable<ITwitterException> GetExceptions()
        {
            return CurrentThreadExceptionHandler.ExceptionInfos;
        }

        /// <summary>
        /// Returns the last Logged Exception.
        /// </summary>
        public static ITwitterException GetLastException()
        {
            return CurrentThreadExceptionHandler.ExceptionInfos.LastOrDefault();
        }

        /// <summary>
        /// Ask for the ExceptionHandler to handle an Exception.
        /// </summary>
        public static TwitterException AddWebException(WebException webException, ITwitterQuery twitterQuery)
        {
            return CurrentThreadExceptionHandler.AddWebException(webException, twitterQuery);
        }

        /// <summary>
        /// Ask for the ExceptionHandler to handle an Exception.
        /// </summary>
        public static void AddTwitterException(ITwitterException twitterException)
        {
            CurrentThreadExceptionHandler.AddTwitterException(twitterException);
        }

        /// <summary>
        /// Remove all the Exceptions from the Log (current thread).
        /// </summary>
        public static void ClearLoggedExceptions()
        {
            CurrentThreadExceptionHandler.ClearLoggedExceptions();
        }

        /// <summary>
        /// Returns a TwitterException from a WebException.
        /// </summary>
        public static ITwitterException GenerateTwitterException(WebException webException, ITwitterQuery twitterQuery)
        {
            return CurrentThreadExceptionHandler.GenerateTwitterException(webException, twitterQuery);
        }

        /// <summary>
        /// Get a string with the value of ALL the exception logs in the current ExceptionHandler.
        /// </summary>
        public static string GetLifetimeExceptionDetails()
        {
            StringBuilder strBuilder = new StringBuilder();
            foreach (var twitterException in CurrentThreadExceptionHandler.ExceptionInfos)
            {
                strBuilder.Append(twitterException);
                strBuilder.Append("---");
            }
            return strBuilder.ToString();
        }
    }
}