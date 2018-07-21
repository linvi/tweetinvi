using System;
using System.Collections.Generic;
using System.Text;

namespace Tweetinvi.Models
{
    public interface IApp
    {
        long Id { get; }
        string Name { get; }
        string Url { get; }
    }
}
