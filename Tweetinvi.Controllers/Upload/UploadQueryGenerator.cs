using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Web;

namespace Tweetinvi.Controllers.Upload
{
    public class UploadQueryGenerator : IUploadQueryGenerator
    {
        public string GetChunkedUploadInitQuery(IChunkUploadInitParameters parameters)
        {
            var initQuery = Resources.Upload_URL;

            initQuery = initQuery.AddParameterToQuery("command", "INIT");
            initQuery = initQuery.AddParameterToQuery("media_type", parameters.MediaType);
            initQuery = initQuery.AddParameterToQuery("total_bytes", parameters.TotalBinaryLength.ToString(CultureInfo.InvariantCulture));
            initQuery = initQuery.AddParameterToQuery("media_category", parameters.MediaCategory);

            if (parameters.AdditionalOwnerIds != null && parameters.AdditionalOwnerIds.Any())
            {
                var ids = string.Join("%2C", parameters.AdditionalOwnerIds.Select(x => x.ToString()));
                initQuery.AddParameterToQuery("additional_owners", ids);
            }

            var formattedParameters = parameters.CustomRequestParameters.FormattedCustomQueryParameters;
            initQuery += formattedParameters;

            return initQuery;
        }

        public string GetChunkedUploadAppendQuery(IChunkUploadAppendParameters parameters)
        {
            if (parameters.MediaId == null)
            {
                throw new ArgumentNullException("APPEND Media Id cannot be null. Make sure you use the media id retrieved from the INIT query.");
            }

            if (parameters.SegmentIndex == null)
            {
                throw new ArgumentNullException("APPEND Segment index is required. Its initial value should be 0.");
            }

            var appendQuery = Resources.Upload_URL;

            appendQuery = appendQuery.AddParameterToQuery("command", "APPEND");
            appendQuery = appendQuery.AddParameterToQuery("media_id", parameters.MediaId.Value.ToString(CultureInfo.InvariantCulture));
            appendQuery = appendQuery.AddParameterToQuery("segment_index", parameters.SegmentIndex.Value.ToString(CultureInfo.InvariantCulture));

            var formattedParameters = parameters.CustomRequestParameters.FormattedCustomQueryParameters;
            appendQuery += formattedParameters;

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