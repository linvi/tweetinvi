using System.IO;
using System.Threading.Tasks;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;
using Tweetinvi.Public.Parameters.UsersClient;

namespace Tweetinvi.Core.Controllers
{
    public interface IUserController
    {
        // USERS
        Task<ITwitterResult<IUserDTO, IUser>> GetUser(IGetUserParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserDTO[], IUser[]>> GetUsers(IGetUsersParameters parameters, ITwitterRequest request);

        // FRIENDS
        ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetFriendIds(IGetFriendIdsParameters parameters, ITwitterRequest request);
        ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetFollowerIds(IGetFollowerIdsParameters parameters, ITwitterRequest request);


        Task<ITwitterResult<IRelationshipDetailsDTO>> GetRelationshipBetween(
            IGetRelationshipBetweenParameters parameters,
            ITwitterRequest request);
        Task<Stream> GetProfileImageStream(IGetProfileImageParameters parameters, ITwitterRequest request);
    }
}