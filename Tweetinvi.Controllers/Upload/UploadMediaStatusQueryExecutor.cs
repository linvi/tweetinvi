using System;
using System.Threading.Tasks;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Controllers.Upload
{
    public interface IUploadMediaStatusQueryExecutor
    {
        Task<ITwitterResult<IUploadedMediaInfo>> GetMediaStatus(IMedia media, ITwitterRequest request);
    }

    public class UploadMediaStatusQueryExecutor : IUploadMediaStatusQueryExecutor
    {
        private readonly ITwitterAccessor _twitterAccessor;

        public UploadMediaStatusQueryExecutor(ITwitterAccessor twitterAccessor)
        {
            _twitterAccessor = twitterAccessor;
        }

        public async Task<ITwitterResult<IUploadedMediaInfo>> GetMediaStatus(IMedia media, ITwitterRequest request)
        {
            if (!media.HasBeenUploaded)
            {
                throw new InvalidOperationException(Resources.Exception_Upload_Status_NotUploaded);
            }

            if (media.UploadedMediaInfo.ProcessingInfo == null)
            {
                throw new InvalidOperationException(Resources.Exception_Upload_Status_No_ProcessingInfo);
            }

            request.Query.Url = $"https://upload.twitter.com/1.1/media/upload.json?command=STATUS&media_id={media.Id}";
            request.Query.HttpMethod = HttpMethod.GET;
            
            return await _twitterAccessor.ExecuteRequest<IUploadedMediaInfo>(request).ConfigureAwait(false);
        }

        
    }
}