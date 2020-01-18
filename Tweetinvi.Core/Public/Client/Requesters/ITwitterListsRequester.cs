using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters.ListsClient;

namespace Tweetinvi.Client.Requesters
{
    public interface ITwitterListsRequester
    {
        Task<ITwitterResult<ITwitterListDTO, ITwitterList>> CreateList(ICreateTwitterListParameters parameters);
    }
}