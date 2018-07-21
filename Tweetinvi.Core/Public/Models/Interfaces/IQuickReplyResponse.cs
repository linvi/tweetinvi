using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi.Models;

namespace Tweetinvi.Models
{
    public interface IQuickReplyResponse
    {
        QuickReplyType Type { get; }
        string Metadata { get; }
    }
}
