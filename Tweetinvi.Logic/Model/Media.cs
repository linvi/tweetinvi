using Tweetinvi.Logic.DTO;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Logic.Model
{
    public class Media : IEditableMedia
    {
        // Parameter used to indicate that the media is in the process
        // of being uploaded, but has not yet completed. This give developers
        // the ability to use chunked uploads from multiple computers in parrallel
        private long? _existingMediaId;

        public string Name { get; set; }
        public byte[] Data { get; set; }
        public string ContentType { get; set; }

        public long? MediaId
        {
            get
            {
                // Once the media has been uploaded there is no possible way for
                // developers to change the value of the media Id.
                // The _mediaId parameter is therefore ignored

                if (HasBeenUploaded)
                {
                    return UploadedMediaInfo.MediaId;
                }

                return _existingMediaId;
            }
            set { _existingMediaId = value; }
        }

        public bool HasBeenUploaded { get { return UploadedMediaInfo != null; } }
        public IUploadedMediaInfo UploadedMediaInfo { get; set; }

        public IMedia CloneWithoutMediaInfo(IMedia source)
        {
            return new Media
            {
                Name = source.Name,
                Data = source.Data
            };
        }

        public IMedia CloneWithoutUploadInfo()
        {
            var clone = new Media
            {
                Name = Name,
                Data = Data,
                ContentType = ContentType,
                MediaId = MediaId,
                _existingMediaId = _existingMediaId
            };

            return clone;
        }
    }
}