using System;
using Tweetinvi.Core.Enum;

namespace Tweetinvi.Core.Web
{
    public interface IHttpRequestParameters
    {
        string Query { get; set; }
        HttpMethod HttpMethod { get; set; }
        TimeSpan? Timeout { get; set; }
    }

    public class HttpRequestParameters : IHttpRequestParameters
    {
        public string Query { get; set; }
        public TimeSpan? Timeout { get; set; }
        public HttpMethod HttpMethod { get; set; }
    }
}