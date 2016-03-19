using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tweetinvi.Core.Parameters.QueryParameters;

namespace Tweetinvi.Core.Web
{
    public interface IChunkUploadAppendParameters
    {
        byte[] Binary { get; }
        string MediaType { get; }
        TimeSpan? Timeout { get; }
        int? SegmentIndex { get; set; }
    }

    public class ChunkUploadAppendParameters : IChunkUploadAppendParameters
    {
        public ChunkUploadAppendParameters(byte[] binary, string mediaType, TimeSpan? timeout)
        {
            Binary = binary;
            MediaType = mediaType;
            Timeout = timeout;
        }

        public byte[] Binary { get; private set; }
        public string MediaType { get; private set; }
        public TimeSpan? Timeout { get; private set; }
        public int? SegmentIndex { get; set; }
    }
}