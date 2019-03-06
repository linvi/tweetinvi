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
        public static IExceptionHandler ExceptionHandlerInstance { get; }

        static ExceptionHandler()
        {
            ExceptionHandlerInstance = TweetinviContainer.Resolve<IExceptionHandler>();
        }

        /// <summary>
        /// Notify that a WebException has been received and that the ExceptionHandler is going to process it.
        /// </summary>
        public static event EventHandler<GenericEventArgs<ITwitterException>> TwitterExceptionRaised
        {
            add { ExceptionHandlerInstance.TwitterExceptionRaised += value; }
            remove { ExceptionHandlerInstance.TwitterExceptionRaised -= value; }
        }

        /// <summary>
        /// DEFAULT VALUE : TRUE. If set to true, WebException will no longer throw. Instead you will receive a result of null/false/0.
        /// </summary>
        public static bool OnTwitterExceptionReturnNull
        {
            get { return TweetinviConfig.ApplicationSettings.OnTwitterExceptionReturnNull; }
            set { TweetinviConfig.ApplicationSettings.OnTwitterExceptionReturnNull = value; }
        }

        /// <summary>
        /// DEFAULT VALUE : TRUE. If set to true, Exceptions will be Logged and you will be able to access them from the ExceptionHandler.
        /// </summary>
        public static bool LogExceptions
        {
            get { return ExceptionHandlerInstance.LogExceptions; }
            set { ExceptionHandlerInstance.LogExceptions = value; }
        }

        /// <summary>
        /// Returns all the Logged Exceptions.
        /// </summary>
        public static ITwitterException[] GetExceptions()
        {
            return ExceptionHandlerInstance.ExceptionInfos.ToArray();
        }

        /// <summary>
        /// Returns the last Logged Exception.
        /// </summary>
        public static ITwitterException GetLastException()
        {
            return ExceptionHandlerInstance.ExceptionInfos.LastOrDefault();
        }
		
		/// <summary>
        /// Try and remove the last exception logged
        /// </summary>
        /// <param name="e">out - Exception</param>
        /// <returns>Whether there is an exception</returns>
        public static bool TryPopException(out ITwitterException e) => ExceptionHandlerInstance.TryPopException(out e);

        /// <summary>
        /// Ask for the ExceptionHandler to handle an Exception.
        /// </summary>
        public static TwitterException AddWebException(WebException webException, ITwitterQuery twitterQuery)
        {
            return ExceptionHandlerInstance.AddWebException(webException, twitterQuery);
        }

        /// <summary>
        /// Ask for the ExceptionHandler to handle an Exception.
        /// </summary>
        public static void AddTwitterException(ITwitterException twitterException)
        {
            ExceptionHandlerInstance.AddTwitterException(twitterException);
        }

        /// <summary>
        /// Remove all the Exceptions from the Log (current thread).
        /// </summary>
        public static void ClearLoggedExceptions()
        {
            ExceptionHandlerInstance.ClearLoggedExceptions();
        }

        /// <summary>
        /// Returns a TwitterException from a WebException.
        /// </summary>
        public static ITwitterException GenerateTwitterException(WebException webException, ITwitterQuery twitterQuery)
        {
            return ExceptionHandlerInstance.GenerateTwitterException(webException, twitterQuery);
        }

        /// <summary>
        /// Get a string with the value of ALL the exception logs in the current ExceptionHandler.
        /// </summary>
        public static string GetLifetimeExceptionDetails()
        {
            var strBuilder = new StringBuilder();

            foreach (var twitterException in ExceptionHandlerInstance.ExceptionInfos)
            {
                strBuilder.Append(twitterException);
                strBuilder.Append("---");
            }

            return strBuilder.ToString();
        }
    }
}