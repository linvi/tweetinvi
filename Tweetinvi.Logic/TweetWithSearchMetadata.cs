using Tweetinvi.Core;
using Tweetinvi.Core.Client;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Logic
{
    public class TweetWithSearchMetadata : Tweet, ITweetWithSearchMetadata
    {
        private ITweetWithSearchMetadataDTO _tweetWithSearchMetadataDTO;

        public TweetWithSearchMetadata(
            ITweetWithSearchMetadataDTO tweetDTO,
            ITwitterExecutionContext executionContext,
            ITweetController tweetController, 
            ITweetFactory tweetFactory, 
            IUserFactory userFactory,
            ITweetinviSettingsAccessor tweetinviSettingsAccessor) 
            : base(tweetDTO, null, executionContext, tweetController, tweetFactory, userFactory, tweetinviSettingsAccessor)
        {
            _tweetWithSearchMetadataDTO = tweetDTO;
        }

        public ITweetFromSearchMetadata SearchMetadata
        {
            get { return _tweetWithSearchMetadataDTO.TweetFromSearchMetadata; }
        }
    }
}