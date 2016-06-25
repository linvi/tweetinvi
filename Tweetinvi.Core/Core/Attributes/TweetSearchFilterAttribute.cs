using System;

namespace Tweetinvi.Core.Attributes
{
    public class TweetSearchFilterAttribute : Attribute
    {
        public TweetSearchFilterAttribute(string filterName)
        {
            FilterName = filterName;
        }

        public string FilterName { get; private set; }
    }
}