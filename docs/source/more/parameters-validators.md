# Parameters Validators

When passing a parameter to a request, Tweetinvi will check that the **required** parameters have been properly configured.\
If the **required** parameters are not properly configured, Tweetinvi will throw an `ArgumentException` before even trying to execute the request.

Though optional parameters are not validated and can have different type of restrictions including:

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
catch (ArgumentException argumentException)
{
}
```

The `TwitterArgumentLimitException` are being thrown when a limit specified in the `client.Config.Limits` is not respected.\
This type of error will provide additional information as to why the limit was breached.

If an exception is raised for invalid reasons, please report it on github.\
You can also change limits as you wish as explained in [Twitter Limits](../twitter-client/twitter-client#twitter-limits).