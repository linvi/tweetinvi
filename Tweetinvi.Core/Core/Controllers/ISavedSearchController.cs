using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Controllers
{
    public interface ISavedSearchController
    {
        Task<IEnumerable<ISavedSearch>> GetSavedSearches();
        Task<bool> DestroySavedSearch(ISavedSearch savedSearch);
        Task<bool> DestroySavedSearch(long searchId);
    }
}