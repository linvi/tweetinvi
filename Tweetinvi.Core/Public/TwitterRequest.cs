using Tweetinvi.Core.Web;
using Tweetinvi.Models;

namespace Tweetinvi
{
    public class TwitterRequest
    {
        public ITwitterQuery Query { get; set; }
        public TwitterRequestConfig Config { get; set; }
        public ITwitterClientHandler TwitterClientHandler { get; set; }
    }
}
