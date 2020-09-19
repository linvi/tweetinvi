using System;
using System.Net;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

namespace Tweetinvi.Exceptions
{
    public interface ITwitterExceptionFactory
    {
        TwitterException Create(ITwitterExceptionInfo[] exceptionInfos, string url);
        TwitterException Create(ITwitterExceptionInfo[] exceptionInfos, ITwitterRequest request);
        TwitterException Create(ITwitterResponse twitterResponse, ITwitterRequest request);

        TwitterException Create(WebException webException, ITwitterRequest request);
        TwitterException Create(WebException webException, ITwitterRequest request, int statusCode);
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

        public TwitterException Create(ITwitterExceptionInfo[] exceptionInfos, ITwitterRequest request)
        {
            return new TwitterException(exceptionInfos, request);
        }

        public TwitterException Create(ITwitterResponse twitterResponse, ITwitterRequest request)
        {
            return new TwitterException(_webExceptionInfoExtractor, twitterResponse, request);
        }

        public TwitterException Create(WebException webException, ITwitterRequest request)
        {
            return new TwitterException(_webExceptionInfoExtractor, webException, request);
        }

        public TwitterException Create(WebException webException, ITwitterRequest request, int statusCode)
        {
            return new TwitterException(_webExceptionInfoExtractor, webException, request, statusCode);
        }
    }

    /// <summary>
    /// Exception raised by the Twitter API.
    /// </summary>
    public class TwitterException : WebException, ITwitterException
    {
        public WebException WebException { get; protected set; }
        public string URL { get; }
        public int StatusCode { get; protected set; }
        public string TwitterDescription { get; protected set; }
        public DateTimeOffset CreationDate { get; }
        public ITwitterExceptionInfo[] TwitterExceptionInfos { get; protected set; }
        public ITwitterQuery TwitterQuery { get; }
        public ITwitterRequest Request { get; }

        protected TwitterException(ITwitterRequest request, string message) : base(message)
        {
            Request = request;
            CreationDate = DateTime.Now;
            URL = request.Query.Url;
            TwitterQuery = request.Query;
        }

        private TwitterException(ITwitterRequest request) : this(request, $"{request.Query.Url} request failed.")
        {
        }

        public TwitterException(ITwitterExceptionInfo[] exceptionInfos, string url)
        {
            CreationDate = DateTime.Now;
            TwitterExceptionInfos = exceptionInfos;
            URL = url;
        }

        public TwitterException(ITwitterExceptionInfo[] exceptionInfos, ITwitterRequest request)
            : this(request)
        {
            CreationDate = DateTime.Now;
            TwitterExceptionInfos = exceptionInfos;
        }

        public TwitterException(
            IWebExceptionInfoExtractor webExceptionInfoExtractor,
            ITwitterResponse twitterResponse,
            ITwitterRequest request)
            : this(request)
        {
            StatusCode = twitterResponse.StatusCode;
            TwitterExceptionInfos = webExceptionInfoExtractor.GetTwitterExceptionInfosFromStream(twitterResponse.ResultStream);
            TwitterDescription = webExceptionInfoExtractor.GetStatusCodeDescription(StatusCode);
        }

        public TwitterException(
            IWebExceptionInfoExtractor webExceptionInfoExtractor,
            WebException webException,
            ITwitterRequest request)
            : this(request, webException.Message)
        {
            WebException = webException;
            StatusCode = webExceptionInfoExtractor.GetWebExceptionStatusNumber(webException);
            TwitterExceptionInfos = webExceptionInfoExtractor.GetTwitterExceptionInfo(webException);
            TwitterDescription = webExceptionInfoExtractor.GetStatusCodeDescription(StatusCode);
        }

        public TwitterException(
            IWebExceptionInfoExtractor webExceptionInfoExtractor,
            WebException webException,
            ITwitterRequest request,
            int statusCode)
            : this(request, webException.Message)
        {
            WebException = webException;
            StatusCode = webExceptionInfoExtractor.GetWebExceptionStatusNumber(webException, statusCode);
            TwitterExceptionInfos = webExceptionInfoExtractor.GetTwitterExceptionInfo(webException);
            TwitterDescription = webExceptionInfoExtractor.GetStatusCodeDescription(StatusCode);
        }

        public override string ToString()
        {
            var date = $"--- Date : {CreationDate.ToLocalTime()}\r\n";
            var url = URL == null ? string.Empty : $"URL : {URL}\r\n";
            var code = $"Code : {StatusCode}\r\n";
            var description = $"Error documentation description : {TwitterDescription}\r\n";
            var exceptionMessage = $"Error message : {Message}\r\n";

            var exceptionInfos = string.Empty;

            if (TwitterExceptionInfos != null)
            {
                foreach (var twitterExceptionInfo in TwitterExceptionInfos)
                {
                    exceptionInfos += $"{twitterExceptionInfo.Message} ({twitterExceptionInfo.Code})\r\n";
                }
            }

            return $"{date}{url}{code}{description}{exceptionMessage}{exceptionInfos}";
        }
    }
}