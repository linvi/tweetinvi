using Tweetinvi.Models.Entities;

namespace Tweetinvi.Models.DTO
{
    public interface IAttachmentDTO
    {
        AttachmentType Type { get; set; }
        IMediaEntity Media { get; set; }
    }
}
