## Rate Limits

EndToEnd tests are subject to Twitter rate limits.
All the tests should pass without any issue once.

Attempting to run them a second time before 15 minutes have passed since the end of the previous tests can result in unexpected results.

## Auth Tests

Auth Tests uses Gecko driver to actually type email, username and password.
For security such information are not stored in the repository.

To be able to run Auth tests you will therefore need to add the following environment variables.

* `TWEETINVI_EMAIL` -> the account that you want to authenticate with 
* `TWEETINVI_PASS` -> the password fo the account that you want to authenticated with.

## Test orders

Some tests have specific orders. This 