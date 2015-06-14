using System;
using System.Collections.Generic;
using System.Net;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.Logic.Exceptions
{
    public class TwitterException : WebException, ITwitterException
    {
        public virtual WebException WebException { get; protected set; }
        public virtual string URL { get; set; }
        public virtual int StatusCode { get; protected set; }
        public virtual string TwitterDescription { get; protected set; }
        public virtual DateTime CreationDate { get; protected set; }
        public virtual IEnumerable<ITwitterExceptionInfo> TwitterExceptionInfos { get; protected set; }

        protected TwitterException(string url, string message) : base(message)
        {
            URL = url;
        }

        private TwitterException(string url) : this(url, string.Format("{0} web request failed.", url))
        {
        }

        public TwitterException(
            IWebExceptionInfoExtractor webExceptionInfoExtractor,
            IWebRequestResult webRequestResult)
            : this(webRequestResult.URL)
        {
            CreationDate = DateTime.Now;
            StatusCode = webRequestResult.StatusCode;
            TwitterExceptionInfos = webExceptionInfoExtractor.GetTwitterExceptionInfosFromStream(webRequestResult.ResultStream);
            TwitterDescription = webExceptionInfoExtractor.GetStatusCodeDescription(StatusCode);
        }

        public TwitterException(
            IWebExceptionInfoExtractor webExceptionInfoExtractor,
            WebException webException,
            string url)
            : this(url)
        {
            CreationDate = DateTime.Now;
            WebException = webException;
            StatusCode = webExceptionInfoExtractor.GetWebExceptionStatusNumber(webException);
            TwitterExceptionInfos = webExceptionInfoExtractor.GetTwitterExceptionInfo(webException);
            TwitterDescription = webExceptionInfoExtractor.GetStatusCodeDescription(StatusCode);
        }

        public override string ToString()
        {
            string date = string.Format("--- Date : {0}\r\n", CreationDate.ToLocalTime());
            string url = URL == null ? String.Empty : string.Format("URL : {0}\r\n", URL);
            string code = string.Format("Code : {0}\r\n", StatusCode);
            string description = string.Format("Error documentation description : {0}\r\n", TwitterDescription);

            string exceptionInfos = string.Empty;

            if (TwitterExceptionInfos != null)
            {
                foreach (var twitterExceptionInfo in TwitterExceptionInfos)
                {
                    exceptionInfos += string.Format("{0} ({1})\r\n", twitterExceptionInfo.Message, twitterExceptionInfo.Code);
                }
            }

            return string.Format("{0}{1}{2}{3}{4}", date, url, code, description, exceptionInfos);
        }
    }
}