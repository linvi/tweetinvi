using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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
        /// <summary>
        /// Current Thread ExceptionHandler
        /// </summary>
        public static IExceptionHandler GlobalExceptionHandler { get; private set; }

        /// <summary>
        /// Notify that a WebException has been received and that the ExceptionHandler is going to process it.
        /// </summary>
        public static event EventHandler<GenericEventArgs<ITwitterException>> WebExceptionReceived
        {
            add { GlobalExceptionHandler.WebExceptionReceived += value; }
            remove { GlobalExceptionHandler.WebExceptionReceived -= value; }
        }

        static ExceptionHandler()
        {
            Initialise();
        }

        private static void Initialise()
        {
            GlobalExceptionHandler = TweetinviContainer.Resolve<IExceptionHandler>();
        }

        /// <summary>
        /// DEFAULT VALUE : TRUE. If set to true, WebException will no longer throw. Instead you will receive a result of null/false/0.
        /// </summary>
        public static bool SwallowWebExceptions
        {
            get { return GlobalExceptionHandler.SwallowWebExceptions; }
            set { GlobalExceptionHandler.SwallowWebExceptions = value; }
        }

        /// <summary>
        /// DEFAULT VALUE : TRUE. If set to true, Exceptions will be Logged and you will be able to access them from the ExceptionHandler.
        /// </summary>
        public static bool LogExceptions
        {
            get { return GlobalExceptionHandler.LogExceptions; }
            set { GlobalExceptionHandler.LogExceptions = value; }
        }

        /// <summary>
        /// Returns all the Logged Exceptions.
        /// </summary>
        public static IEnumerable<ITwitterException> GetExceptions()
        {
            return GlobalExceptionHandler.ExceptionInfos;
        }

        /// <summary>
        /// Returns the last Logged Exception.
        /// </summary>
        public static ITwitterException GetLastException()
        {
            return GlobalExceptionHandler.ExceptionInfos.LastOrDefault();
        }

        /// <summary>
        /// Ask for the ExceptionHandler to handle an Exception.
        /// </summary>
        public static TwitterException AddWebException(WebException webException, ITwitterRequest request)
        {
            return GlobalExceptionHandler.AddWebException(webException, request);
        }

        /// <summary>
        /// Ask for the ExceptionHandler to handle an Exception.
        /// </summary>
        public static void AddTwitterException(ITwitterException twitterException)
        {
            GlobalExceptionHandler.AddTwitterException(twitterException);
        }

        /// <summary>
        /// Remove all the Exceptions from the Log (current thread).
        /// </summary>
        public static void ClearLoggedExceptions()
        {
            GlobalExceptionHandler.ClearLoggedExceptions();
        }

        /// <summary>
        /// Returns a TwitterException from a WebException.
        /// </summary>
        public static ITwitterException GenerateTwitterException(WebException webException, ITwitterRequest request)
        {
            return GlobalExceptionHandler.GenerateTwitterException(webException, request);
        }

        /// <summary>
        /// Get a string with the value of ALL the exception logs in the current ExceptionHandler.
        /// </summary>
        public static string GetLifetimeExceptionDetails()
        {
            StringBuilder strBuilder = new StringBuilder();
            foreach (var twitterException in GlobalExceptionHandler.ExceptionInfos)
            {
                strBuilder.Append(twitterException);
                strBuilder.Append("---");
            }
            return strBuilder.ToString();
        }
    }
}