using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Factories.Tweet
{
    public interface ITweetFactoryQueryExecutor
    {
        ITweetDTO CreateTweetDTO(string text);
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

        public ITweetDTO CreateTweetDTO(string text)
        {
            var tweetDTO = _tweetDTOUnityFactory.Create();
            tweetDTO.Text = text;

            return tweetDTO;
        }
    }
}