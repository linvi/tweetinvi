using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.Interfaces;

namespace Tweetinvi.Factories.Tweet
{
    public interface ITweetFactoryQueryExecutor
    {
        ITweetDTO CreateTweetDTO(string text);
        Task<ITwitterResult<ITweetDTO>> GetTweetDTO(long tweetId, ITwitterRequest request);
        Task<ITwitterResult<ITweetDTO[]>> GetTweetDTOs(long[] tweetIds, ITwitterRequest request);
    }

    public class TweetFactoryQueryExecutor : ITweetFactoryQueryExecutor
    {
        private readonly ITweetQueryGenerator _tweetQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly IFactory<ITweetDTO> _tweetDTOUnityFactory;

        public TweetFactoryQueryExecutor(
            ITweetQueryGenerator tweetQueryGenerator,
            ITwitterAccessor twitterAccessor, 
            IFactory<ITweetDTO> tweetDTOUnityFactory)
        {
            _tweetQueryGenerator = tweetQueryGenerator;
            _twitterAccessor = twitterAccessor;
            _tweetDTOUnityFactory = tweetDTOUnityFactory;
        }

        public async Task<ITwitterResult<ITweetDTO>> GetTweetDTO(long tweetId, ITwitterRequest request)
        {
            request.Query.Url = _tweetQueryGenerator.GetTweetQuery(tweetId, request.ExecutionContext);

            return await _twitterAccessor.ExecuteRequest<ITweetDTO>(request);
        }

        public async Task<ITwitterResult<ITweetDTO[]>> GetTweetDTOs(long[] tweetIds, ITwitterRequest request)
        {
            var maxSize = request.ExecutionContext.Limits.Tweets.GetTweetsRequestMaxSize;

            if (tweetIds.Length > maxSize)
            {
                throw new TwitterLimitException($"tweetIds cannot contain more than {maxSize} elements.", "Limits.Tweets.GetTweetsRequestMaxSize");
            }

            var tweetIdsArray = tweetIds.Distinct().ToArray();

            request.Query.Url = _tweetQueryGenerator.GetTweetsQuery(tweetIdsArray);

            var result = await _twitterAccessor.ExecuteRequest<ITweetDTO[]>(request);

            return result;
        }

        public ITweetDTO CreateTweetDTO(string text)
        {
            var tweetDTO = _tweetDTOUnityFactory.Create();
            tweetDTO.Text = text;

            return tweetDTO;
        }
    }
}