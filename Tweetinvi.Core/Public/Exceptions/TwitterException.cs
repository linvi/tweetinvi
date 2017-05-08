using System;
using System.Collections.Generic;
using System.Net;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

namespace Tweetinvi.Exceptions
{
    public interface ITwitterExceptionFactory
    {
        TwitterException Create(ITwitterExceptionInfo[] exceptionInfos, string url, ITwitterCredentials credentials);
        TwitterException Create(IWebRequestResult webRequestResult, ITwitterCredentials credentials);
        TwitterException Create(WebException webException, string url, ITwitterCredentials credentials);
    }

    public class TwitterExceptionFactory : ITwitterExceptionFactory
    {
        private readonly IWebExceptionInfoExtractor _webExceptionInfoExtractor;

        public TwitterExceptionFactory(IWebExceptionInfoExtractor webExceptionInfoExtractor)
        {
            _webExceptionInfoExtractor = webExceptionInfoExtractor;
        }

        public TwitterException Create(ITwitterExceptionInfo[] exceptionInfos, string url, ITwitterCredentials credentials)
        {
            return new TwitterException(exceptionInfos, url, credentials);
        }

        public TwitterException Create(IWebRequestResult webRequestResult, ITwitterCredentials credentials)
        {
            return new TwitterException(_webExceptionInfoExtractor, webRequestResult, credentials);
        }

        public TwitterException Create(WebException webException, string url, ITwitterCredentials credentials)
        {
            return new TwitterException(_webExceptionInfoExtractor, webException, url, credentials);
        }
    }

    public class TwitterException : WebException, ITwitterException
    {
        public virtual WebException WebException { get; protected set; }
        public virtual string URL { get; set; }
        public virtual int StatusCode { get; protected set; }
        public virtual string TwitterDescription { get; protected set; }
        public virtual DateTime CreationDate { get; protected set; }
        public virtual IEnumerable<ITwitterExceptionInfo> TwitterExceptionInfos { get; protected set; }
        public virtual ITwitterCredentials Credentials { get; protected set; }

        protected TwitterException(string url, ITwitterCredentials credentials, string message)
            : base(message)
        {
            CreationDate = DateTime.Now;
            URL = url;
            Credentials = credentials;
        }

        private TwitterException(string url, ITwitterCredentials credentials)
            : this(url, credentials, string.Format("{0} web request failed.", url))
        {
        }

        public TwitterException(ITwitterExceptionInfo[] exceptionInfos, string url, ITwitterCredentials credentials)
            : this(url, credentials)
        {
            CreationDate = DateTime.Now;
            TwitterExceptionInfos = exceptionInfos;
        }

        public TwitterException(
            IWebExceptionInfoExtractor webExceptionInfoExtractor,
            IWebRequestResult webRequestResult,
            ITwitterCredentials credentials)
            : this(webRequestResult.URL, credentials)
        {
            StatusCode = webRequestResult.StatusCode;
            TwitterExceptionInfos = webExceptionInfoExtractor.GetTwitterExceptionInfosFromStream(webRequestResult.ResultStream);
            TwitterDescription = webExceptionInfoExtractor.GetStatusCodeDescription(StatusCode);
        }

        public TwitterException(
            IWebExceptionInfoExtractor webExceptionInfoExtractor,
            WebException webException,
            string url,
            ITwitterCredentials credentials)
            : this(url, credentials, webException.Message)
        {
            WebException = webException;
            StatusCode = webExceptionInfoExtractor.GetWebExceptionStatusNumber(webException);
            TwitterExceptionInfos = webExceptionInfoExtractor.GetTwitterExceptionInfo(webException);
            TwitterDescription = webExceptionInfoExtractor.GetStatusCodeDescription(StatusCode);
        }

        public override string ToString()
        {
            string date = string.Format("--- Date : {0}\r\n", CreationDate.ToLocalTime());
            string url = URL == null ? string.Empty : string.Format("URL : {0}\r\n", URL);
            string code = string.Format("Code : {0}\r\n", StatusCode);
            string description = string.Format("Error documentation description : {0}\r\n", TwitterDescription);
            string exceptionMessage = string.Format("Error message : {0}\r\n", Message);

            string exceptionInfos = string.Empty;

            if (TwitterExceptionInfos != null)
            {
                foreach (var twitterExceptionInfo in TwitterExceptionInfos)
                {
                    exceptionInfos += string.Format("{0} ({1})\r\n", twitterExceptionInfo.Message, twitterExceptionInfo.Code);
                }
            }

            // Note: Credentials purposely excluded from ToString()
            //  Don't want users accidentally posting their credentials online when looking for help,
            //  or more generally for logs to contain sensitive data.
            //  Printing credentials anywhere must be a conscious decision made by the user.

            return string.Format("{0}{1}{2}{3}{4}{5}", date, url, code, description, exceptionMessage, exceptionInfos);
        }
    }
}