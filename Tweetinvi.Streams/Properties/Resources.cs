using Tweetinvi.Core.Helpers;

namespace Tweetinvi.Streams.Properties
{
    internal class Resources
    {
        /// <summary>
        ///   Looks up a localized string similar to https://stream.twitter.com/1.1/statuses/filter.json?.
        /// </summary>
        public static string Stream_Filter = "https://stream.twitter.com/1.1/statuses/filter.json?";

        /// <summary>
        ///   Looks up a localized string similar to You cannot run the multiple streams at the same times.
        /// </summary>
        public static string Stream_IllegalMultipleStreams = "You cannot start a stream while it is already running.";

        /// <summary>
        ///   Looks up a localized string similar to You must specify what to do when the stream retrieves an object.
        /// </summary>
        public static string Stream_ObjectDelegateIsNull = "You must specify what to do when the stream retrieves an object";

        /// <summary>
        ///   Looks up a localized string similar to https://stream.twitter.com/1.1/statuses/sample.json?.
        /// </summary>
        public static string Stream_Sample = "https://stream.twitter.com/1.1/statuses/sample.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://userstream.twitter.com/1.1/user.json?.
        /// </summary>
        public static string Stream_UserStream = "https://userstream.twitter.com/1.1/user.json?";

        /// <summary>
        ///   Looks up a localized string similar to You cannot change the tracks while having the stream running or on pause. The stream must be stopped before updating the tracks..
        /// </summary>
        public static string TrackedStream_ModifyTracks_NotStoppedException_Description = "You cannot change the tracks while having the stream running or on pause. The stream must be stopped before updating the tracks.";

        public static string GetResourceByName(string resourceName)
        {
            return ResourcesHelper.GetResourceByType(typeof(Resources), resourceName);
        }
    }
}