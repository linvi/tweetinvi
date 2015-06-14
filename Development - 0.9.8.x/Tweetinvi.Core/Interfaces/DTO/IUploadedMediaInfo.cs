using System;

namespace Tweetinvi.Core.Interfaces.DTO
{
    public interface IUploadedMediaInfo
    {
        DateTime CreatedDate { get; }

        long MediaId { get; set; }
        string MediaIdStr { get; set; }
        int MediaSize { get; set; }
        IUploadedMediaInfoDTO MediaInfo { get; set; }
    }
}