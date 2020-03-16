namespace Tweetinvi.Models
{
    /// <summary>
    /// Identifier allowing to identify a unique list on Twitter.
    /// </summary>
    public interface ITwitterListIdentifier
    {
        /// <summary>
        /// Id of the list.
        /// </summary>
        long Id { get; }

        /// <summary>
        /// The short name of list or a category.
        /// An owner id needs to be provided in addition to the Slug for a list identifier to be valid.
        /// </summary>
        string Slug { get; }

        /// <summary>
        /// Id of the user owning the list.
        /// A slug needs to be provided in addition to the OwnerId for a list identifier to be valid.
        /// </summary>
        long OwnerId { get; }

        /// <summary>
        /// Screen name of the user owning the list.
        /// A slug needs to be provided in addition to the OwnerScreenName for a list identifier to be valid.
        /// </summary>
        string OwnerScreenName { get; }
    }
}