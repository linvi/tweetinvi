using System.Threading.Tasks;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Factories
{
    public interface IMessageFactory
    {
        // Get existing message
        Task<IMessage> GetExistingMessage(long messageId);
    }
}