using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public interface IMessageRequester
    {
        Task<ITwitterResult<ICreateMessageDTO, IMessage>> PublishMessage(IPublishMessageParameters parameters);
        Task<ITwitterResult> DestroyMessage(IDeleteMessageParameters parameters);
        Task<ITwitterResult<IGetMessageDTO, IMessage>> GetMessage(IGetMessageParameters parameters);
    }
}