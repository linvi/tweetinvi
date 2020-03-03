using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public interface IMessageClient
    {
        Task<IMessage> PublishMessage(string text, long? recipientId);
        Task<IMessage> PublishMessage(IPublishMessageParameters parameters);

        Task<IMessage> GetMessage(long messageId);
        Task<IMessage> GetMessage(IGetMessageParameters parameters);

        Task DestroyMessage(long messageId);
        Task DestroyMessage(IMessage message);
        Task DestroyMessage(IDeleteMessageParameters parameters);

    }
}