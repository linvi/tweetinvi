# Iterators

Twitter API has multiple endpoints that exposes pages api. The implementation might differ from endpoint to endpoint.\
Tweetinvi hides this complexity to you via `iterators`.

<p class="iterator-available" style="margin-bottom: 10px;">
    When you see a tag 
    <span style="padding: 0px 5px;">
    <span class="iterator" style="float: none; padding: 1px 10px 3px 10px;">
        <b>iterators</b> 
        <span class="fa fa-arrow-circle-right" style="margin-left: 10px;"></span>
    </span>
    </span>
    it informs you that the endpoint can be reached via an iterator.
</p>

## Iterator

> Iterators let you request multiple pages until no more items are available.

* `MoveToNextPage()` request twitter for the next page of results
* `NextCursor` is updated after each call to `MoveToNextPage`. It can be used to start a new request at this position.
* `Completed` is updated after each call to `MoveToNextPage`. It is marked as true when no more results are available.

## Iterator page

> Pages are enumerators and contain elements returned by a single request.

* `ToArray()` returns the content of the page.
* `NextCursor` contains the next cursor to use for following requests.
* `IsLastPage` informs whether the page was the last one that could be reached.

## Example

``` c#
var timelineTweets = new List<ITweet>();
var timelineIterator = client.Timelines.GetHomeTimelineIterator();

while (!timelineIterator.Completed)
{
    var page = await timelineIterator.MoveToNextPage(); 
    timelineTweets.AddRange(page);
}
```

## Restart from a previous cursor

As mentioned you can restart from a previous cursor state.

``` c#
var firstFriendIdsIterator = client.Users.GetFollowerIdsIterator(new GetFollowerIdsParameters("tweetinvi")
{
    PageSize = 50
});
var firstPage = await firstFriendIdsIterator.MoveToNextPage();
```

After having retrieved a first page, we can now request a new one from the previous cursor.

``` c#
var friendIdsIterator = client.Users.GetFollowerIdsIterator(new GetFollowerIdsParameters("tweetinvi")
{
    Cursor = firstFriendIdsIterator.NextCursor,
    // or Cursor = firstPage.NextCursor
});
var secondPage = await friendIdsIterator.MoveToNextPage();
```

## Flavour of iterators

As mentioned earlier Twitter exposes different support of pages. As a consequence parameters might differ between 2 types of iterator.

### Cursor iterators

> Cursor iterators accesses cursor endpoint properly supported by Twitter API

* `int PageSize` defines the maximum number of items that will be returned per request.
* `string Cursor` defines the cursor that will be used for executing the first request.

``` c#
var friendIdsIterator = client.Users.GetFollowerIdsIterator(new GetFollowerIdsParameters("tweetinvi")
{
    Cursor = "previous_cursor",
    PageSize = 50
});
```

### MinMax iterators

> MinMax iterators accesses older api endpoints that have less reliable support for cursors

* `int PageSize` defines the maximum number of items that will be returned per request.
* `long? SinceId` defines the minimum id that can be returned by Twitter
* `long? MaxId` defines the maximum id that can be returned by Twitter
* `ContinueMinMaxCursor` defines when the cursor is considered complete. By default it will be considered complete when 0 results are being returned.

``` c#
var timelineIterator = client.Timelines.GetHomeTimelineIterator(new GetHomeTimelineParameters()
{
    MaxId = 410984184018409,
    SinceId = 3109841840184091,
    PageSize = 50,
    ContinueMinMaxCursor = ContinueMinMaxCursor.UntilNoItemsReturned
});
```

### Multi-Levels iterators

> Twitter API misses some useful endpoints. Tweetinvi created multi level cursors that takes care of handling the complexity for you

Twitter do not provide any endpoint to retrieve a collection of friends. It only supports retrieving friend ids.\
The `Users.GetFollowersIterator` is an example of a multi level cursor and is used exactly as other cursors.

``` c#
var friendsIterator = client.Users.GetFollowersIterator(new GetFollowersParameters("tweetinvi"));
while (!friendsIterator.Completed)
{
    var page = await friendsIterator.MoveToNextPage(); 
}
```

Here is what happens behind the scenes when a user has more than 5000 friends:

* 1st call to `await friendsIterator.MoveToNextPage();`, Tweetinvi performs 2 requests and returns 100 users.
    * `GetFriendIds` that return 5000 friend ids
    * `GetUsers` that return 100 users
* Next **49x** calls to `await friendsIterator.MoveToNextPage();`; Tweetinvi 1 request to get the next users.
    * `GetUsers` that return 100 users
* On the **51st** call to `await friendsIterator.MoveToNextPage();`; Tweetinvi performs again 2 requests.
    * `GetFriendIds` that return 5000 friend ids
    * `GetUsers` that return 100 users