using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;
using Tweetinvi.Factories.Properties;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Factories.User
{
    public interface IUserFactoryQueryExecutor
    {
        Task<List<IUserDTO>> GetUsersDTOFromIds(IEnumerable<long> userIds);
        Task<List<IUserDTO>> GetUsersDTOFromScreenNames(IEnumerable<string> userScreenNames);

        Task<List<IUserDTO>> LookupUserIds(List<long> userIds);
        Task<List<IUserDTO>> LookupUserScreenNames(List<string> userName);
    }

    public class UserFactoryQueryExecutor : IUserFactoryQueryExecutor
    {
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly IUserQueryParameterGenerator _userQueryParameterGenerator;

        public UserFactoryQueryExecutor(ITwitterAccessor twitterAccessor, IUserQueryParameterGenerator userQueryParameterGenerator)
        {
            _twitterAccessor = twitterAccessor;
            _userQueryParameterGenerator = userQueryParameterGenerator;
        }

        // Get Multiple users
        public async Task<List<IUserDTO>> GetUsersDTOFromIds(IEnumerable<long> enumerableUserIds)
        {
            // Optimisation: prevent multiple enumerations
            long[] userIds = enumerableUserIds as long[] ?? enumerableUserIds.ToArray();

            var usersDTO = new List<IUserDTO>();

            for (int i = 0; i < userIds.Length; i += TweetinviConsts.USERS_LOOKUP_MAX_PER_REQ)
            {
                var userIdsToLookup = userIds.Skip(i).Take(TweetinviConsts.USERS_LOOKUP_MAX_PER_REQ).ToList();
                var retrievedUsers = await LookupUserIds(userIdsToLookup);
                usersDTO.AddRangeSafely(retrievedUsers);

                if (retrievedUsers == null)
                {
                    break;
                }
            }

            return usersDTO;
        }

        public async Task<List<IUserDTO>> GetUsersDTOFromScreenNames(IEnumerable<string> enumerableUserScreenNames)
        {
            // Optimisation: prevent multiple enumerations
            string[] userScreenNames = enumerableUserScreenNames as string[] ?? enumerableUserScreenNames.ToArray();

            var usersDTO = new List<IUserDTO>();

            for (int i = 0; i < userScreenNames.Length; i += TweetinviConsts.USERS_LOOKUP_MAX_PER_REQ)
            {
                var userScreenNamesToLookup = userScreenNames.Skip(i).Take(TweetinviConsts.USERS_LOOKUP_MAX_PER_REQ).ToList();
                var retrievedUsers = await LookupUserScreenNames(userScreenNamesToLookup);
                usersDTO.AddRangeSafely(retrievedUsers);

                if (retrievedUsers == null)
                {
                    break;
                }
            }

            return usersDTO;
        }

        // Lookup
        public Task<List<IUserDTO>> LookupUserIds(List<long> userIds)
        {
            if (userIds.Count > TweetinviConsts.USERS_LOOKUP_MAX_PER_REQ)
            {
                throw new InvalidOperationException("Cannot retrieve that quantity of users at once");
            }

            var userIdsParameter = _userQueryParameterGenerator.GenerateListOfIdsParameter(userIds);
            var query = string.Format(Resources.User_GetUsersFromIds, userIdsParameter);

            return _twitterAccessor.ExecutePOSTQuery<List<IUserDTO>>(query);
        }

        public Task<List<IUserDTO>> LookupUserScreenNames(List<string> userName)
        {
            if (userName.Count > TweetinviConsts.USERS_LOOKUP_MAX_PER_REQ)
            {
                throw new InvalidOperationException("Cannot retrieve that quantity of users at once");
            }

            var userIdsParameter = _userQueryParameterGenerator.GenerateListOfScreenNameParameter(userName);
            var query = string.Format(Resources.User_GetUsersFromNames, userIdsParameter);

            return _twitterAccessor.ExecutePOSTQuery<List<IUserDTO>>(query);
        }
    }
}