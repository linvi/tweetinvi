# Account Activity - Stream/Events

> Now that users are subscribed to our webhook, Twitter will send events (through http requests) related to the subscribed users to our server.\
> These events will be raised by the [Account Activity Stream](./account-activity-events).\
> We will use this stream and watch for specific events to take actions.

## Account Activity Stream

To start listening to events coming from a user we will create an Account Activity Stream.\
This stream will raise events when the webhook receives a request from Twitter for this specific user.

``` c#
// the USER_ID specified must have been register for receiving the events
var accountActivityStream = accountActivityHandler.GetAccountActivityStream(USER_ID, "sandbox");

// we can now subscribe to various events from the stream
accountActivityStream.TweetCreated += (sender, tweetCreatedEvent) =>
{
    Console.WriteLine("A tweet was created by USER_ID or a tweet mentioning USER_ID");
};
```

## Events - Properties

Account Activity EventArgs contain information that explain what happened and why.

| Property       | Description                                          |
|----------------|------------------------------------------------------|
| Json           | The raw content of the http request                  |
| **InResultOf** | Informs of the reason that caused the event to occur |
| EventDate      | Date when the event was received                     |
| AccountUserId  | For which user this event is for                     |

<div class="note">

`InResultOf` varies depending on the type of event. Each of the possibilities are listed in the section below.
</div>



## Events

| Event         | Description                                                                        |
|---------------|------------------------------------------------------------------------------------|
| EventReceived | Twitter sent an event to the application, it is up for you to make sense out of it |

