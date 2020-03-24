using System.Text;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.QueryGenerators
{
    public interface ITwitterListQueryParameterGenerator
    {
        string GenerateIdentifierParameter(ITwitterListIdentifier twitterListIdentifier);

        // User Parameters
        IGetTweetsFromListParameters CreateTweetsFromListParameters();

        // Query Parameters
        void AppendListIdentifierParameter(StringBuilder query, ITwitterListIdentifier listIdentifier);
        void AppendListIdentifierParameter(StringBuilder query, IListParameters parameters);
    }
}