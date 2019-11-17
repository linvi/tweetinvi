using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tweetinvi.Core.Logic
{
    public interface IWebhooksRequestInfoRetriever
    {
        string GetPath();
        IDictionary<string, string[]> GetQuery();
        IDictionary<string, string[]> GetHeaders();
    }

    public interface IWebhooksRequestHandler : IWebhooksRequestInfoRetriever
    {
        Task<string> GetJsonFromBody();
        void SetResponseStatusCode(int statusCode);
        Task WriteInResponseAsync(string content, string contentType);
    }
}
