namespace Tweetinvi
{
    /// <summary>
    /// Application wide constants.
    /// </summary>
    public static class TweetinviConsts
    {
        public static int MEDIA_CONTENT_SIZE = 24;
        public static int HTTP_LINK_SIZE = 23;
        public static int HTTPS_LINK_SIZE = 23;

        // https://dev.twitter.com/rest/reference/post/media/upload
        // https://dev.twitter.com/rest/public/uploading-media
        public static int UPLOAD_MAX_IMAGE_SIZE = 5 * 1024 * 1024;
        public static int UPLOAD_MAX_VIDEO_SIZE = 15 * 1024 * 1024;
        public static int UPLOAD_MAX_CHUNK_SIZE = 4 * 1024 * 1024;
    }
}