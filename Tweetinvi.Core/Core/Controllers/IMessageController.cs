using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Models;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Controllers
{
    public interface IMessageController
    {
        Task<AsyncCursorResult<IEnumerable<IMessage>>> GetLatestMessages(int count = TweetinviConsts.MESSAGE_GET_COUNT);
        Task<AsyncCursorResult<IEnumerable<IMessage>>> GetLatestMessages(IGetMessagesParameters getMessagesParameters);

        Task<ITwitterResult<ICreateMessageDTO>> PublishMessage(IPublishMessageParameters parameters, ITwitterRequest request);
        Task<ITwitterResult> DestroyMessage(IDeleteMessageParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IGetMessageDTO>> GetMessage(IGetMessageParameters parameters, ITwitterRequest request);
    }
}