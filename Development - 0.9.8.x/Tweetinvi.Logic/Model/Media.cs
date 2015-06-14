using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi.Logic.Model
{
    public class Media : IMedia
    {
        public string Name { get; set; }
        public byte[] Data { get; set; }

        public long? MediaId
        {
            get
            {
                if (HasBeenUploaded)
                {
                    return UploadedMediaInfo.MediaId;
                }

                return null;
            }
        }

        public bool HasBeenUploaded { get { return UploadedMediaInfo != null; }}
        public IUploadedMediaInfo UploadedMediaInfo { get; set; }
        
        public IMedia CloneWithoutMediaInfo(IMedia source)
        {
            return new Media
            {
                Name = source.Name,
                Data = source.Data
            };
        }
    }
}