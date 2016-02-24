using System.Threading.Tasks;

namespace Tweetinvi.Core.Interfaces.Async
{
    public interface IMessageAsync
    {
        /// <summary>
        /// Destroy the message.
        /// </summary>
        Task<bool> DestroyAsync();
    }
}