# Tweetinvi - Xamarin for iOS

This is a very simple iOS application that uses Tweetinvi to access the `AuthenticatedUser` and his `Timeline`.

To start working on this project please open the file `ViewController.cs` and simply enter your credentials at the line:

`Auth.SetUserCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");`

# How to use this project?

You can run this project with **Visual Studio on Windows** or **Visual Studio for Mac**.

This project has been tested with the iPhone emulator available on Mac.
To have access to it, please install XCode and enable the developer mode on your machine.

### Visual Studio (Windows)

- Ensure that the latest version of Xamarin is installed on both your Windows machine and Mac.
- In your Mac `System Preferences` authorize the following : `Sharing > Remote Login > Select All users or Specific User`
- In Visual Studio select `Simulator` instead of `Device`
- Run
- The emulator should start on your Mac.

### Visual Studio (Mac)

- Simply run the project on `Debug|iPhoneSimulator > iPhone x  iOS XX.X.X`.
- The emulator should start.