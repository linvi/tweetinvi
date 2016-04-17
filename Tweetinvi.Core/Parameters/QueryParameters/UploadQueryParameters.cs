using System;
using System.Collections.Generic;

namespace Tweetinvi.Core.Parameters.QueryParameters
{
    /// <summary>
    /// https://dev.twitter.com/rest/media/uploading-media
    /// </summary>
    public interface IUploadQueryParameters
    {
        /// <summary>
        /// Binaries that you want to publish
        /// </summary>
        List<byte[]> Binaries { get; set; }

        /// <summary>
        /// Type of element that you want to publish.
        /// This will be used as the ContenType in the HttpRequest.
        /// </summary>
        string MediaType { get; set; }

        /// <summary>
        /// Maximum size of a chunk size (in bytes) for a single upload.
        /// </summary>
        int MaxChunkSize { get; set; }

        /// <summary>
        /// Timeout after which a chunk request will fail.
        /// </summary>
        TimeSpan? Timeout { get; set; }

        /// <summary>
        /// User Ids who are allowed to use the uploaded media.
        /// </summary>
        List<long> AdditionalOwnerIds { get; set; }

        /// <summary>
        /// Additional parameters to use during the upload INIT HttpRequest.
        /// </summary>
        ICustomRequestParameters InitCustomRequestParameters { get; set; }

        /// <summary>
        /// Additional parameters to use during the upload APPEND HttpRequest.
        /// </summary>
        ICustomRequestParameters AppendCustomRequestParameters { get; set; }
    }

    /// <summary>
    /// https://dev.twitter.com/rest/media/uploading-media
    /// </summary>
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