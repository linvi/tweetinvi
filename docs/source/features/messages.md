# Messages

Messages allow private interactions between users.

## Create, Read, Delete

``` c#
var message = await client.Messages.PublishMessageAsync("hello", recipient);
var publishedMessage = await client.Messages.GetMessageAsync(message.Id);
await client.Messages.DestroyMessageAsync(publishedMessage);
```

## Publish with a Media

``` c#
var media = await client.Upload.UploadMessageImageAsync(binary);

var message = await client.Messages.PublishMessageAsync(new PublishMessageParameters("piloupe", recipient.Id)
{
    MediaId = media.Id
});
```

## Quick Reply Options

> Quick replies let you send messages with multiple choice options

``` c#
var message = await client.Messages.PublishMessageAsync(new PublishMessageParameters("hello", recipient)
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
var messages = await client.Messages.GetMessagesAsync();
// or
var messagesIterator = client.Messages.GetMessagesIterator();
```

</div>