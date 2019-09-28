using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Controllers
{
    public interface IFriendshipController
    {
        // Retweet Muted Friends
        Task<long[]> GetUserIdsWhoseRetweetsAreMuted();
        Task<IEnumerable<IUser>> GetUsersWhoseRetweetsAreMuted();
    }
}