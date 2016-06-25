namespace Tweetinvi.Models
{
    public enum PlaceType
    {
        // details here : http://developer.yahoo.com/geo/geoplanet/guide/concepts.html#placetypes
        Undefined = 0,
        Admin = 1,
        Poi = 2,
        Neighborhood = 5,
        City = 6,
        Town = 7,
        AdministrativeArea1 = 8,
        AdministrativeArea2 = 9,
        AdministrativeArea3 = 10,
        PostalCode = 11,
        Country = 12,
        SuperName = 19, // Multiple countries, regions (latin america), historical location (USSR)
        Suburb = 22,
        Colloquial = 24,
        Continent = 29,
        TimeZone = 31,
    }
}