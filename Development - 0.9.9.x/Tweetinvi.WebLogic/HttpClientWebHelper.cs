using System;
using System.Net.Http;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.WebLogic
{
    public interface IHttpClientWebHelper
    {
        Task<HttpResponseMessage> GetHttpResponse(ITwitterQuery twitterQuery, HttpContent httpContent = null, TwitterClientHandler handler = null);
    }

    public class HttpClientWebHelper : IHttpClientWebHelper
    {
        private readonly IOAuthWebRequestGenerator _oAuthWebRequestGenerator;

        public HttpClientWebHelper(IOAuthWebRequestGenerator oAuthWebRequestGenerator)
        {
            _oAuthWebRequestGenerator = oAuthWebRequestGenerator;
        }

        public async Task<HttpResponseMessage> GetHttpResponse(ITwitterQuery twitterQuery, HttpContent httpContent = null, TwitterClientHandler handler = null)
        {
            handler = handler ?? new TwitterClientHandler();
            handler.TwitterQuery = twitterQuery;

            using (var client = new HttpClient(handler))
            {
                if (twitterQuery.Timeout != null && twitterQuery.Timeout.Value.TotalMilliseconds > 0)
                {
                    client.Timeout = twitterQuery.Timeout.Value;
                }
                else
                {
                    client.Timeout = new TimeSpan(0, 0, 10);
                }

                var httpMethod = new HttpMethod(twitterQuery.HttpMethod.ToString());

                if (httpContent == null)
                {
                    return await client.SendAsync(new HttpRequestMessage(httpMethod, twitterQuery.QueryURL)).ConfigureAwait(false);
                }
                else
                {
                    if (httpMethod != HttpMethod.Post)
                    {
                        throw new ArgumentException("Cannot send HttpContent in a WebRequest that is not POST.");
                    }

                    return await client.PostAsync(twitterQuery.QueryURL, httpContent);
                }
            }
        }
    }
}