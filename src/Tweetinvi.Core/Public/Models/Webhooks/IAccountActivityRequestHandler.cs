using System.Threading.Tasks;
using Tweetinvi.Streaming;

namespace Tweetinvi.Models
{
    public interface IAccountActivityRequestHandler
    {
        Task<bool> IsRequestManagedByTweetinviAsync(IWebhooksRequest request);
        Task<bool> TryRouteRequestAsync(IWebhooksRequest request);
        IAccountActivityStream GetAccountActivityStream(long userId, string environment);
    }
}