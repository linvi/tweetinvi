using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Credentials.QueryJsonConverters;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public interface IInternalUsersRequester : IUsersRequester, IBaseRequester
    {
    }

    public class UsersRequester : BaseRequester, IInternalUsersRequester
    {
        private readonly IUserController _userController;
        private readonly ITwitterResultFactory _twitterResultFactory;
        private readonly IFriendshipFactory _friendshipFactory;
        private readonly IUserFactory _userFactory;
        private readonly IUsersClientRequiredParametersValidator _validator;

        public UsersRequester(
            IUserController userController, 
            ITwitterResultFactory twitterResultFactory, 
            IFriendshipFactory friendshipFactory,
            IUserFactory userFactory,
            IUsersClientRequiredParametersValidator validator)
        {
            _userController = userController;
            _twitterResultFactory = twitterResultFactory;
            _friendshipFactory = friendshipFactory;
            _userFactory = userFactory;
            _validator = validator;
        }

        public async Task<ITwitterResult<IUserDTO, IUser>> GetUser(IGetUserParameters parameters)
        {
            _validator.Validate(parameters);
            
            var request = TwitterClient.CreateRequest();
            var twitterResult = await ExecuteRequest(() => _userController.GetUser(parameters, request), request).ConfigureAwait(false);
            
            return _twitterResultFactory.Create(twitterResult, userDTO =>
            {
                var user = _userFactory.GenerateUserFromDTO(userDTO, null);
                
                if (user != null)
                {
                    user.Client = TwitterClient;
                }

                return user;
            });
        }

        public async Task<ITwitterResult<IUserDTO[], IUser[]>> GetUsers(IGetUsersParameters parameters)
        {
            _validator.Validate(parameters);
            
            var request = TwitterClient.CreateRequest();
            var result = await ExecuteRequest(() => _userController.GetUsers(parameters, request), request).ConfigureAwait(false);
            
            return _twitterResultFactory.Create(result, userDTO =>
            {
                var users = _userFactory.GenerateUsersFromDTO(userDTO, null);
                users?.ForEach(x => x.Client = TwitterClient);
                return users;
            });
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetFriendIds(IGetFriendIdsParameters parameters)
        {
            _validator.Validate(parameters);
            
            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _userController.GetFriendIds(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetFollowerIds(IGetFollowerIdsParameters parameters)
        {
            _validator.Validate(parameters);
            
            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _userController.GetFollowerIds(parameters, request);
        }

        public async Task<ITwitterResult<IRelationshipDetailsDTO, IRelationshipDetails>> GetRelationshipBetween(IGetRelationshipBetweenParameters parameters)
        {
            _validator.Validate(parameters);
            
            var request = TwitterClient.CreateRequest();
            var result = await ExecuteRequest(() => _userController.GetRelationshipBetween(parameters, request), request).ConfigureAwait(false);

            return _twitterResultFactory.Create(result, _friendshipFactory.GenerateRelationshipFromRelationshipDTO);
        }

        public Task<System.IO.Stream> GetProfileImageStream(IGetProfileImageParameters parameters)
        {
            _validator.Validate(parameters);
            
            var request = TwitterClient.CreateRequest();
            return _userController.GetProfileImageStream(parameters, request);
        }
    }
}