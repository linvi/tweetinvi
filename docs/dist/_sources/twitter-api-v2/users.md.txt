# Users V2

## Get users

``` c#
// By id
var userResponse = await client.UsersV2.GetUserByIdAsync(1577389800);
var user = userResponse.User;

// By username
var userResponse = await client.UsersV2.GetUserByNameAsync("tweetinviapi");
var user = userResponse.User;
```

## Get multiple users

``` c#
// By ids
var usersResponse = await client.UsersV2.GetUsersByIdAsync(1577389800, 1693649419);
var users = usersResponse.Users;

// By usernames
var usersResponse = await client.UsersV2.GetUsersByNameAsync("tweetinviapi", "tweetinvitest");
var users = usersResponse.Users;
```