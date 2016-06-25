using System;
using Tweetinvi.Core.Attributes;

namespace Tweetinvi.Parameters
{
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