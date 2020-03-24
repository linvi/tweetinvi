using System;
using Tweetinvi.Events;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Upload
{
    public interface IChunkUploadAppendParameters
    {
        byte[] Binary { get; }
        string MediaType { get; }
        TimeSpan? Timeout { get; }
        int? SegmentIndex { get; set; }
        long? MediaId { get; set; }
        Action<IMediaUploadProgressChangedEventArgs> UploadProgressChanged { get; set; }
        ICustomRequestParameters CustomRequestParameters { get; set; }
    }

    public class ChunkUploadAppendParameters : IChunkUploadAppendParameters
    {
        public ChunkUploadAppendParameters(byte[] binary, string mediaType, TimeSpan? timeout)
        {
            Binary = binary;
            MediaType = mediaType;
            Timeout = timeout;

            CustomRequestParameters = new CustomRequestParameters();
        }

        public byte[] Binary { get; }
        public string MediaType { get; }
        public TimeSpan? Timeout { get; }
        public int? SegmentIndex { get; set; }
        public long? MediaId { get; set; }
        public Action<IMediaUploadProgressChangedEventArgs> UploadProgressChanged { get; set; }
        public ICustomRequestParameters CustomRequestParameters { get; set; }
    }
}