using System.Threading.Tasks;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.V2.Responses;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Controllers.User
{
    public interface IUsersV2QueryExecutor
    {
        Task<ITwitterResult<UserResponseDTO>> GetUserAsync(IGetUserByIdV2Parameters parameters, ITwitterRequest request);
        Task<ITwitterResult<UsersResponseDTO>> GetUsersAsync(IGetUsersByIdV2Parameters parameters, ITwitterRequest request);
        Task<ITwitterResult<UserResponseDTO>> GetUserAsync(IGetUserByUsernameV2Parameters parameters, ITwitterRequest request);
        Task<ITwitterResult<UsersResponseDTO>> GetUsersAsync(IGetUsersByUsernameV2Parameters parameters, ITwitterRequest request);
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

        public Task<ITwitterResult<UserResponseDTO>> GetUserAsync(IGetUserByIdV2Parameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _tweetQueryGenerator.GetUserQuery(parameters);
            return _twitterAccessor.ExecuteRequestAsync<UserResponseDTO>(request);
        }

        public Task<ITwitterResult<UsersResponseDTO>> GetUsersAsync(IGetUsersByIdV2Parameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _tweetQueryGenerator.GetUsersQuery(parameters);
            return _twitterAccessor.ExecuteRequestAsync<UsersResponseDTO>(request);
        }

        public Task<ITwitterResult<UserResponseDTO>> GetUserAsync(IGetUserByUsernameV2Parameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _tweetQueryGenerator.GetUserQuery(parameters);
            return _twitterAccessor.ExecuteRequestAsync<UserResponseDTO>(request);
        }

        public Task<ITwitterResult<UsersResponseDTO>> GetUsersAsync(IGetUsersByUsernameV2Parameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _tweetQueryGenerator.GetUsersQuery(parameters);
            return _twitterAccessor.ExecuteRequestAsync<UsersResponseDTO>(request);
        }
    }
}