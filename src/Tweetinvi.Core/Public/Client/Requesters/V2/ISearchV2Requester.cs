using System.Threading.Tasks;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.V2.Responses;
using Tweetinvi.Parameters.V2.SearchesClientV2;

namespace Tweetinvi.Client.Requesters.V2
{
    public interface ISearchV2Requester
    {
        Task<ITwitterResult<SearchTweetsResponseDTO>> SearchTweetsAsync(ISearchTweetsV2Parameters parameters);
        ITwitterPageIterator<ITwitterResult<SearchTweetsResponseDTO>, string> GetSearchTweetsV2Iterator(ISearchTweetsV2Parameters parameters);
    }
}