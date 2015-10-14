using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Core.Interfaces.QueryValidators
{
    public interface ITwitterListQueryValidator
    {
        // ListParameter
        bool IsListUpdateParametersValid(ITwitterListUpdateParameters parameters);
        
        // Parameters
        bool IsDescriptionParameterValid(string description);
        bool IsNameParameterValid(string name);
        
        // Identifiers
        bool IsListIdentifierValid(ITwitterListIdentifier parameters);
        bool IsOwnerScreenNameValid(string ownerScreeName);
        bool IsOwnerIdValid(long? ownerId);
    }
}