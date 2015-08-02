using System.Collections.Generic;
using Tweetinvi.Core.Enum;

namespace Tweetinvi.Logic
{
    public class UploadTwitterQuery : TwitterQuery
    {
        public UploadTwitterQuery(string queryURL, HttpMethod httpMethod)
            : base(queryURL, httpMethod)
        {
        }

        public string ContentId { get; set; }
        List<byte[]>  Binaries { get; set; }
    }
}