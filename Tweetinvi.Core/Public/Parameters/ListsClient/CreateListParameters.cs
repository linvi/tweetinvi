using Tweetinvi.Models;

namespace Tweetinvi.Parameters.ListsClient
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-create
    /// </summary>
    /// <inheritdoc />
    public interface ICreateTwitterListParameters : ICustomRequestParameters
    {
        /// <summary>
        /// The name for the list. A list's name must start with a letter and can consist only of
        /// 25 or fewer letters, numbers, "-", or "_" characters.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Whether your list is public or private. Values can be public or private.
        /// If no mode is specified the list will be public.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// The description to give the list.
        /// </summary>
        PrivacyMode? PrivacyMode { get; set; }
    }

    /// <inheritdoc />
    public class CreateTwitterListParameters : CustomRequestParameters, ICreateTwitterListParameters
    {
        public CreateTwitterListParameters(string name)
        {
            Name = name;
        }

        /// <inheritdoc />
        public string Name { get; set; }
        /// <inheritdoc />
        public string Description { get; set; }
        /// <inheritdoc />
        public PrivacyMode? PrivacyMode { get; set; }
    }
}