using System.Net.Http;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.WebLogic;

namespace Tweetinvi.Core.Helpers
{
    public interface IHttpClientWebHelper
    {
        Task<HttpResponseMessage> GetHttpResponse(ITwitterQuery twitterQuery, HttpContent httpContent = null, TwitterClientHandler handler = null);
        HttpClient GetHttpClient(ITwitterQuery twitterQuery, TwitterClientHandler handler = null);
    }
}