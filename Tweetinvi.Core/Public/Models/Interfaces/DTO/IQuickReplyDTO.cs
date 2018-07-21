using System;
using System.Collections.Generic;
using System.Text;

namespace Tweetinvi.Models.DTO
{
    public interface IQuickReplyDTO
    {
        QuickReplyType Type { get; set; }
        IQuickReplyOption[] Options { get; set; }
    }
}
