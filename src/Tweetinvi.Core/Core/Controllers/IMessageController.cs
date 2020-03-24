using System.Threading.Tasks;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Controllers
{
    public interface IMessageController
    {
        Task<ITwitterResult<ICreateMessageDTO>> PublishMessage(IPublishMessageParameters parameters, ITwitterRequest request);
        Task<ITwitterResult> DestroyMessage(IDeleteMessageParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IGetMessageDTO>> GetMessage(IGetMessageParameters parameters, ITwitterRequest request);
        ITwitterPageIterator<ITwitterResult<IMessageCursorQueryResultDTO>> GetMessagesIterator(IGetMessagesParameters parameters, ITwitterRequest request);
    }
}