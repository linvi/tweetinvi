using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Factories.User
{
    public class UserFactory : IUserFactory
    {
        private readonly IUserFactoryQueryExecutor _userFactoryQueryExecutor;
        private readonly IFactory<IAuthenticatedUser> _authenticatedUserUnityFactory;
        private readonly IFactory<IUser> _userUnityFactory;
        private readonly IJsonObjectConverter _jsonObjectConverter;
        private readonly ICredentialsAccessor _credentialsAccessor;

        public UserFactory(
            IUserFactoryQueryExecutor userFactoryQueryExecutor,
            IFactory<IAuthenticatedUser> authenticatedUserUnityFactory,
            IFactory<IUser> userUnityFactory,
            IJsonObjectConverter jsonObjectConverter,
            ICredentialsAccessor credentialsAccessor)
        {
            _userFactoryQueryExecutor = userFactoryQueryExecutor;
            _authenticatedUserUnityFactory = authenticatedUserUnityFactory;
            _userUnityFactory = userUnityFactory;
            _jsonObjectConverter = jsonObjectConverter;
            _credentialsAccessor = credentialsAccessor;
        }

        // Get User
        public async Task<IAuthenticatedUser> GetAuthenticatedUser(ITwitterCredentials credentials = null, IGetAuthenticatedUserParameters parameters = null)
        {
            IUserDTO userDTO;

            if (credentials == null)
            {
                credentials = _credentialsAccessor.CurrentThreadCredentials;
                userDTO = await _userFactoryQueryExecutor.GetAuthenticatedUser(parameters);
            }
            else
            {
                userDTO = await _credentialsAccessor.ExecuteOperationWithCredentials(credentials, () =>
                {
                    return _userFactoryQueryExecutor.GetAuthenticatedUser(parameters);
                });
            }

            var authenticatedUser = GenerateAuthenticatedUserFromDTO(userDTO);

            if (authenticatedUser != null)
            {
                authenticatedUser.SetCredentials(credentials);
            }

            return authenticatedUser;
        }

        public async Task<IUser> GetUserFromId(long userId)
        {
            var userDTO = await _userFactoryQueryExecutor.GetUserDTOFromId(userId);
            return GenerateUserFromDTO(userDTO);
        }

        public async Task<IUser> GetUserFromScreenName(string userName)
        {
            var userDTO = await _userFactoryQueryExecutor.GetUserDTOFromScreenName(userName);
            return GenerateUserFromDTO(userDTO);
        }

        // Generate User from Json
        public IUser GenerateUserFromJson(string jsonUser)
        {
            var userDTO = _jsonObjectConverter.DeserializeObject<IUserDTO>(jsonUser);
            return GenerateUserFromDTO(userDTO);
        }

        public async Task<IEnumerable<IUser>> GetUsersFromIds(IEnumerable<long> userIds)
        {
            var usersDTO = await _userFactoryQueryExecutor.GetUsersDTOFromIds(userIds);
            return GenerateUsersFromDTO(usersDTO);
        }

        public async Task<IEnumerable<IUser>> GetUsersFromScreenNames(IEnumerable<string> userNames)
        {
            var usersDTO = await _userFactoryQueryExecutor.GetUsersDTOFromScreenNames(userNames);
            return GenerateUsersFromDTO(usersDTO);
        }

        // Generate DTO from id
        public IUserIdentifier GenerateUserIdentifierFromId(long userId)
        {
            return new UserIdentifier(userId);
        }

        public IUserIdentifier GenerateUserIdentifierFromScreenName(string userScreenName)
        {
            return new UserIdentifier(userScreenName);
        }

        // Generate from DTO
        public IAuthenticatedUser GenerateAuthenticatedUserFromDTO(IUserDTO userDTO)
        {
            if (userDTO == null)
            {
                return null;
            }

            var userDTOParameterOverride = _authenticatedUserUnityFactory.GenerateParameterOverrideWrapper("userDTO", userDTO);
            var user = _authenticatedUserUnityFactory.Create(userDTOParameterOverride);

            return user;
        }

        public IUser GenerateUserFromDTO(IUserDTO userDTO)
        {
            if (userDTO == null)
            {
                return null;
            }

            var parameterOverride = _userUnityFactory.GenerateParameterOverrideWrapper("userDTO", userDTO);
            var user = _userUnityFactory.Create(parameterOverride);

            return user;
        }

        public IEnumerable<IUser> GenerateUsersFromDTO(IEnumerable<IUserDTO> usersDTO)
        {
            if (usersDTO == null)
            {
                return null;
            }

            return usersDTO.Select(GenerateUserFromDTO).ToList();
        }
    }
}