using Tweetinvi.Core.Models;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Logic
{
    public class TweetWithSearchMetadata : Tweet, ITweetWithSearchMetadata
    {
        private readonly ITweetWithSearchMetadataDTO _tweetWithSearchMetadataDTO;

        public TweetWithSearchMetadata(ITweetWithSearchMetadataDTO tweetDTO, TweetMode? tweetMode, ITwitterClient client)
            : base(tweetDTO, tweetMode, client)
        {
            _tweetWithSearchMetadataDTO = tweetDTO;
        }

        public ITweetFromSearchMetadata SearchMetadata => _tweetWithSearchMetadataDTO.TweetFromSearchMetadata;
    }
}