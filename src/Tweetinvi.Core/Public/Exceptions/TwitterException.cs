using System;
using System.IO;
using System.Net;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Models.TwitterEntities;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

namespace Tweetinvi.Exceptions
{
    public interface ITwitterExceptionFactory
    {
        TwitterException Create(ITwitterResponse twitterResponse, ITwitterRequest request);
        TwitterException Create(Exception exception, ITwitterRequest request);
        TwitterException Create(WebException webException, ITwitterRequest request);
    }

    public class TwitterExceptionFactory : ITwitterExceptionFactory
    {
        private readonly IWebExceptionInfoExtractor _webExceptionInfoExtractor;

        public TwitterExceptionFactory(IWebExceptionInfoExtractor webExceptionInfoExtractor)
        {
            _webExceptionInfoExtractor = webExceptionInfoExtractor;
        }

        public TwitterException Create(ITwitterResponse twitterResponse, ITwitterRequest request)
        {
            return new TwitterException(_webExceptionInfoExtractor, twitterResponse, request);
        }

        public TwitterException Create(Exception exception, ITwitterRequest request)
        {
            return new TwitterException(request, exception);
        }

        public TwitterException Create(WebException webException, ITwitterRequest request)
        {
            return new TwitterException(_webExceptionInfoExtractor, webException, request);
        }
    }

    /// <summary>
    /// Exception raised by the Twitter API.
    /// </summary>
    public class TwitterException : WebException, ITwitterException
    {
        public WebException WebException { get; protected set; }
        public string URL { get; }
        public int StatusCode { get; protected set; } = -1;
        public string TwitterDescription { get; protected set; }
        public DateTimeOffset CreationDate { get; }

        public string Content { get; set; }
        public ITwitterExceptionInfo[] TwitterExceptionInfos { get; protected set; }
        public ITwitterQuery TwitterQuery { get; }
        public ITwitterRequest Request { get; }

        private string _message { get; }
        public override string Message => ToString();

        public TwitterException(ITwitterRequest request, string message, Exception innerException) : base(message, innerException)
        {
            _message = message;
            Request = request;
            URL = request.Query.Url;
            CreationDate = DateTime.Now;
            TwitterQuery = request.Query;
        }

        protected TwitterException(ITwitterRequest request, string message) : this(request, message, null)
        {
        }

        public TwitterException(ITwitterRequest request, Exception innerException) : this(request, innerException?.Message, innerException)
        {
        }

        public TwitterException(
            IWebExceptionInfoExtractor webExceptionInfoExtractor,
            ITwitterResponse twitterResponse,
            ITwitterRequest request)
            : this(request, twitterResponse.ReasonPhrase)
        {
            StatusCode = twitterResponse.StatusCode;
            if (twitterResponse.ResultStream != null)
            {
                using (var reader = new StreamReader(twitterResponse.ResultStream))
                {
                    Content = reader.ReadToEnd();
                    TwitterExceptionInfos = webExceptionInfoExtractor.GetTwitterExceptionInfos(Content);
                }
            }

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
            TwitterExceptionInfos = webExceptionInfoExtractor.GetTwitterExceptionInfos(webException);
            TwitterDescription = webExceptionInfoExtractor.GetStatusCodeDescription(StatusCode);
        }

        public override string ToString()
        {
            var date = $"Date : {CreationDate.ToLocalTime()}\r\n";
            var url = URL == null ? string.Empty : $"URL : {URL}\r\n";
            var code = $"Code : {StatusCode}\r\n";
            var twitterDescription = $"Twitter documentation description : {TwitterDescription}\r\n";
            var exceptionMessage = _message == null ? "" : $"Reason : {_message}\r\n";

            var exceptionInfos = string.Empty;
            if (TwitterExceptionInfos != null && TwitterExceptionInfos.Length > 0)
            {
                exceptionInfos = "Details : ";
                foreach (var twitterExceptionInfo in TwitterExceptionInfos)
                {
                    exceptionInfos += $"{twitterExceptionInfo.Message} ({twitterExceptionInfo.Code})\r\n";
                }
            }

            return $"{exceptionMessage}{exceptionInfos}{code}{date}{url}{twitterDescription}";
        }
    }
}