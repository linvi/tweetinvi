using System.Net.Http;
using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Helpers
{
    public interface IHttpClientWebHelper
    {
        Task<HttpResponseMessage> GetHttpResponse(ITwitterQuery twitterQuery, ITwitterClientHandler handler = null);
        HttpClient GetHttpClient(ITwitterQuery twitterQuery, ITwitterClientHandler handler = null);
    }
}