using System.Collections.Generic;
using Tweetinvi.Core.Web;

namespace Tweetinvi.Core.Extensions
{
    public static class UploadQueryParametersExtensions
    {
        public static IUploadQueryParameters CloneForSingleBinary(this IUploadQueryParameters parameters, byte[] binary)
        {
            return new UploadQueryParameters
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