using System;
using System.Collections.Generic;

namespace Tweetinvi.Core.Parameters.QueryParameters
{
    public interface IUploadQueryParameters
    {
        List<byte[]> Binaries { get; set; }
        string MediaType { get; set; }
        int MaxChunkSize { get; set; }
        TimeSpan? Timeout { get; set; }
        List<long> AdditionalOwnerIds { get; set; }

        ICustomRequestParameters InitCustomRequestParameters { get; set; }
        ICustomRequestParameters AppendCustomRequestParameters { get; set; }
    }

    public class UploadQueryParameters : IUploadQueryParameters
    {
        public UploadQueryParameters()
        {
            Binaries = new List<byte[]>();
            MediaType = "media";
            MaxChunkSize = TweetinviConsts.UPLOAD_MAX_CHUNK_SIZE;
            AdditionalOwnerIds = new List<long>();

            InitCustomRequestParameters = new CustomRequestParameters();
            AppendCustomRequestParameters = new CustomRequestParameters();
        }

        public List<byte[]> Binaries { get; set; }
        public string MediaType { get; set; }
        public int MaxChunkSize { get; set; }
        public TimeSpan? Timeout { get; set; }
        public List<long> AdditionalOwnerIds { get; set; }

        public ICustomRequestParameters InitCustomRequestParameters { get; set; }
        public ICustomRequestParameters AppendCustomRequestParameters { get; set; }
    }
}