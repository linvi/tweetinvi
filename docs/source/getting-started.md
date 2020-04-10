# Getting Started

In this section we will create a simple console application that will print your username.

## Create a new project

``` sh
# create a new directory for the hello world application
mkdir tweetinvi-hello-world && cd tweetinvi-hello-world

# initialize a console project
dotnet new console
```

## Install Tweetinvi

Tweetinvi is available github and on [nuget](https://www.nuget.org/packages/TweetinviAPI/):

``` sh
dotnet add package TweetinviAPI
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
