using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.V2;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Controllers.User
{
    public interface IUsersV2QueryExecutor
    {
        Task<ITwitterResult<UserV2Response>> GetUserAsync(IGetUserByIdV2Parameters parameters, ITwitterRequest request);
        Task<ITwitterResult<UsersV2Response>> GetUsersAsync(IGetUsersByIdV2Parameters parameters, ITwitterRequest request);
        Task<ITwitterResult<UserV2Response>> GetUserAsync(IGetUserByUsernameV2Parameters parameters, ITwitterRequest request);
        Task<ITwitterResult<UsersV2Response>> GetUsersAsync(IGetUsersByUsernameV2Parameters parameters, ITwitterRequest request);
    }

    public class UsersV2QueryExecutor : IUsersV2QueryExecutor
    {
        private readonly IUsersV2QueryGenerator _tweetQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;

        public UsersV2QueryExecutor(
            IUsersV2QueryGenerator tweetQueryGenerator,
            ITwitterAccessor twitterAccessor)
        {
            _tweetQueryGenerator = tweetQueryGenerator;
            _twitterAccessor = twitterAccessor;
        }

        public Task<ITwitterResult<UserV2Response>> GetUserAsync(IGetUserByIdV2Parameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _tweetQueryGenerator.GetUserQuery(parameters);
            return _twitterAccessor.ExecuteRequestAsync<UserV2Response>(request);
        }

        public Task<ITwitterResult<UsersV2Response>> GetUsersAsync(IGetUsersByIdV2Parameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _tweetQueryGenerator.GetUsersQuery(parameters);
            return _twitterAccessor.ExecuteRequestAsync<UsersV2Response>(request);
        }

        public Task<ITwitterResult<UserV2Response>> GetUserAsync(IGetUserByUsernameV2Parameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _tweetQueryGenerator.GetUserQuery(parameters);
            return _twitterAccessor.ExecuteRequestAsync<UserV2Response>(request);
        }

        public Task<ITwitterResult<UsersV2Response>> GetUsersAsync(IGetUsersByUsernameV2Parameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _tweetQueryGenerator.GetUsersQuery(parameters);
            return _twitterAccessor.ExecuteRequestAsync<UsersV2Response>(request);
        }
    }
}