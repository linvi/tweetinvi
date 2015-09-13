using System.Globalization;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces.QueryGenerators;

namespace Tweetinvi.Controllers.Upload
{
    public class UploadQueryGenerator : IUploadQueryGenerator
    {
        public string GetChunkedUploadInitQuery(string mediaType, long totalBinaryLength)
        {
            var initQuery = Resources.Upload_URL;

            initQuery = initQuery.AddParameterToQuery("command", "INIT");
            initQuery = initQuery.AddParameterToQuery("media_type", mediaType);
            initQuery = initQuery.AddParameterToQuery("total_bytes", totalBinaryLength.ToString(CultureInfo.InvariantCulture));

            return initQuery;
        }

        public string GetChunkedUploadAppendQuery(long mediaId, int segmentIndex)
        {
            var appendQuery = Resources.Upload_URL;

            appendQuery = appendQuery.AddParameterToQuery("command", "APPEND");
            appendQuery = appendQuery.AddParameterToQuery("media_id", mediaId.ToString());
            appendQuery = appendQuery.AddParameterToQuery("segment_index", segmentIndex.ToString(CultureInfo.InvariantCulture));

            return appendQuery;
        }

        public string GetChunkedUploadFinalizeQuery(long mediaId)
        {
            var finalizeQuery = Resources.Upload_URL;

            finalizeQuery = finalizeQuery.AddParameterToQuery("command", "FINALIZE");
            finalizeQuery = finalizeQuery.AddParameterToQuery("media_id", mediaId.ToString(CultureInfo.InvariantCulture));

            return finalizeQuery;
        }
    }
}