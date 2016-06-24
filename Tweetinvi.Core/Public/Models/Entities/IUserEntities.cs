namespace Tweetinvi.Models.Entities
{
    public interface IUserEntities
    {
        IWebsiteEntity Website { get; set; }
        IDescriptionEntity Description { get; set; }
    }
}