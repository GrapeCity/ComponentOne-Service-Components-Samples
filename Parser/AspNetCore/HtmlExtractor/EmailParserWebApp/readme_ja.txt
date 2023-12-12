EMailParserWebApp
-------------------------------------------------------------------------------
これは、C1TextParser ライブラリ（Html エクストラクタ）の機能を示す Web アプリのサンプルです。

システムの前提条件：
- .Net Core 2.1：https://dotnet.microsoft.com/download
- Web Developer Tools for Visual Studio

展開：

デフォルトでは、WebApp はプライマリドメインで実行するように構成されていました。したがって、このアプリがサブドメインにデプロイされている場合は、正しく実行されるように構成する必要があります。

たとえば、WebApp が https://demos.componentone.com/TextParsers/EmailParserWebApp にデプロイされた場合

1. wwwroot/index.html で、ベースの場所をサブドメインに変更します。

  <base href="/TextParsers/EmailParserWebApp/"/>

2. wwwroot/assets/config.json で、ホストを次のように変更します。

  "host" : "TextParsers/EmailParserWebApp/",

注："host" は API Url です。 通常は、https://demos.componentone.com/TextParsers/EmailParserWebApp/ になります。ただし、絶対 API URL を使用すると、Cors の問題が発生する場合があります。そのため、絶対パスではなく相対パスを使用します。 
