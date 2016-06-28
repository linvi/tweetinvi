using System;
using System.Net.Http;
using System.Threading.Tasks;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using HttpMethod = System.Net.Http.HttpMethod;

namespace Tweetinvi.WebLogic
{
    public class HttpClientWebHelper : IHttpClientWebHelper
    {
        public async Task<HttpResponseMessage> GetHttpResponse(ITwitterQuery twitterQuery, ITwitterClientHandler handler = null)
        {
            using (var client = GetHttpClient(twitterQuery, handler))
            {
                client.Timeout = twitterQuery.Timeout;

                var httpMethod = new HttpMethod(twitterQuery.HttpMethod.ToString());

                if (twitterQuery.HttpContent == null)
                {
                    return await client.SendAsync(new HttpRequestMessage(httpMethod, twitterQuery.QueryURL)).ConfigureAwait(false);
                }
                else
                {
                    if (httpMethod != HttpMethod.Post)
                    {
                        throw new ArgumentException("Cannot send HttpContent in a WebRequest that is not POST.");
                    }

                    return await client.PostAsync(twitterQuery.QueryURL, twitterQuery.HttpContent).ConfigureAwait(false);
                }
            }
        }

        public HttpClient GetHttpClient(ITwitterQuery twitterQuery, ITwitterClientHandler twitterHandler = null)
        {
            var handler = (twitterHandler as TwitterClientHandler) ?? new TwitterClientHandler();
            handler.TwitterQuery = twitterQuery;

            var client = new HttpClient(handler)
            {
                Timeout = twitterQuery.Timeout,
            };

            return client;
        }
    }
}