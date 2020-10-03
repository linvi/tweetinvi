using System.Threading.Tasks;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.Responses;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Client.Requesters.V2
{
    public interface ISearchV2Requester
    {
        Task<ITwitterResult<SearchTweetsV2Response>> SearchTweetsAsync(ISearchTweetsV2Parameters parameters);
        ITwitterPageIterator<ITwitterResult<SearchTweetsV2Response>, string> GetSearchTweetsV2Iterator(ISearchTweetsV2Parameters parameters);
    }
}