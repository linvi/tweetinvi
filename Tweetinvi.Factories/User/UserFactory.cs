using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

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
        public IAuthenticatedUser GetAuthenticatedUser(ITwitterCredentials credentials = null, IGetAuthenticatedUserParameters parameters = null)
        {
            IUserDTO userDTO;

            if (credentials == null)
            {
                credentials = _credentialsAccessor.CurrentThreadCredentials;
                userDTO = _userFactoryQueryExecutor.GetAuthenticatedUser(parameters);
            }
            else
            {
                userDTO = _credentialsAccessor.ExecuteOperationWithCredentials(credentials, () =>
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

        public IUser GetUserFromId(long userId)
        {
            var userDTO = _userFactoryQueryExecutor.GetUserDTOFromId(userId);
            return GenerateUserFromDTO(userDTO);
        }

        public IUser GetUserFromScreenName(string userName)
        {
            var userDTO = _userFactoryQueryExecutor.GetUserDTOFromScreenName(userName);
            return GenerateUserFromDTO(userDTO);
        }

        // Generate User from Json
        public IUser GenerateUserFromJson(string jsonUser)
        {
            var userDTO = _jsonObjectConverter.DeserializeObject<IUserDTO>(jsonUser);
            return GenerateUserFromDTO(userDTO);
        }

        public IEnumerable<IUser> GetUsersFromIds(IEnumerable<long> userIds)
        {
            var usersDTO = _userFactoryQueryExecutor.GetUsersDTOFromIds(userIds);
            return GenerateUsersFromDTO(usersDTO);
        }

        public IEnumerable<IUser> GetUsersFromScreenNames(IEnumerable<string> userNames)
        {
            var usersDTO = _userFactoryQueryExecutor.GetUsersDTOFromScreenNames(userNames);
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