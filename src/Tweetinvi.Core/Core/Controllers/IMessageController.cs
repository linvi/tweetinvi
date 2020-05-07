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
        Task<ITwitterResult<ICreateMessageDTO>> PublishMessageAsync(IPublishMessageParameters parameters, ITwitterRequest request);
        Task<ITwitterResult> DestroyMessageAsync(IDeleteMessageParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IGetMessageDTO>> GetMessageAsync(IGetMessageParameters parameters, ITwitterRequest request);
        ITwitterPageIterator<ITwitterResult<IMessageCursorQueryResultDTO>> GetMessagesIterator(IGetMessagesParameters parameters, ITwitterRequest request);
    }
}