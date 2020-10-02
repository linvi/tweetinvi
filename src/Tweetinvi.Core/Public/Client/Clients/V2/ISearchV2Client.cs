using System.Threading.Tasks;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Iterators;
using Tweetinvi.Models.V2.Responses;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Client.V2
{
    public interface ISearchV2Client
    {
        Task<SearchTweetsResponseDTO> SearchTweetsAsync(ISearchTweetsV2Parameters parameters);
        ITwitterSimpleIterator<SearchTweetsIteratorResponseDTO, string> GetSearchTweetsV2Iterator(ISearchTweetsV2Parameters parameters);
    }
}