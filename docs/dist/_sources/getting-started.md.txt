# Getting Started

In this section we will create a simple console application that will print your username.

## Pre requisites

Before starting you need to have created an app on https://developer.twitter.com/en/apps.
In the `Keys and tokens` section, create a pair of consumer and access keys.

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

## Hello Twitter World!

Now we just need to modify the `Main` function of `Program.cs`

First we create a client with your user's credentials (the ones from [apps.twitter.com](https://developer.twitter.com/en/apps)).

``` c#
static async Task Main(string[] args)
{
    var client = new TwitterClient("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
}
```

Now lets print your username!

``` c#
var user = await client.Users.GetAuthenticatedUser();
Console.WriteLine(user);
```

#### Congratulation and welcome to the world of Tweetinvi!

<details>
<summary>Program.cs</summary>

``` c#
using System;
using System.Threading.Tasks;

// You need to add the tweetinvi namespace
using Tweetinvi;

namespace tweetinvi_hello_world
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // we create a client with your user's credentials
            var client = new TwitterClient("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");

            // request the user's information from Twitter API
            var user = await client.Users.GetAuthenticatedUser();

            Console.WriteLine("Hello " + user);
        }
    }
}
```

</details>