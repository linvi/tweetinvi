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
        private readonly IOAuthWebRequestGeneratorFactory _oAuthWebRequestGeneratorFactory;

        public HttpClientWebHelper(IOAuthWebRequestGeneratorFactory oAuthWebRequestGeneratorFactory)
        {
            _oAuthWebRequestGeneratorFactory = oAuthWebRequestGeneratorFactory;
        }

        public async Task<HttpResponseMessage> GetHttpResponseAsync(ITwitterQuery twitterQuery, ITwitterClientHandler handler = null)
        {
            using (var client = GetHttpClient(twitterQuery, handler))
            {
                client.Timeout = twitterQuery.Timeout;

                var httpMethod = new HttpMethod(twitterQuery.HttpMethod.ToString());

                if (twitterQuery.HttpContent == null)
                {
                    return await client.SendAsync(new HttpRequestMessage(httpMethod, twitterQuery.Url)).ConfigureAwait(false);
                }
                else
                {
                    if (httpMethod == HttpMethod.Post)
                    {
                        return await client.PostAsync(twitterQuery.Url, twitterQuery.HttpContent).ConfigureAwait(false);
                    }

                    if (httpMethod == HttpMethod.Put)
                    {
                        return await client.PutAsync(twitterQuery.Url, twitterQuery.HttpContent).ConfigureAwait(false);
                    }

                    throw new ArgumentException("Cannot send HttpContent in a WebRequest that is not POST or PUT.");
                }
            }
        }

        public HttpClient GetHttpClient(ITwitterQuery twitterQuery, ITwitterClientHandler twitterHandler = null)
        {
            var oAuthWebRequestGenerator = _oAuthWebRequestGeneratorFactory.Create();
            var handler = (twitterHandler as TwitterClientHandler) ?? new TwitterClientHandler(oAuthWebRequestGenerator);
            handler.TwitterQuery = twitterQuery;

            var client = new HttpClient(handler)
            {
                Timeout = twitterQuery.Timeout,
            };

            return client;
        }
    }
}