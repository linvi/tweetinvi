# Trends

Trends give you information about what is currently popular on Twitter.

## Searches

> Get trending searches based on location defined via Woeid

``` c#
var worldwideWoeid = 1;
var trendingSearches = await userClient.Trends.GetPlaceTrendsAtAsync(new GetTrendsAtParameters(worldwideWoeid));
```

## Locations

> Get the locations that Twitter has trending topic information for.

``` c#
// Get general trending locations
var trendingLocations = await userClient.Trends.GetTrendLocationsAsync();

// Get trending location close to a specific location
var newYorkCoordinates = new Coordinates(40.785091, -73.968285);
var trendingLocations = await userClient.Trends.GetTrendsLocationCloseToAsync(newYorkCoordinates);
```