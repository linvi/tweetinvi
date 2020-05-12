# Search

## User Search

<div class="iterator-available">

``` c#
var users = await client.Search.SearchUsersAsync("tweetinvi");

// or

var usersIterator = client.Search.GetSearchUsersIterator("tweetinvi");
```
</div>

<div class="warning" style="padding-bottom: 1px">

User search paging of Twitter API does not behave the same as other Twitter API endpoint (including tweet search).
* First search will run on page 1 (and not 0) - do not change this value to 0.
* Pages can return the same items multiple times (tweetinvi will filter these for you).
* Tweetinvi will need to perform 1 additional request to detect the completion of the iterator.

<details>
<summary>Why?</summary>

* Twitter user search API always return results regardless of the page you request.
* Twitter user search API always return a number of items equal to count requested.
* Twitter can return multiple time the same results in different pages.

</details>

</div>


## Tweet Search

<div class="iterator-available">

``` c#
var tweets = await client.Search.SearchTweetsAsync("hello");

//or

var tweetsIterator = client.Search.GetSearchTweetsIterator(new SearchTweetsParameters("hello"));
```
</div>

<div class="warning">

Tweet search paging of Twitter API does not inform of no more results available.\
As a result Tweetinvi will need to perform 1 additional request to detect the completion of the iterator.

</div>


## Saved Searches

Twitter API offers a way to interact with users' saved searches.

``` c#
var savedSearch = await client.Search.CreateSavedSearchAsync("hello");
var createdSavedSearch = await client.Search.GetSavedSearchAsync(savedSearch.Id);
var mySavedSearches = await client.Search.ListSavedSearchesAsync();
await client.Search.DestroySavedSearchAsync(savedSearch.Id);
```