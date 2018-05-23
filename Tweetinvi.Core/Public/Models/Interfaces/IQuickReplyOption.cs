using System;
using System.Collections.Generic;
using System.Text;

namespace Tweetinvi.Models
{
    public interface IQuickReplyOption
    {
        string Label { get; }
        string Description { get; }
        string Metadata { get; }
    }
}
