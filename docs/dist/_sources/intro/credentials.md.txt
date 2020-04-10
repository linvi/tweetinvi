# Credentials in Twitter

Twitter offers 2 types of credentials:

* **Application Credentials** that let you perform operations from an application perspective. It consists of:
    * `Required` CONSUMER_KEY
    * `Required` CONSUMER_SECRET
    * `Optional` BEARER_TOKEN (learn more [here](https://developer.twitter.com/en/docs/basics/authentication/oauth-2-0))
* **User Credentials** that let you perform operations from a user perspective. It consists of:
    * `Required` Application Credentials (without bearer token)
    * `Required` ACCESS_TOKEN
    * `Required` ACCESS_TOKEN_SECRET

## Create your first set of credentials

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