using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.Controllers
{
    public interface ISavedSearchController
    {
        IEnumerable<ISavedSearch> GetSavedSearches();
        bool DestroySavedSearch(ISavedSearch savedSearch);
        bool DestroySavedSearch(long searchId);
    }
}