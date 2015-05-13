# Imgur.Api
Yet another implementation of Imgur API v3 for .NET.

It's portable class library that targets:

- .NET Framework 4.5
- Windows 8
- Windows Phone 8.1
- Windows Phone Silverlight 8
- Xamarin.Android
- Xamarin.iOS
- Xamarin.iOS (Classic)

Original code was used in Grin app (Imgur client for Windows Phone) and used RestSharp for making REST requests. This version of the library no longer uses RestSharp as it's not portable, but relies on HttpClient and Json.NET.
