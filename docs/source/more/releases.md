# Releases

## 5.0

Tweetinvi 5.0 is a major refactor of Tweetinvi. The new version introduces the `TwitterClient` which lets you perform all operations based on a fixed set of credentials. The change let us resolved various core problematics that arised with the evolution of the .NET Framework including async/await and ASP.NETCore execution contexts.

Here are some of the major changes.

* Async/Await

> Tweetinvi is now using the Async/Await pattern in its entire code base. This will help modern development and ease up ASPNET thread pool.

* [TwitterClient](https://linvi.github.io/tweetinvi/dist/intro/basic-concepts.html#twitterclient)

> The TwitterClient gives you better control over who is executing a request. Also it gives developers the ability to pass the client through the code.

* Twitter API V2

> Support of all the endpoints of Twitter API V2!

* [Parameters](https://linvi.github.io/tweetinvi/dist/intro/basic-concepts.html#parameters)

> Developers no longer have to search for endpoints available parameters. Tweetinvi contains all the parameters from the official documentation and all the parameters missing from the official documentation :D

* [Iterators](https://linvi.github.io/tweetinvi/dist/twitter-api/iterators.html)

> Iterators are a new way to use paging in Twitter in a single and unique way. Developers no longer need to understand the 4 types of paging from Twitter API, Tweetinvi takes care of it for you!

* [Brand new documentation](https://linvi.github.io/tweetinvi/dist/index.html)

> Tweetinvi documentation has been at the core of the new version.
> * The new documentation should explain provide examples for all the bits and pieces you can find in the Tweetinvi library.
> * All publicly available code has been annotated to explain what each method do and what they return.

* [Redesign of Account Activity](https://linvi.github.io/tweetinvi/dist/account-activity/account-activity.html)

> A lot of work has been done to simplify the Account Activity process and make it as easy as possible for developers to start using it.

* .NET Compatibility

> Tweetinvi 5.0 has been built for .NETStandard 1.4 and .NETStandard 2.0 which should make it compatible for most of the existing .NET environments!

* Support for missing endpoints

> Some endpoints were missing from the library. All endpoints in the Twitter documentation are now officially supported.

* [Redesign of Error Management](https://linvi.github.io/tweetinvi/dist/more/exceptions.html)

> Exceptions have never been a strong part of Tweetinvi. This has changed and Tweetinvi is now throwing a very limited set of Exceptions.

* Better support for special cases

> Twitter API endpoints are managing response formats, status codes, headers, response, error messages in different ways. Tweetinvi now takes care of all the special cases for you and handle these cases as you would expect hiding the complexity behind a simple API.

* Bug Fixes

> 5.0 was also a chance to fix a long list of bugs, including multithreading, missing data, unhandled cases, non matching criteria and many more...

* Integration Tests

> Tweetinvi 5.0 uses integration tests to verify that the code base is properly working. Not only does this give us a better view on the state of the library but also let you discover how to use the library!

* And much more

> Performance improvements, upload support, parameters validation, namespaces/code cleanup, dependencies updates, .NETCore support...

Tweetinvi 5.0 is the result of more than a year of work, countless hours and more than 350 commits so I hope you will appreciate using the new version. 

I take this opportunity to thanks @JoshKeegan for his special contributions to the project.

I look forward to hearing your feedback!