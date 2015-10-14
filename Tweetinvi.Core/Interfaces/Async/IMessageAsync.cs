using System.Threading.Tasks;

namespace Tweetinvi.Core.Interfaces.Async
{
    public interface IMessageAsync
    {
        Task<bool> DestroyAsync();
    }
}