using System.Threading.Tasks;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Web
{

    public interface ITwitterAccessor
    {
        Task<ITwitterResult> ExecuteRequestAsync(ITwitterRequest request);
        Task<ITwitterResult<T>> ExecuteRequestAsync<T>(ITwitterRequest request) where T : class;
        Task PrepareTwitterRequestAsync(ITwitterRequest request);
    }
}