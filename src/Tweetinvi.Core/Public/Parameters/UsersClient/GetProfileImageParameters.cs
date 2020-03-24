using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// Parameters to download an profile image from Twitter.
    /// </summary>
    /// <inheritdoc />
    public interface IGetProfileImageParameters : ICustomRequestParameters
    {
        /// <summary>
        /// Url of the profile image
        /// </summary>
        string ImageUrl { get; set; }

        /// <summary>
        /// Size of the image
        /// </summary>
        ImageSize ImageSize { get; set; }
    }

    /// <inheritdoc />
    public class GetProfileImageParameters : CustomRequestParameters, IGetProfileImageParameters
    {
        public GetProfileImageParameters(string imageUrl)
        {
            ImageUrl = imageUrl;
        }

        public GetProfileImageParameters(IUserDTO user)
        {
            ImageUrl = user.ProfileImageUrl;
        }

        public GetProfileImageParameters(IUser user)
        {
            ImageUrl = user.ProfileImageUrl;
        }

        public GetProfileImageParameters(IGetProfileImageParameters parameters) : base(parameters)
        {
            ImageSize = ImageSize.Normal;

            if (parameters == null) return;

            ImageSize = parameters.ImageSize;
            ImageUrl = parameters.ImageUrl;
        }

        /// <inheritdoc />
        public string ImageUrl { get; set; }
        /// <inheritdoc />
        public ImageSize ImageSize { get; set; }
    }
}
