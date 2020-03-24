using System.Collections.Generic;
using System.Text;
using Tweetinvi.Models;

namespace Tweetinvi.Core.QueryGenerators
{
    public interface IUserQueryParameterGenerator
    {
        string GenerateIdOrScreenNameParameter(IUserIdentifier user, string idParameterName = "user_id", string screenNameParameterName = "screen_name");
        string GenerateListOfUserIdentifiersParameter(IEnumerable<IUserIdentifier> usersIdentifiers);

        void AppendUser(StringBuilder query, IUserIdentifier user);
        void AppendUsers(StringBuilder query, IEnumerable<IUserIdentifier> users);
    }
}