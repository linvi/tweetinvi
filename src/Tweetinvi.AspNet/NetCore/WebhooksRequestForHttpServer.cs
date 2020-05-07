using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Tweetinvi.Models;

namespace Tweetinvi.AspNet
{
    public class WebhooksRequestForHttpServer : IWebhooksRequest
    {
        private readonly HttpListenerContext _context;

        public WebhooksRequestForHttpServer(HttpListenerContext context)
        {
            _context = context;
        }

        public string GetPath()
        {
            var path = PathString.FromUriComponent(_context.Request.Url);
            return path.ToString();
        }

        public IDictionary<string, string[]> GetQuery()
        {
            var parameters = _context.Request.QueryString;
            var result = new Dictionary<string, string[]>();

            foreach (string name in parameters)
            {
                result.Add(name, new[] { parameters[name] });
            }

            return result;
        }

        public IDictionary<string, string[]> GetHeaders()
        {
            var headers = _context.Request.Headers;
            var result = new Dictionary<string, string[]>();

            foreach (string headerName in headers)
            {
                result.Add(headerName.ToLowerInvariant(), new []{ headers[headerName]});
            }

            return result;
        }

        private string _body;
        public Task<string> GetJsonFromBodyAsync()
        {
            if (_body != null)
            {
                return Task.FromResult(_body);
            }

            if (!_context.Request.HasEntityBody)
            {
                return Task.FromResult(null as string);
            }

            using (var bodyStream = _context.Request.InputStream)
            {
                using (var bodyReader = new StreamReader(bodyStream, _context.Request.ContentEncoding))
                {
                    _body = bodyReader.ReadToEnd();
                    return Task.FromResult(_body);
                }
            }
        }

        public void SetResponseStatusCode(int statusCode)
        {
            _context.Response.StatusCode = statusCode;
        }

        public async Task WriteInResponseAsync(string content, string contentType)
        {
            _context.Response.ContentType = contentType;

            var streamWriter = new StreamWriter(_context.Response.OutputStream);
            await streamWriter.WriteAsync(content).ConfigureAwait(false);
            await streamWriter.FlushAsync().ConfigureAwait(false);
            _context.Response.Close();
        }
    }
}