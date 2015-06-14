using System.Net;
using System.Threading.Tasks;

namespace Tweetinvi.Core.Interfaces.WebLogic
{
    public interface IMultipartWebRequest
    {
        HttpWebRequest WebRequest { get; }
        byte[] Content { get; }
        string GetResult();
        Task<string> GetResultAsync();
    }
}