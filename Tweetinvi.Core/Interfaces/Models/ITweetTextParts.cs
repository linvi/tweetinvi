using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tweetinvi.Core.Interfaces.Models
{
    public interface ITweetTextParts
    {
        string Content { get; }
        string Prefix { get; }
        string[] Mentions { get; }
    }
}
