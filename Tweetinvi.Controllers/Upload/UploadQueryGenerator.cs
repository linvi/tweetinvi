using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Upload;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Upload
{
    public class UploadQueryGenerator : IUploadQueryGenerator
    {
        public string GetChunkedUploadInitQuery(IChunkUploadInitParameters parameters)
        {
            var initQuery = new StringBuilder(Resources.Upload_URL);

            initQuery.AddParameterToQuery("command", "INIT");
            initQuery.AddParameterToQuery("media_type", parameters.MediaType);
            initQuery.AddParameterToQuery("total_bytes", parameters.TotalBinaryLength.ToString(CultureInfo.InvariantCulture));
            initQuery.AddParameterToQuery("media_category", parameters.MediaCategory);

            if (parameters.AdditionalOwnerIds != null && parameters.AdditionalOwnerIds.Any())
            {
                var ids = string.Join(",", parameters.AdditionalOwnerIds.Select(x => x.ToString()));
                initQuery.AddParameterToQuery("additional_owners", ids);
            }

            initQuery.AddFormattedParameterToQuery(parameters.CustomRequestParameters?.FormattedCustomQueryParameters);

            return initQuery.ToString();
        }

        public string GetChunkedUploadAppendQuery(IChunkUploadAppendParameters parameters)
        {
            if (parameters.MediaId == null)
            {
                throw new ArgumentNullException($"{nameof(parameters.MediaId)}", "APPEND Media Id cannot be null. Make sure you use the media id retrieved from the INIT query.");
            }

            if (parameters.SegmentIndex == null)
            {
                throw new ArgumentNullException($"{nameof(parameters.SegmentIndex)}", "APPEND Segment index is required. Its initial value should be 0.");
            }

            var appendQuery = new StringBuilder(Resources.Upload_URL);

            appendQuery.AddParameterToQuery("command", "APPEND");
            appendQuery.AddParameterToQuery("media_id", parameters.MediaId.Value.ToString(CultureInfo.InvariantCulture));
            appendQuery.AddParameterToQuery("segment_index", parameters.SegmentIndex.Value.ToString(CultureInfo.InvariantCulture));

            appendQuery.AddFormattedParameterToQuery(parameters.CustomRequestParameters?.FormattedCustomQueryParameters);

            return appendQuery.ToString();
        }

        public string GetChunkedUploadFinalizeQuery(long mediaId, ICustomRequestParameters customRequestParameters)
        {
            var finalizeQuery = new StringBuilder(Resources.Upload_URL);

            finalizeQuery.AddParameterToQuery("command", "FINALIZE");
            finalizeQuery.AddParameterToQuery("media_id", mediaId.ToString(CultureInfo.InvariantCulture));
            finalizeQuery.AddFormattedParameterToQuery(customRequestParameters?.FormattedCustomQueryParameters);

            return finalizeQuery.ToString();
        }
    }
}