# Parameters Validators

When passing a parameter to a request, Tweetinvi will check that the required parameters have been properly configured.\
If the required parameters are not properly configured, Tweetinvi will throw an `ArgumentException` before even trying to execute the request.

Twitter has various restrictions on its request parameters.

* Length
* Value
* Enums
* Limits
* ...

By default Tweetinvi will **not** prevent you from executing a query with invalid parameters. The reason being that such restrictions might change in the future and we do not want to release a new version of Tweetinvi every time Twitter changes their limits.

To help you discover and understand these restrictions we have been through the documentation. The `ParametersValidators` verifies that both the required and optional parameters are valid. If any of them is not valid, an `ArgumentException` will be raised.

``` c#
 try
{
    userClient.ParametersValidator.Validate(new GetBlockedUsersParameters()
    {
        PageSize = 10000 // Maximum is 5000
    });
}
catch (TwitterArgumentLimitException e) // this extends ArgumentException
{
    // You will receive a message looking like: "Argument PageSize was over the limit of 5000 page size"
    Console.WriteLine(e.Message);
}
```

## Twitter Limits

`TwitterArgumentLimitException` is an `ArgumentException` that was caused because a limit was not respected.\
Because such limits are subject to change Tweetinvi offer you a way to modify these limits so that you do not receive an `Exception` when the api changes.

`TwitterArgumentLimitException` contains 2 interesting properties:

* `Note` explains how to modify the limit value (e.g. "Limits can be changed in the TwitterClient.ExecutionContext.Limits.ACCOUNT_GET_BLOCKED_USER_MAX_PAGE_SIZE").
* `LimitType` gives you the type of limit that you have breached (e.g. "ACCOUNT_GET_BLOCKED_USER_MAX_PAGE_SIZE")

Here is how to modify a limit.

``` c#
userClient.Config.Limits.ACCOUNT_GET_BLOCKED_USER_MAX_PAGE_SIZE = 10000;

// now no exception will be raised by the validator
userClient.ParametersValidator.Validate(new GetBlockedUsersParameters
{
    PageSize = 10000
});
```