namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information read : https://developer.twitter.com/en/docs/geo/place-information/api-reference/get-geo-id-place_id
    /// </summary>
    public interface IGetPlaceParameters : ICustomRequestParameters
    {
        /// <summary>
        /// A place in the world. These IDs can be retrieved from geo/reverse_geocode.
        /// </summary>
        string PlaceId { get; set; }
    }

    /// <inheritdoc />
    public class GetPlaceParameters : CustomRequestParameters, IGetPlaceParameters
    {
        public GetPlaceParameters(string placeId)
        {
            PlaceId = placeId;
        }

        /// <inheritdoc />
        public string PlaceId { get; set; }
    }
}