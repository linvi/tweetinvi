using Tweetinvi.Core.Interfaces.Models.Entities.ExtendedEntities;

namespace Tweetinvi.Logic.TwitterEntities.ExtendedEntities
{
    public class VideoEntityVariant : IVideoEntityVariant
    {
        public int Bitrate { get; set; }
        public string ContentType { get; set; }
        public string URL { get; set; }
    }
}