using System.Threading.Tasks;
using Tweetinvi.Models.V2.Responses;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Client.V2
{
    public interface ISearchV2Client
    {
        Task<SearchTweetsResponseDTO> SearchTweetsAsync(string query);
        Task<SearchTweetsResponseDTO> SearchTweetsAsync(ISearchTweetsV2Parameters parameters);
    }
}