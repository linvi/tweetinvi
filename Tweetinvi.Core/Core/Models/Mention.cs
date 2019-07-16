using Tweetinvi.Core;
using Tweetinvi.Core.Client;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Logic
{
    public class Mention : Tweet, IMention
    {
        public Mention(
            ITweetDTO tweetDTO,
            ITwitterExecutionContext executionContext,
            ITweetController tweetController,
            ITweetFactory tweetFactory,
            IUserFactory userFactory,
            ITweetinviSettingsAccessor tweetinviSettingsAccessor)

                : base(tweetDTO, 
                       executionContext.TweetMode,
                       executionContext,
                       tweetController,
                       tweetFactory,
                       userFactory,
                       tweetinviSettingsAccessor)
        {
            // Default constructor inheriting from the default Tweet constructor
        }

        public string Annotations { get; set; }
    }
}