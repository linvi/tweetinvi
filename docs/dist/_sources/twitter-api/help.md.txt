# Help

> The help api provide some endpoints that let you retrieve different resources from Twitter.

## Places

``` c#
// Get a place by id
var place = await client.Help.GetPlaceAsync("df51dec6f4ee2b2c");

// Search for places
var places = await client.Help.SearchGeoAsync(new GeoSearchParameters
{
    Query = "Toronto"
});

// Search by coordinates
var places = await client.Help.SearchGeoReverseAsync(new Coordinates(37.781157, -122.398720));
```

## Twitter configuration

Twitter has some variables, like the size of urls within a tweet or the maximum size of uploads.

``` c#
var configuration = await client.Help.GetTwitterConfigurationAsync();
```

Twitter ui supports a limited set of languages.

``` c#
var languages = await client.Help.GetSupportedLanguagesAsync();
```