using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi.Models.Entities;

namespace Tweetinvi.Models.DTO
{
    public interface IMessageDataDTO
    {
        string Text { get; set; }
        IMessageEntities Entities { get; set; }
        IQuickReplyDTO QuickReply { get; set; }
        IQuickReplyResponse QuickReplyResponse { get; set; }
        IAttachmentDTO Attachment { get; set; }
    }
}
