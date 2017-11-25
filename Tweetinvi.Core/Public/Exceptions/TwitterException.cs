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
        TwitterException Create(ITwitterExceptionInfo[] exceptionInfos, string url);
        TwitterException Create(ITwitterExceptionInfo[] exceptionInfos, ITwitterQuery twitterQuery);
        TwitterException Create(IWebRequestResult webRequestResult, ITwitterQuery twitterQuery);

        TwitterException Create(WebException webException, ITwitterQuery twitterQuery);
        TwitterException Create(WebException webException, ITwitterQuery twitterQuery, int statusCode);
    }

    public class TwitterExceptionFactory : ITwitterExceptionFactory
    {
        private readonly IWebExceptionInfoExtractor _webExceptionInfoExtractor;

        public TwitterExceptionFactory(IWebExceptionInfoExtractor webExceptionInfoExtractor)
        {
            _webExceptionInfoExtractor = webExceptionInfoExtractor;
        }

        public TwitterException Create(ITwitterExceptionInfo[] exceptionInfos, string url)
        {
            return new TwitterException(exceptionInfos, url);
        }

        public TwitterException Create(ITwitterExceptionInfo[] exceptionInfos, ITwitterQuery twitterQuery)
        {
            return new TwitterException(exceptionInfos, twitterQuery);
        }

        public TwitterException Create(IWebRequestResult webRequestResult, ITwitterQuery twitterQuery)
        {
            return new TwitterException(_webExceptionInfoExtractor, webRequestResult, twitterQuery);
        }

        public TwitterException Create(WebException webException, ITwitterQuery twitterQuery)
        {
            return Create(webException, twitterQuery, TwitterException.DEFAULT_STATUS_CODE);
        }

        public TwitterException Create(WebException webException, ITwitterQuery twitterQuery, int defaultStatusCode)
        {
            return new TwitterException(_webExceptionInfoExtractor, webException, twitterQuery, defaultStatusCode);
        }
    }

    public class TwitterException : WebException, ITwitterException
    {
        public const int DEFAULT_STATUS_CODE = -1;

        public virtual WebException WebException { get; protected set; }
        public virtual string URL { get; set; }
        public virtual int StatusCode { get; protected set; }
        public virtual string TwitterDescription { get; protected set; }
        public virtual DateTime CreationDate { get; protected set; }
        public virtual IEnumerable<ITwitterExceptionInfo> TwitterExceptionInfos { get; protected set; }
        public virtual ITwitterQuery TwitterQuery { get; protected set; }


        protected TwitterException(ITwitterQuery twitterQuery, string message)
            : base(message)
        {
            CreationDate = DateTime.Now;
            URL = twitterQuery?.QueryURL;
            TwitterQuery = twitterQuery;
        }

        private TwitterException(ITwitterQuery twitterQuery) : this(twitterQuery, string.Format("{0} web request failed.", twitterQuery?.QueryURL))
        {
        }

        public TwitterException(ITwitterExceptionInfo[] exceptionInfos, string url) 
        {
            CreationDate = DateTime.Now;
            TwitterExceptionInfos = exceptionInfos;
            URL = url;
        }

        public TwitterException(ITwitterExceptionInfo[] exceptionInfos, ITwitterQuery twitterQuery)
            : this(twitterQuery)
        {
            CreationDate = DateTime.Now;
            TwitterExceptionInfos = exceptionInfos;
            TwitterQuery = twitterQuery;
        }

        public TwitterException(
            IWebExceptionInfoExtractor webExceptionInfoExtractor,
            IWebRequestResult webRequestResult,
            ITwitterQuery twitterQuery)
            : this(twitterQuery)
        {
            StatusCode = webRequestResult.StatusCode;
            TwitterExceptionInfos = webExceptionInfoExtractor.GetTwitterExceptionInfosFromStream(webRequestResult.ResultStream);
            TwitterDescription = webExceptionInfoExtractor.GetStatusCodeDescription(StatusCode);
        }

        public TwitterException(
            IWebExceptionInfoExtractor webExceptionInfoExtractor,
            WebException webException,
            ITwitterQuery twitterQuery,
            int defaultStatusCode = DEFAULT_STATUS_CODE)
            : this(twitterQuery, webException.Message)
        {
            WebException = webException;
            StatusCode = webExceptionInfoExtractor.GetWebExceptionStatusNumber(webException, defaultStatusCode);
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

            return string.Format("{0}{1}{2}{3}{4}{5}", date, url, code, description, exceptionMessage, exceptionInfos);
        }
    }
}