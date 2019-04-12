using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.Interfaces;

namespace Tweetinvi
{
    public class TwitterRequest : ITwitterRequest
    {
        public TwitterRequest()
        {
            Query = new TwitterQuery();
            Config = new TweetinviSettings();
        }

        public ITwitterQuery Query { get; set; }
        public ITweetinviSettings Config { get; set; }
        public ITwitterClientHandler TwitterClientHandler { get; set; }

        public ITwitterRequest Clone()
        {
            return new TwitterRequest
            {
                Query = Query?.Clone(),
                Config = Config.Clone(),
                TwitterClientHandler = TwitterClientHandler
            };
        }
    }
}
