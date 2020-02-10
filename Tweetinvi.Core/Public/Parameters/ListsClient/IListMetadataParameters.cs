using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    public interface IListMetadataParameters : ICustomRequestParameters
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

    public class ListMetadataParameters : CustomRequestParameters, IListMetadataParameters
    {
        public ListMetadataParameters()
        {
        }

        public ListMetadataParameters(IListMetadataParameters parameters)
        {
            Name = parameters?.Name;
            Description = parameters?.Description;
            PrivacyMode = parameters?.PrivacyMode;
        }

        /// <inheritdoc />
        public string Name { get; set; }
        /// <inheritdoc />
        public string Description { get; set; }
        /// <inheritdoc />
        public PrivacyMode? PrivacyMode { get; set; }
    }
}