using System;
using System.Collections.Generic;
using Tweetinvi.Core.Enum;

namespace Tweetinvi.Core.Web
{
    public interface IUploadQueryParameters
    {
        string Query { get; set; }
        List<byte[]>  Binaries { get; set; }
        string ContentId { get; set; }
        TimeSpan? Timeout { get; set; }
        HttpMethod HttpMethod { get; set; }
        int MaxChunkSize { get; set; }
    }

    public class UploadQueryParameters : IUploadQueryParameters
    {
        public UploadQueryParameters()
        {
            Binaries = new List<byte[]>();
            ContentId = "media";
            HttpMethod = HttpMethod.POST;
            MaxChunkSize = TweetinviConsts.UPLOAD_MAX_CHUNK_SIZE;
        }

        public string Query { get; set; }
        public List<byte[]> Binaries { get; set; }
        public string ContentId { get; set; }
        public TimeSpan? Timeout { get; set; }
        public HttpMethod HttpMethod { get; set; }
        public int MaxChunkSize { get; set; }
    }
}