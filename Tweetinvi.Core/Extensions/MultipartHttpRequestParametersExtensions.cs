using System.Collections.Generic;
using Tweetinvi.Core.Web;

namespace Tweetinvi.Core.Extensions
{
    public static class MultipartHttpRequestParametersExtensions
    {
        public static IMultipartHttpRequestParameters CloneForSingleBinary(this IMultipartHttpRequestParameters parameters, byte[] binary)
        {
            return new MultipartHttpRequestParameters
            {
                Query = parameters.Query,
                Binaries = new List<byte[]> { binary },
                ContentId = parameters.ContentId,
                Timeout = parameters.Timeout,
                HttpMethod = parameters.HttpMethod
            };
        }
    }
}