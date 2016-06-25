using System;
using System.Net.Http;
using HttpMethod = Tweetinvi.Models.HttpMethod;

namespace Tweetinvi.Core.Web
{
    public interface IHttpRequestParameters
    {
        string Query { get; set; }
        HttpMethod HttpMethod { get; set; }
        HttpContent HttpContent { get; set; }
        TimeSpan? Timeout { get; set; }
    }

    public class HttpRequestParameters : IHttpRequestParameters
    {
        public string Query { get; set; }
        public virtual HttpContent HttpContent { get; set; }
        public TimeSpan? Timeout { get; set; }
        public HttpMethod HttpMethod { get; set; }
    }
}