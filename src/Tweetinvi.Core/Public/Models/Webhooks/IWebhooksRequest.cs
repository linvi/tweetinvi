using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tweetinvi.Models
{
    public interface IWebhooksRequestInfoRetriever
    {
        string GetPath();
        IDictionary<string, string[]> GetQuery();
        IDictionary<string, string[]> GetHeaders();
    }

    public interface IWebhooksRequest : IWebhooksRequestInfoRetriever
    {
        Task<string> GetJsonFromBodyAsync();
        void SetResponseStatusCode(int statusCode);
        Task WriteInResponseAsync(string content, string contentType);
    }
}