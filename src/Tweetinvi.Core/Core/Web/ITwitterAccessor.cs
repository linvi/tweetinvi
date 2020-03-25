using System.Threading.Tasks;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Web
{

    public interface ITwitterAccessor
    {
        Task<ITwitterResult> ExecuteRequest(ITwitterRequest request);
        Task<ITwitterResult<T>> ExecuteRequest<T>(ITwitterRequest request) where T : class;
        Task<byte[]> DownloadBinary(ITwitterRequest request);
        Task PrepareTwitterRequest(ITwitterRequest request);
    }
}