Manual Testing can be done as followed:

For both `Examplinvi.AccountActivity.ASP.NETCore` and `Examplinvi.AccountActivity.ASP.NETCore.3.0` do the following

* `SetUserCredentials` -> configures the credentials that will be used to subscribe the user to Account Activity
* `DeleteWebhook` -> cleanup
* `RegisterWebhook` -> set webhook with ngrok url
* `SubscribeToAccountActivity` -> make webhook receive events for the registered user
* `SubscribeToEvents` -> events will now output in console

On twitter.com

* Wait for 10 second after `SubscribeToAccountActivity` as this operation is not instantaneous on Twitter
* `Create a new Tweet`

Verify

* Check the console printed out the creation of the tweet