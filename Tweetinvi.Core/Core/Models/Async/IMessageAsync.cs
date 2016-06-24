using System.Threading.Tasks;

namespace Tweetinvi.Core.Models.Async
{
    public interface IMessageAsync
    {
        /// <summary>
        /// Destroy the message.
        /// </summary>
        Task<bool> DestroyAsync();
    }
}