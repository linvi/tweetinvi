# Messages

Messages allow private interactions between users.

## Create, Read, Delete

``` c#
var message = await userClient.Messages.PublishMessageAsync("hello", recipient);
var publishedMessage = await userClient.Messages.GetMessageAsync(message.Id);
await userClient.Messages.DestroyMessageAsync(publishedMessage);
```

## Publish with a Media

``` c#
var media = await userClient.Upload.UploadMessageImageAsync(binary);

var message = await userClient.Messages.PublishMessageAsync(new PublishMessageParameters("piloupe", recipient.Id)
{
    MediaId = media.Id
});
```

## Quick Reply Options

> Quick replies let you send messages with multiple choice options

``` c#
var message = await userClient.Messages.PublishMessageAsync(new PublishMessageParameters("hello", recipient)
{
    QuickReplyOptions = new IQuickReplyOption[]
    {
        new QuickReplyOption
        {
            Label = "Superb"
        },
        new QuickReplyOption
        {
            Label = "Cool"
        },
        new QuickReplyOption
        {
            Label = "Hum"
        },
    }
});
```

## List Messages

<div class="iterator-available">

``` c#
var messages = await userClient.Messages.GetMessagesAsync();
// or
var messagesIterator = userClient.Messages.GetMessagesIterator();
```

</div>