using System;
using Tweetinvi.Core.Client;
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
            ExecutionContext = new TwitterExecutionContext
            {
                RequestFactory = () => new TwitterRequest()
            };
        }

        public ITwitterQuery Query
        {
            get { return _query; }
            set
            {
                _query = value ?? throw new ArgumentException("Cannot set query to null");
            }
        }

        public ITwitterExecutionContext ExecutionContext { get; set; }
        public ITwitterClientHandler TwitterClientHandler { get; set; }

        public ITwitterRequest Clone()
        {
            return new TwitterRequest
            {
                Query = Query?.Clone(),
                TwitterClientHandler = TwitterClientHandler,
                ExecutionContext = ExecutionContext.Clone()
            };
        }
    }
}
