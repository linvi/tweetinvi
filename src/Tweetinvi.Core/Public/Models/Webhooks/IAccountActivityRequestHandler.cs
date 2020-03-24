using System.Threading.Tasks;
using Tweetinvi.Streaming;

namespace Tweetinvi.Models
{
    public interface IAccountActivityRequestHandler
    {
        Task<bool> IsRequestManagedByTweetinvi(IWebhooksRequest request);
        Task<bool> TryRouteRequest(IWebhooksRequest request);
        IAccountActivityStream GetAccountActivityStream(long userId, string environment);
    }
}