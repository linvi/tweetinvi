# Users V2

## Get users

Retrieving users from the Twitter API v2 support both application and user credentials.\
Note that the value of some properties might differ between the 2 types of authentication.

``` c#
// Get single user by id
var userResponse = await client.UsersV2.GetUserByIdAsync(1577389800);
var user = userResponse.User;

// Get single user by username
var userResponse = await client.UsersV2.GetUserByNameAsync("tweetinviapi");
var user = userResponse.User;

// Get multiple users by id
var usersResponse = await client.UsersV2.GetUsersByIdAsync(1577389800, 1693649419);
var users = usersResponse.Users;

// Get multiple users by username
var usersResponse = await client.UsersV2.GetUsersByNameAsync("tweetinviapi", "tweetinvitest");
var users = usersResponse.Users;
```

[Expansions and custom fields](./basics) can be found in the class `UserResponseFields`.