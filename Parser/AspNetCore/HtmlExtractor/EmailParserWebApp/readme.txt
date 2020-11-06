EMailParserWebApp
-------------------------------------------------------------------------------
This is a web app sample to demonstrate the capabilities of the C1TextParser library - Html extractor.

System prerequisites:
- .Net Core 2.1 : https://dotnet.microsoft.com/download
- Web Developer Tools for Visual Studio

Deployment:

As default, the WebApp was configured to run on primary domain. So if this app deployed on subdomain, we need to configuration to make it run properly. 

for example, if WebApp was deployed to https://demos.componentone.com/TextParsers/EmailParserWebApp

1.In wwwroot/index.html, change base location to subdomain.

 <base href="/TextParsers/EmailParserWebApp/"/>

2.In wwwroot/assets/config.json, change host as bellow.

 "host" : "TextParsers/EmailParserWebApp/",

Note: "host" is the API Url. Normally, it will be https://demos.componentone.com/TextParsers/EmailParserWebApp/. But in some case, we will face Cors issue if we use absolutely API URL. That's why we use relative path instead of absolute path.


