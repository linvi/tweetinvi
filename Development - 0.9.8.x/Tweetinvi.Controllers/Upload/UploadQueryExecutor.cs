using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi.Controllers.Upload
{
    public interface IUploadQueryExecutor
    {
        /// <summary>
        /// Upload a collection of media. The media uploaded info is updated.
        /// If the uploaded info is null the media failed to be uploaded.
        /// </summary>
        void UploadMedias(IEnumerable<IMedia> medias, bool forceReUpload);

        /// <summary>
        /// Upload the media associated with a Tweet if this tweet can be published
        /// </summary>
        void UploadTweetMediasBeforePublish(ITweetDTO tweetToPublish);

        /// <summary>
        /// Create and Upload a media on upload.twitter.com
        /// </summary>
        IEnumerable<IMedia> UploadBinaries(IEnumerable<byte[]> binaries);
    }

    public class UploadQueryExecutor : IUploadQueryExecutor
    {
        private readonly ITweetQueryValidator _tweetQueryValidator;
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly IFactory<IMedia> _mediaFactory;

        public UploadQueryExecutor(
            ITweetQueryValidator tweetQueryValidator,
            ITwitterAccessor twitterAccessor,
            IFactory<IMedia> mediaFactory)
        {
            _tweetQueryValidator = tweetQueryValidator;
            _twitterAccessor = twitterAccessor;
            _mediaFactory = mediaFactory;
        }

        public IEnumerable<IMedia> UploadBinaries(IEnumerable<byte[]> binaries)
        {
            var medias = new List<IMedia>();

            foreach (var binary in binaries)
            {
                var media = _mediaFactory.Create();
                media.Data = binary;
                medias.Add(media);
            }

            UploadMedias(medias);
            return medias;
        }

        public void UploadMedias(IEnumerable<IMedia> medias, bool forceReUpload = true)
        {
            if (medias == null)
            {
                return;
            }

            var mediaArray = medias.ToArray();

            if (forceReUpload)
            {
                mediaArray.ForEach(x => x.UploadedMediaInfo = null);
            }

            // Twitter documentation states that an image uploaded on Twitter is retained 60 minutes.
            // Tweetinvi is republishing a Media after 58 minutes after its first upload.
            var mediasToPublish = mediaArray.Where(x => !x.HasBeenUploaded || DateTime.Now.Subtract(x.UploadedMediaInfo.CreatedDate).TotalMinutes > 58);

            foreach (var mediaToPublish in mediasToPublish)
            {
                var singleMediaList = new[] { mediaToPublish };
                mediaToPublish.UploadedMediaInfo = _twitterAccessor.ExecutePOSTMultipartQuery<IUploadedMediaInfo>(Resources.Upload_URL, singleMediaList, "media");
            }
        }

        public void UploadTweetMediasBeforePublish(ITweetDTO tweetToPublish)
        {
            if (_tweetQueryValidator.CanTweetDTOBePublished(tweetToPublish))
            {
                UploadMedias(tweetToPublish.MediasToPublish, false);
            }
        }
    }
}