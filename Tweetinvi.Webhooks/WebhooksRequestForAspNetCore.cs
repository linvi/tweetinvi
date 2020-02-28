using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Tweetinvi.Models;

namespace Tweetinvi.AspNet
{
    public class WebhooksRequestForAspNetCore : IWebhooksRequest
    {
        private readonly HttpContext _context;

        public WebhooksRequestForAspNetCore(HttpContext context)
        {
            _context = context;
        }

        private string _body;
        public Task<string> GetJsonFromBody()
        {
            if (_body == "")
            {
                return null;
            }

            if (_body != null)
            {
                return Task.FromResult(_body);
            }

            _context.Request.EnableRewind();
            _body = new StreamReader(_context.Request.Body).ReadToEnd();
            return Task.FromResult(_body);
        }

        public string GetPath()
        {
            return _context.Request.Path.ToString();
        }

        public IDictionary<string, string[]> GetHeaders()
        {
            return _context.Request.Headers.ToDictionary(x => x.Key.ToLowerInvariant(), x => x.Value.ToArray());
        }


        public IDictionary<string, string[]> GetQuery()
        {
            return _context.Request.Query.ToDictionary(x => x.Key.ToLowerInvariant(), x => x.Value.ToArray());
        }

        public void SetResponseStatusCode(int statusCode)
        {
            _context.Response.StatusCode = statusCode;
        }

        public Task WriteInResponseAsync(string content, string contentType)
        {
            _context.Response.ContentType = contentType;
            return _context.Response.WriteAsync(content);
        }
    }
}
