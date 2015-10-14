using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.QueryGenerators
{
    public interface IUserQueryParameterGenerator
    {
        string GenerateUserIdParameter(long userId, string parameterName = "user_id");
        string GenerateScreenNameParameter(string screenName, string parameterName = "screen_name");
        string GenerateIdOrScreenNameParameter(IUserIdentifier userIdentifier, string idParameterName = "user_id", string screenNameParameterName = "screen_name");

        string GenerateListOfIdsParameter(IEnumerable<long> ids);
        string GenerateListOfUserIdentifiersParameter(IEnumerable<IUserIdentifier> usersIdentifiers);
        string GenerateListOfScreenNameParameter(IEnumerable<string> userNames);
    }
}