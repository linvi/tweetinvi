namespace Tweetinvi.Models.Entities.ExtendedEntities
{
    public interface IVideoInformationEntity
    {
        int[] AspectRatio { get; set; }

        int DurationInMilliseconds { get; set; }

        IVideoEntityVariant[] Variants { get; set; }
    }
}