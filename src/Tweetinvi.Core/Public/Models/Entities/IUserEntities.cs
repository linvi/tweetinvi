namespace Tweetinvi.Models.Entities
{
    public interface IUserEntities
    {
        /// <summary>
        /// Website metadata
        /// </summary>
        IWebsiteEntity Website { get; set; }

        /// <summary>
        /// User description
        /// </summary>
        IDescriptionEntity Description { get; set; }
    }
}