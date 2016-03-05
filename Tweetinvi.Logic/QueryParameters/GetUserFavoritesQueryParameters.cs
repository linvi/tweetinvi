using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Parameters.QueryParameters;

namespace Tweetinvi.Logic.QueryParameters
{
    public class GetUserFavoritesQueryParameters : IGetUserFavoritesQueryParameters
    {
        public GetUserFavoritesQueryParameters(IUserIdentifier user, IGetUserFavoritesParameters parameters = null)
        {
            UserIdentifier = user;
            Parameters = parameters ?? new GetUserFavoritesParameters();
        }

        public GetUserFavoritesQueryParameters(string screenName, IGetUserFavoritesParameters parameters = null)
        {
            UserIdentifier = new UserIdentifier(screenName);
            Parameters = parameters ?? new GetUserFavoritesParameters();
        }

        public GetUserFavoritesQueryParameters(long userId, IGetUserFavoritesParameters parameters = null)
        {
            UserIdentifier = new UserIdentifier(userId);
            Parameters = parameters ?? new GetUserFavoritesParameters();
        }

        public IUserIdentifier UserIdentifier { get; set; }
        public IGetUserFavoritesParameters Parameters { get; set; }
    }
}