namespace Tweetinvi.Models.Entities.ExtendedEntities
{
    public interface IVideoEntityVariant
    {
        int Bitrate { get; set; }
        string ContentType { get; set; }
        string URL { get; set; }
    }
}