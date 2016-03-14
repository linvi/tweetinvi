using System;

namespace Tweetinvi.Core.Parameters
{
    public class TweetSearchFilterAttribute : Attribute
    {
        public TweetSearchFilterAttribute(string filterName)
        {
            FilterName = filterName;
        }

        public string FilterName { get; private set; }
    }

    /// <summary>
    /// List of filters that can be used to retrieve tweets.
    /// </summary>
    [Flags]
    public enum TweetSearchFilters
    {
        None = 1,
        Hashtags = 2,
        Links = 4,
        Images = 8,
        [TweetSearchFilter("native_video")]
        NativeVideo = 16,
        News = 32,
        Periscope = 64,
        Replies = 128,
        Safe = 256,
        Twimg = 512,
        Verified = 1024,
        Videos = 2048,
        Vine = 4096
    }
}