using System.Collections.Generic;
using Tweetinvi.Core.Enum;

namespace Tweetinvi.Core.Web
{
    public interface IMultipartHttpRequestParameters : IHttpRequestParameters
    {
        IEnumerable<byte[]> Binaries { get; set; }
        string ContentId { get; set; }
    }

    public class MultipartHttpRequestParameters : HttpRequestParameters, IMultipartHttpRequestParameters
    {
        public MultipartHttpRequestParameters()
        {
            ContentId = "media";
            HttpMethod = HttpMethod.POST;
        }

        public IEnumerable<byte[]> Binaries { get; set; }
        public string ContentId { get; set; }
    }
}