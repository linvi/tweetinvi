# Messages

Messages allow private interactions between users.

## Create, Read, Delete

``` c#
var message = await client.Messages.PublishMessage("hello", recipient);
var publishedMessage = await client.Messages.GetMessage(message.Id);
await client.Messages.DestroyMessage(publishedMessage);
```

## Publish with a Media

``` c#
var media = await client.Upload.UploadMessageImage(binary);

var message = await client.Messages.PublishMessage(new PublishMessageParameters("piloupe", recipient.Id)
{
    MediaId = media.Id
});
```

## Quick Reply Options

> Quick replies let you send messages with multiple choice options

``` c#
var message = await client.Messages.PublishMessage(new PublishMessageParameters("hello", recipient)
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
var messages = await client.Messages.GetMessages();
// or
var messagesIterator = client.Messages.GetMessagesIterator();
```

</div>