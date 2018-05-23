using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi.Models.Entities;

namespace Tweetinvi.Models.DTO
{
    public interface IAttachmentDTO
    {
        AttachmentType Type { get; }
        IMediaEntity Media { get; }
    }
}
