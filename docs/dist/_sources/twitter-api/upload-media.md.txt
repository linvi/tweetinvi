# Media Upload

Twitter allows developers to upload files on their servers so that they can be used in tweets. Twitter has a limited file format that they support:

* **Images** : PNG, JPEG, WEBP - max size = 5MB
* **GIFs** - max size = 15MB
* **Videos** : MP4 - max size = 15MB

If you want to learn more about the supported formats and video recommendations visit the [Twitter upload documentation](https://developer.twitter.com/en/docs/media/upload-media/overview).
Restrictions to the media also apply, check this [documentation for more information](https://developer.twitter.com/en/docs/media/upload-media/uploading-media/media-best-practices).

## Images

``` c#
var tweetinviLogoBinary = File.ReadAllBytes("./tweetinvi-logo-purple.png");

// for tweets
var uploadedImage = await client.Upload.UploadTweetImageAsync(tweetinviLogoBinary);

// for messages
var uploadedImage = await client.Upload.UploadMessageImageAsync(tweetinviLogoBinary);
```

## Video

``` c#
var videoBinary = File.ReadAllBytes("./video.mp4");

// for tweets
var uploadedVideo = await client.Upload.UploadTweetVideoAsync(videoBinary);

// for messages
var uploadedVideo = await client.Upload.UploadMessageVideoAsync(videoBinary);
```

<div class="warning">
Videos are different from images as they need to be processed by Twitter before becoming usable...
</div>

### Video Processing Status

You can request the status of the video processing and do this until you receive `Succeeded` or `Failed`.

``` c#
var status = await client.Upload.GetVideoProcessingStatusAsync(uploadedVideo);
// processing state can be : Pending, InProgress, Succeeded, Failed
var processingCompleted = status.ProcessingInfo.ProcessingState == ProcessingState.Succeeded;
```

### Automatic wait

If you only care about knowing when the video processing has completed you use `WaitForMediaProcessingToGetAllMetadata`.

``` c#
await client.Upload.WaitForMediaProcessingToGetAllMetadataAsync(uploadedVideo);
```

## Parameters

Tweetinvi includes various parameters to customize the experience and behaviour of upload.

``` c#
var uploadedVideo = await client.Upload.UploadBinaryAsync(new UploadTweetVideoParameters(binary)
{
    // Defines a timeout after which the operation gets cancelled
    Timeout = TimeSpan.FromMinutes(1),

    // Size of request sent to Twitter. Restrictions applies from twitter.
    MaxChunkSize = 500,

    // Event that informed the state of the upload
    UploadStateChanged = stateChange =>
    {
        _logger.WriteLine("Upload progress changed to " + stateChange.Percentage + "%");
    }
});
```

## Add metadata

Twitter let you [add metadata](https://developer.twitter.com/en/docs/media/upload-media/api-reference/post-media-metadata-create) to media after they have been uploaded.

``` c#
var media = await client.Upload.UploadTweetImageAsync(binary);
await client.Upload.AddMediaMetadataAsync(new MediaMetadata(media)
{
    AltText = "Hello",
});
```