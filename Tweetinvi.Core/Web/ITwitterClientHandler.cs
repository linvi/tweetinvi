using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Web
{
    public interface ITwitterClientHandler
    {
        ITwitterQuery TwitterQuery { get; set; }
    }
}
