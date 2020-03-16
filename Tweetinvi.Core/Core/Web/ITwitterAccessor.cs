using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Tweetinvi.Core.Models.Properties;
using Tweetinvi.Models;
using HttpMethod = Tweetinvi.Models.HttpMethod;

namespace Tweetinvi.Core.Web
{

    public interface ITwitterAccessor
    {
        Task<AsyncOperation<string>> TryExecutePOSTQuery(string query);

        // Get specific type of object from path
        Task<T> ExecuteGETQueryWithPath<T>(string query, params string[] paths) where T : class;

        // Get specific type of object
        Task<T> ExecuteGETQuery<T>(string query, JsonConverter[] converters = null) where T : class;
        Task<T> ExecutePOSTQuery<T>(string query, JsonConverter[] converters = null) where T : class;

        Task<T> ExecuteQuery<T>(string query, HttpMethod method, IConsumerOnlyCredentials credentials, HttpContent httpContent = null) where T : class;
        Task<ITwitterResult> ExecuteRequest(ITwitterRequest request);
        Task<ITwitterResult<T>> ExecuteRequest<T>(ITwitterRequest request) where T : class;
    }
}