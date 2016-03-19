using System.Collections.Generic;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Core.Web
{
    public interface IChunkUploadInitParameters
    {
        string MediaType { get; set; }
        string MediaCategory { get; set; }
        int TotalBinaryLength { get; set; }
        List<long> AdditionalOwnerIds { get; set; }
        ICustomRequestParameters CustomRequestParameters { get; set; }
    }

    public class ChunkUploadInitParameters : IChunkUploadInitParameters
    {
        public ChunkUploadInitParameters()
        {
            MediaType = "media";
            AdditionalOwnerIds = new List<long>();
            CustomRequestParameters = new CustomRequestParameters();
        }

        public string MediaType { get; set; }
        public string MediaCategory { get; set; }
        public int TotalBinaryLength { get; set; }
        public List<long> AdditionalOwnerIds { get; set; }
        public ICustomRequestParameters CustomRequestParameters { get; set; }
    }
}