using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;
using Tweetinvi.Logic.DTO;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Factories.Tweet
{
    public interface ITweetFactoryQueryExecutor
    {
        ITweetDTO GetTweetDTO(long tweetId);
        IEnumerable<ITweetDTO> GetTweetDTOs(IEnumerable<long> tweetIds);
        ITweetDTO CreateTweetDTO(string text);
    }

    public class TweetFactoryQueryExecutor : ITweetFactoryQueryExecutor
    {
        private const int MAX_NUMBER_OF_TWEET_TO_GET_IN_A_SINGLE_QUERY = 100;

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

        public ITweetDTO GetTweetDTO(long tweetId)
        {
            string query = _tweetQueryGenerator.GetTweetQuery(tweetId);
            return _twitterAccessor.ExecuteGETQuery<TweetDTO>(query);
        }

        public IEnumerable<ITweetDTO> GetTweetDTOs(IEnumerable<long> tweetIds)
        {
            var tweetIdsArray = tweetIds.Distinct().ToArray();
            var distinctTweetDTOs = new List<ITweetDTO>();

            for (int i = 0; i < tweetIdsArray.Length; i += MAX_NUMBER_OF_TWEET_TO_GET_IN_A_SINGLE_QUERY)
            {
                var tweetIdsToAnalyze = tweetIdsArray.Skip(i).Take(MAX_NUMBER_OF_TWEET_TO_GET_IN_A_SINGLE_QUERY).ToArray();
                string query = _tweetQueryGenerator.GetTweetsQuery(tweetIdsToAnalyze);
                var tweetDTOs = _twitterAccessor.ExecuteGETQuery<IEnumerable<TweetDTO>>(query);

                if (tweetDTOs == null)
                {
                    break;
                }

                distinctTweetDTOs.AddRange(tweetDTOs);
            }

            return distinctTweetDTOs;
        }

        public ITweetDTO CreateTweetDTO(string text)
        {
            var tweetDTO = _tweetDTOUnityFactory.Create();
            tweetDTO.Text = text;

            return tweetDTO;
        }
    }
}