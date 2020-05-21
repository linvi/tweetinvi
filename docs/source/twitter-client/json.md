# Json - Serialize/Deserialize

The process of serialization transforms an object into a string.\
The process of deserialization transforms a string into an object.

## Json

`TwitterClient.Json` contains 2 methods that let you manipulate Tweetinvi objects as json.\
The serializer supports all tweetinvi objects and arrays of such objects.

To serialize any Tweetinvi object you can use `client.Json.Serialize`.

``` c#
ITweet tweet = await client.Tweets.GetTweetAsync(42);
string json = client.Json.Serialize(tweet);
```

You can do the reverse operation with `client.Json.Deserialize`.

``` c#
ITweet deserializedTweet = client.Json.Deserialize<ITweet>(json);
```

<div class="note">

Remember the concept of [smart objects](./intro/basic-concepts#smart-objects).\
It also applies to deserialized objects.

It means that all actions performed from that object will be executed through the client which created it.

</div>


## Factories

Factories are similar to `Json` but provide user friendly methods to create Tweetinvi objects.\
They usually can take as a parameter either a json string or a Data Transfer Object (DTO).

``` c#
// from serialized json
ITweet tweet = client.Factories.CreateTweet(json);

// from a dto
ITweetDTO dto = tweet.TweetDTO;
ITweet recreatedTweet = client.Factories.CreateTweet(dto);
```