using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Parameters
{
    public interface IGetProfileImageParameters : ICustomRequestParameters
    {
        string ImageUrl { get; set; }
        ImageSize ImageSize { get; set; }
    }

    public class GetProfileImageParameters : CustomRequestParameters, IGetProfileImageParameters
    {
        public GetProfileImageParameters(string imageUrl)
        {
            ImageUrl = imageUrl;
        }

        public GetProfileImageParameters(IUserDTO user)
        {
            ImageUrl = string.IsNullOrEmpty(user.ProfileImageUrlHttps) ? user.ProfileImageUrl : user.ProfileImageUrlHttps;
        }

        public GetProfileImageParameters(IUser user)
        {
            ImageUrl = string.IsNullOrEmpty(user.ProfileImageUrlHttps) ? user.ProfileImageUrl : user.ProfileImageUrlHttps;
        }

        public GetProfileImageParameters(IGetProfileImageParameters parameters) : base(parameters)
        {
            ImageSize = ImageSize.normal;

            if (parameters == null) return;

            ImageSize = parameters.ImageSize;
            ImageUrl = parameters.ImageUrl;
        }

        public string ImageUrl { get; set; }
        public ImageSize ImageSize { get; set; }
    }
}
