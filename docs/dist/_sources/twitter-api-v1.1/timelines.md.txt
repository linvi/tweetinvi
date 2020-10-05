# Timelines

Timelines represent a collection of tweets.

## Home Timeline

The Home timeline contains the tweets available on the authenticated user's home page.

<div class="iterator-available">

``` c#
// Get the tweets available on the user's home page
var homeTimelineTweets = await userClient.Timelines.GetHomeTimelineAsync();
```
</div>

## User Timeline

A User timeline contains the tweets shown on a user's page.

<div class="iterator-available">

``` c#
// Get tweets from a specific user
var userTimelineTweets = await userClient.Timelines.GetUserTimelineAsync("tweetinviapi");
```
</div>

## Mentions Timeline

The Mentions timeline contains tweets where the authenticated user has recently been mentioned (tweet containing the `@username`).

<div class="iterator-available">

``` c#
var mentionsTimeline = await userClient.Timelines.GetMentionsTimelineAsync();
```
</div>

## Retweets Of Me Timeline

The RetweetsOfMe timeline contains the most recent Tweets authored by the authenticated user that have been retweeted by others.

<div class="iterator-available">

``` c#
var retweetsOfMeTimeline = await userClient.Timelines.GetRetweetsOfMeTimelineAsync();
```
</div>