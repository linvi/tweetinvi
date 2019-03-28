## Tips

* If you can reach localhost but not 127.0.0.1 you need to modify your applicationhost.config bindings.
This is important specially when you want to use ngrok.

``` xml
<!-- ONLY LOCALHOST replying -->
<binding protocol="http" bindingInformation=":8080:localhost" />

<!-- ONLY 127.0.0.1 replying -->
<binding protocol="http" bindingInformation=":8080:127.0.0.1" />
```