using Tweetinvi.Core.Web;

namespace Tweetinvi.Models.Interfaces
{
    public interface ITwitterRequest
    {
        ITwitterQuery Query { get; set; }
        ITweetinviSettings Config { get; set; }
        ITwitterClientHandler TwitterClientHandler { get; set; }
        ITwitterRequest Clone();
    }
}
