## Ngrok with Https

Use `-host-header=rewrite` so that IIS do not complain when redirecting requests arrive to it.

`ngrok http -host-header=rewrite https://localhost:44300`

## Port

Port has to be between 44300 and 44398 for iis to simulate https certificates.