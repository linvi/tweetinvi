using Tweetinvi.Core.Client;
using Tweetinvi.Core.Web;

namespace Tweetinvi.Models
{
    public interface ITwitterRequest
    {
        ITwitterQuery Query { get; set; }
        ITwitterClientHandler TwitterClientHandler { get; set; }
        ITwitterExecutionContext ExecutionContext { get; set; }
    }
}
