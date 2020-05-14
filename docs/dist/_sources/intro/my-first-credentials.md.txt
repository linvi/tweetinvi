# Create my first credentials

<div class="warning">
If you already have credentials skip this section
</div>

1. Create a new application on https://developer.twitter.com/en/apps/create
2. Select the `Keys and Tokens` tab
3. Click `Generate` next to the **Access token & access token secret**
4. Now you can find your application credentials as well as the additional credentials for authenticating as a user.

<div style="max-width:700px;">

![](./credentials-twitter-page.png)

</div>

## Test your credentials

To test these new credentials run the following code.

``` c#
var client = new TwitterClient("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
var me = await client.Users.GetAuthenticatedUserAsync();

// me should contain the information of your twitter app account
```