<div class="wy-table-responsive account-activity-events">
    <table class="docutils">
        <thead>
            <tr>
                <th>Event</th>
                <th>InResultOf</th>
                <th>Properties</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>
                    <b>TweetCreated</b><br/><br/>
                    A tweet related to the subscribed user has been created
                </td>
                <td>
                    <b>TweetCreatedRaisedInResultOf</b>
                    <ul>
                        <li>AccountUserCreatingATweet</li>
                        <li>AnotherUserReplyingToAccountUser</li>
                        <li>AnotherUserMentioningTheAccountUser</li>
                        <li>Unknown</li>
                    </ul>
                </td>
                <td>
                    <ul>
                        <li>Tweet</li>
                    </ul>
                </td>
            </tr>
            <tr>
                <td>
                    <b>TweetDeleted</b><br/><br/>
                    A tweet related to the subscribed user has been deleted
                </td>
                <td>
                    <b>TweetDeletedRaisedInResultOf</b>
                    <ul>
                        <li>AccountUserDeletingOneOfHisTweets</li>
                        <li>AnotherUserDeletedATweet</li>
                    </ul>
                </td>
                <td>
                    <ul>
                        <li>TweetId</li>
                        <li>UserId</li>
                    </ul>
                </td>
            </tr>
            <tr>
                <td>
                    <b>TweetFavorited</b><br/><br/>
                    A tweet related to the subscribed user was favorited
                </td>
                <td>
                    <b>TweetFavoritedRaisedInResultOf</b>
                    <ul>
                        <li>AccountUserFavoritingHisOwnTweet</li>
                        <li>AccountUserFavoritingATweetOfAnotherUser</li>
                        <li>AnotherUserFavoritingATweetOfTheAccountUser</li>
                        <li>Unknown</li>
                    </ul>
                </td>
                <td>
                    <ul>
                        <li>Tweet</li>
                        <li>FavoritedBy</li>
                    </ul>
                </td>
            </tr>
            <tr>
                <td colspan="3"></td>
            </tr>
            <tr>
                <td>
                    <b>UserFollowed</b><br/><br/>
                    The subscribed user followed or was followed by another user
                </td>
                <td>
                    <b>UserFollowedRaisedInResultOf</b>
                    <ul>
                        <li>AccountUserFollowingAnotherUser</li>
                        <li>AnotherUserFollowingAccountUser</li>
                        <li>Unknown</li>
                    </ul>
                </td>
                <td>
                    <ul>
                        <li>FollowedBy</li>
                        <li>FollowedUser</li>
                    </ul>
                </td>
            </tr>
            <tr>
                <td>
                    <b>UserUnfollowed</b><br/><br/>
                    The subscribed user stopped following another user
                </td>
                <td>
                    <b>UserUnfollowedRaisedInResultOf</b>
                    <ul>
                        <li>AccountUserUnfollowingAnotherUser</li>
                        <li>Unknown</li>
                    </ul>
                </td>
                <td>
                    <ul>
                        <li>UnfollowedBy</li>
                        <li>UnfollowedUser</li>
                    </ul>
                </td>
            </tr>
            <tr>
                <td>
                    <b>UserBlocked</b><br/><br/>
                    The subscribed user blocked another user
                </td>
                <td>
                    <b>UserBlockedRaisedInResultOf</b>
                    <ul>
                        <li>AccountUserBlockingAnotherUser</li>
                        <li>Unknown</li>
                    </ul>
                </td>
                <td>
                    <ul>
                        <li>BlockedBy</li>
                        <li>BlockedUser</li>
                    </ul>
                </td>
            </tr>
            <tr>
                <td>
                    <b>UserUnblocked</b><br/><br/>
                    The subscribed user unblocked another user
                </td>
                <td>
                    <b>UserUnblockedRaisedInResultOf</b>
                    <ul>
                        <li>AccountUserUnblockingAnotherUser</li>
                        <li>Unknown</li>
                    </ul>
                </td>
                <td>
                    <ul>
                        <li>UnblockedBy</li>
                        <li>UnblockedUser</li>
                    </ul>
                </td>
            </tr>
            <tr>
                <td>
                    <b>UserMuted</b><br/><br/>
                    The subscribed user muted another user
                </td>
                <td>
                    <b>UserMutedRaisedInResultOf</b>
                    <ul>
                        <li>AccountUserMutingAnotherUser</li>
                        <li>Unknown</li>
                    </ul>
                </td>
                <td>
                    <ul>
                        <li>MutedBy</li>
                        <li>MutedUser</li>
                    </ul>
                </td>
            </tr>
            <tr>
                <td>
                    <b>UserUnmuted</b><br/><br/>
                    The subscribed user muted another user
                </td>
                <td>
                    <b>UserUnmutedRaisedInResultOf</b>
                    <ul>
                        <li>AccountUserUnmutingAnotherUser</li>
                        <li>Unknown</li>
                    </ul>
                </td>
                <td>
                    <ul>
                        <li>UnmutedBy</li>
                        <li>UnmutedUser</li>
                    </ul>
                </td>
            </tr>
            <tr>
                <td colspan="3"></td>
            </tr>
            <tr>
                <td>
                    <b>MessageReceived</b><br/><br/>
                    The subscribed user received a message
                </td>
                <td>
                    <b>MessageReceivedInResultOf</b>
                    <ul>
                        <li>AccountUserReceivingAMessage</li>
                    </ul>
                </td>
                <td>
                    <ul>
                        <li>Message</li>
                        <li>Sender</li>
                        <li>Recipient</li>
                        <li>App</li>
                    </ul>
                </td>
            </tr>
            <tr>
                <td>
                    <b>MessageSent</b><br/><br/>
                    The subscribed user sent a message
                </td>
                <td>
                    <b>MessageSentInResultOf</b>
                    <ul>
                        <li>AccountUserSendingAMessage</li>
                        <li>Unknown</li>
                    </ul>
                </td>
                <td>
                    <ul>
                        <li>Message</li>
                        <li>Sender</li>
                        <li>Recipient</li>
                        <li>App</li>
                    </ul>
                </td>
            </tr>
            <tr>
                <td>
                    <b>UserIsTypingMessage</b><br/><br/>
                    The subscribed user is currently typing a message
                </td>
                <td>
                    <b>UserIsTypingMessageInResultOf</b>
                    <ul>
                        <li>AnotherUserTypingAMessageToAccountUser</li>
                        <li>Unknown</li>
                    </ul>
                </td>
                <td>
                    <ul>
                        <li>TypingTo</li>
                        <li>TypingUser</li>
                    </ul>
                </td>
            </tr>
            <tr>
                <td>
                    <b>UserReadMessageConversation</b><br/><br/>
                    A user read the latest message of a conversation with the subscribed user
                </td>
                <td>
                    <b>UserReadMessageConversationInResultOf</b>
                    <ul>
                        <li>AnotherUserReadingConversation</li>
                        <li>Unknown</li>
                    </ul>
                </td>
                <td>
                    <ul>
                        <li>UserWhoWroteTheMessage</li>
                        <li>UserWhoReadTheMessageConversation</li>
                        <li>LastReadEventId</li>
                    </ul>
                </td>
            </tr>
            <tr>
                <td colspan="3"></td>
            </tr>
            <tr>
                <td>
                    <b>UserRevokedAppPermissions</b><br/><br/>
                    The subscribed user revoked the application permissions.<br/>The user is now unsubsribed from the webhook
                </td>
                <td>
                    <b>UserRevokedAppPermissionsInResultOf</b>
                    <ul>
                        <li>AccountUserRemovingAppPermissions</li>
                        <li>Unknown</li>
                    </ul>
                </td>
                <td>
                    <ul>
                        <li>AppId</li>
                        <li>UserId</li>
                    </ul>
                </td>
            </tr>
        </tbody>
    </table>
</div>

**Special events**

| Event                                  | Description                                                                          |
|----------------------------------------|--------------------------------------------------------------------------------------|
| UnsupportedEventReceived               | The event is not yet supported. Please open a ticket                                 |
| EventKnownButNotFullySupportedReceived | The event is supported but Tweetinvi could not understand why the event was received |
| UnexpectedExceptionThrown              | Something went wrong while processing an event                                       |
