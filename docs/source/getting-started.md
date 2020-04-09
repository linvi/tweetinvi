# Getting Started

## Installation

``` sh
Install-Package TweetinviAPI
```

## Hello World!

First we need to add the tweetinvi namespace:

``` c#
using Tweetinvi;
using Tweetinvi.Models;
```

Then lets create our client:


``` c#
using Tweetinvi;
using Tweetinvi.Models;

var client = new TwitterClient("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
var user = await client.Users.GetAuthenticatedUser();

Console.WriteLine("You successfully authenticated with " + user);
```
