using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

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
        public IAuthenticatedUser GenerateAuthenticatedUserFromDTO(IUserDTO userDTO, ITwitterCredentials credentials = null)
        {
            if (userDTO == null)
            {
                return null;
            }

            var userDTOParameterOverride = _authenticatedUserUnityFactory.GenerateParameterOverrideWrapper("userDTO", userDTO);
            var user = _authenticatedUserUnityFactory.Create(userDTOParameterOverride);

            if (credentials != null)
            {
                user.Credentials = credentials;
            }

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