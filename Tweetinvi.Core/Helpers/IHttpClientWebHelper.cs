using System.Net.Http;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Web;

namespace Tweetinvi.Core.Helpers
{
    public interface IHttpClientWebHelper
    {
        Task<HttpResponseMessage> GetHttpResponse(ITwitterQuery twitterQuery, HttpContent httpContent = null, ITwitterClientHandler handler = null);
        HttpClient GetHttpClient(ITwitterQuery twitterQuery, ITwitterClientHandler handler = null);
    }
}