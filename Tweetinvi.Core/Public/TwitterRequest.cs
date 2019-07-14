using System;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.Interfaces;

namespace Tweetinvi
{
    public class TwitterRequest : ITwitterRequest
    {
        private ITwitterQuery _query;

        public TwitterRequest()
        {
            Query = new TwitterQuery();
            Config = new TweetinviSettings();
        }

        public ITwitterQuery Query
        {
            get { return _query; }
            set
            {
                _query = value ?? throw new ArgumentException("Cannot set query to null");
            }
        }

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
