using System.IO;
using System.Threading.Tasks;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Controllers
{
    public interface IUserController
    {
        // USERS
        Task<ITwitterResult<IUserDTO>> GetUser(IGetUserParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserDTO[]>> GetUsers(IGetUsersParameters parameters, ITwitterRequest request);

        // FRIENDS
        ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetFriendIdsIterator(IGetFriendIdsParameters parameters, ITwitterRequest request);
        ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetFollowerIdsIterator(IGetFollowerIdsParameters parameters, ITwitterRequest request);


        Task<ITwitterResult<IRelationshipDetailsDTO>> GetRelationshipBetween(
            IGetRelationshipBetweenParameters parameters,
            ITwitterRequest request);
        Task<Stream> GetProfileImageStream(IGetProfileImageParameters parameters, ITwitterRequest request);
    }
}