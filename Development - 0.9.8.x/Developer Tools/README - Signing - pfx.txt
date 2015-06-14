Open Visual Studio Command Prompt

// Generate the public Key file
sn -p tweetinvi.pfx tweetinvi.key

// Get the Hexa version of the public key
// When performing this action the password should be requested
sn -tp tweetinvi.key 