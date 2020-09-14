# Exceptions

Tweetinvi 5.0 changes the way to manage exceptions. The old `TweetinviConfig.SwallowWebExceptions` no longer exists and devevelopers will now be expected to handle exceptions.

Tweetinvi does the work of understanding why Twitter failed for you and present the errors in a unified and simple to use format.

You will find below a list of exceptions that can happen in Tweetinvi.

## Common exceptions

The 2 types of exception below are recommended to be catched.

* `TwitterException` - It means that Twitter sent a failure response. Tweetinvi will share all the data sent by Twitter. In **ALMOST all the cases** Tweetinvi will not act based on errors and you as developers will need to take care of Exceptions. ALMOST all the cases as some operations from Twitter actually return 400+ status code to inform that an operation succeeded but the response is "No", in such case Twitter will analyse the error, if it is an excepted failure tweetinvi will handle if for you and will send you the results.

* `TwitterAuthException` - This happen when something goes wrong during the [authentication process](../authentication/authentication). This type of exception only occurs for methods located under `client.Auth`.

## Properties

* `.ToString()` displays a developer friendly error message as followed:

> --- Date : 09/14/2020 01:01:50\
> URL : https://api.twitter.com/1.1/account/verify_credentials.json?include_email=true\
> Code : 401\
> Error documentation description : Unauthorized -  Authentication credentials were missing or incorrect.\
> Error message : https://api.twitter.com/1.1/account/verify_credentials.json?include_email=true request failed.\
> Could not authenticate you. (32) 

* `StatusCode` of the http request that was sent to Twitter
* `URL` sent to Twitter
* `TwitterDescription` is a single line explanation of what has gone wrong in the request
* `TwitterExceptionInfos` is a collection of errors that happened when processing the request
* `Request` object containing all the information associated with the execution of the http request

## Other exceptions

* `ArgumentException` - This exception is raised when executing an action with required parameters that are invalid. Tweetinvi won't even try to run the request because it knows that it will fail. Apart if you pass null for input values you should never have such an exception and can freely not catch it. - Such exceptions could be raised in the form of TwitterArgumentLimitException because of attempting to provide inputs that are too high (e.g. count parameter of 500 while the max value is 200); Tweetinvi parameters are always configured by default with the maximum value for all parameters so you should not need to change this.
    
* `TwitterInvalidCredentialsException` - This exception is raised if some of the properties in the client Credentials are null while they are required.

* `TwitterTimeoutException` - It means that the operation was too long. This can happen on very slow networks or when Twitter actually have backend issues. This is pretty rare and you might not be interested to catch this specifc exception. Note that it is actually inherits from `TwitterException`.

## How to handle an exception?

``` c#
// some code to illustrate how to catch Tweetinvi exceptions.
try
{
    var user = await userClient.Users.GetAuthenticatedUserAsync();
}
catch (TwitterException e)
{
    Console.WriteLine(e.ToString());
}
```