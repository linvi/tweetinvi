using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Models;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Logic
{
    public class TweetWithSearchMetadata : Tweet, ITweetWithSearchMetadata
    {
        private ITweetWithSearchMetadataDTO _tweetWithSearchMetadataDTO;

        public TweetWithSearchMetadata(
            ITweetWithSearchMetadataDTO tweetDTO,
            TweetMode? tweetMode,
            ITweetFactory tweetFactory,
            IUserFactory userFactory)
            : base(tweetDTO, tweetMode, tweetFactory, userFactory)
        {
            _tweetWithSearchMetadataDTO = tweetDTO;
        }

        public ITweetFromSearchMetadata SearchMetadata => _tweetWithSearchMetadataDTO.TweetFromSearchMetadata;
    }
}