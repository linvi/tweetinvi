# Twitter Labs

Twitter Labs is the next version of the Twitter API.\
Tweetinvi 5.0 does not provide a wrapper around this new version of the API.

You can use the [direct api calls](../twitter-api/direct-api-calls) to perform requests and parse the results on your own.

## Example

``` c#
// get labs api response
var getTweetResponse = await client.Execute.RequestAsync(request =>
{
    request.Url = "https://api.twitter.com/labs/2/tweets/1138505981460193280?expansions=attachments.media_keys&tweet.fields=created_at";
});

var responseContent = JsonConvert.DeserializeObject<dynamic>(getTweetResponse.RawResult);
var tweet = responseContent.data;
var tweetId = tweet.id;
```