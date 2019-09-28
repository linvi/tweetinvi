using System.Threading.Tasks;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;

namespace Tweetinvi.Controllers.Friendship
{
    public interface IFriendshipQueryExecutor
    {
        Task<long[]> GetUserIdsWhoseRetweetsAreMuted();
    }

    public class FriendshipQueryExecutor : IFriendshipQueryExecutor
    {

        private readonly IFriendshipQueryGenerator _friendshipQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;

        public FriendshipQueryExecutor(
            IFriendshipQueryGenerator friendshipQueryGenerator,
            ITwitterAccessor twitterAccessor)
        {
            _twitterAccessor = twitterAccessor;
            _friendshipQueryGenerator = friendshipQueryGenerator;
        }

        public Task<long[]> GetUserIdsWhoseRetweetsAreMuted()
        {
            string query = _friendshipQueryGenerator.GetUserIdsWhoseRetweetsAreMutedQuery();
            return _twitterAccessor.ExecuteGETQuery<long[]>(query);
        }
    }
}