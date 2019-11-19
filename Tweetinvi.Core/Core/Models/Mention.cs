using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Models;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Logic
{
    public class Mention : Tweet, IMention
    {
        public Mention(
            ITweetDTO tweetDTO,
            TweetMode? tweetMode,
            ITweetFactory tweetFactory,
            IUserFactory userFactory)
            : base(tweetDTO,
                tweetMode,
                tweetFactory,
                userFactory)
        {
            // Default constructor inheriting from the default Tweet constructor
        }

        public string Annotations { get; set; }
    }
}