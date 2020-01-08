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

        public Task<ITwitterResult<IUserDTO, IUser>> GetUser(IGetUserParameters parameters)
        {
            _validator.Validate(parameters);

            return ExecuteRequest(async request =>
            {
                var twitterResult = await _userController.GetUser(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(twitterResult, userDTO =>
                {
                    var user = _userFactory.GenerateUserFromDTO(userDTO, null);

                    if (user != null)
                    {
                        user.Client = TwitterClient;
                    }

                    return user;
                });
            });
        }

        public Task<ITwitterResult<IUserDTO[], IUser[]>> GetUsers(IGetUsersParameters parameters)
        {
            _validator.Validate(parameters);

            return ExecuteRequest(async request =>
            {
                var twitterResult = await _userController.GetUsers(parameters, request).ConfigureAwait(false);

                return _twitterResultFactory.Create(twitterResult, userDTO =>
                {
                    var users = _userFactory.GenerateUsersFromDTO(userDTO, null);
                    users?.ForEach(x => x.Client = TwitterClient);
                    return users;
                });
            });
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetFriendIdsIterator(IGetFriendIdsParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _userController.GetFriendIdsIterator(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetFollowerIdsIterator(IGetFollowerIdsParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _userController.GetFollowerIdsIterator(parameters, request);
        }

        public Task<ITwitterResult<IRelationshipDetailsDTO, IRelationshipDetails>> GetRelationshipBetween(IGetRelationshipBetweenParameters parameters)
        {
            _validator.Validate(parameters);

            return ExecuteRequest(async request =>
            {
                var result = await _userController.GetRelationshipBetween(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(result, _friendshipFactory.GenerateRelationshipFromRelationshipDTO);
            });
        }

        public Task<System.IO.Stream> GetProfileImageStream(IGetProfileImageParameters parameters)
        {
            _validator.Validate(parameters);

            return ExecuteRequest(request => _userController.GetProfileImageStream(parameters, request));
        }
    }
}