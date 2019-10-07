using System.Collections.Generic;
using Tweetinvi.Models;

namespace Tweetinvi.Core.QueryGenerators
{
    public interface IUserQueryParameterGenerator
    {
        string GenerateUserIdParameter(long? userId, string parameterName = "user_id");
        string GenerateScreenNameParameter(string screenName, string parameterName = "screen_name");
        string GenerateIdOrScreenNameParameter(IUserIdentifier user, string idParameterName = "user_id", string screenNameParameterName = "screen_name");

        string GenerateListOfIdsParameter(long?[] ids);
        string GenerateListOfIdsParameter(long[] ids);
        string GenerateListOfUserIdentifiersParameter(IEnumerable<IUserIdentifier> usersIdentifiers);
        string GenerateListOfScreenNameParameter(string[] usernames);
        string GenerateUserNamesParameter(IUserIdentifier[] users);
        string GenerateUserIdsParameter(IUserIdentifier[] users);
    }
}