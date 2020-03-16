using System.Threading.Tasks;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Factories
{
    public interface ISavedSearchFactory
    {
        Task<ISavedSearch> CreateSavedSearch(string searchQuery);
        Task<ISavedSearch> GetSavedSearch(long searchId);
    }
}