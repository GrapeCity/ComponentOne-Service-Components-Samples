using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ConnectorsSample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            C1.DataConnector.LicenseManager.Key = License.Key;
        }
    }

    public static class License
    {
        public const string Key =
            "ACgBKAIoB4FDAG8AbgBuAGUAYwB0AG8AcgBzAFMAYQBtAHAAbABlABe7xPJHCuZu" +
            "HvAAmMi9ed3ENA9DpmVe21ByYjlkA0kh0BugwddD9FkAykzDz17wHQYR0xCBCykE" +
            "RYSzPEQpkfCk4e/e5g6hMcGc1VTRB7XMwnziuHEQvnxRqIZBHIsVkCztW2S71AVg" +
            "8j2wcOZdDUZph/tlHG/T93INPsXUzG9NZ7wVqJ4kmSd3h3F0M1FFKbZYKpid01IW" +
            "A+yzLI9bigX18bA1fgxZXF7OlJtMhd/CbcCr+bFN8+f11FLuguYVaHrSVkZERD0x" +
            "tYP/hgwZacx8EwBZTAbK0fJemhpaQ33qNvu7CblDyg1JWdaEfHGdLfVKLmtKso8X" +
            "tAIW41UgVoVyHvf8qKhuxS18jSUtBUsk2LqPXdhi5vXf6WXjFxs7nV/Y9noOvVKt" +
            "D5gvXgd91Vomj7C6RlNKj+sta/upF+tLkeTIFW5rE6+y20R8mXsp/fpmcFHZvakh" +
            "4fzJhd4hKIkP/90lOf+DXYE1hf9nxospMf48d0dkHVw3jf/Ixxej1CWAwBa4zSe3" +
            "DTrd7/qK5weZBMXJGkQtDXuxFtQK+M3ylfsCqtj9gylaey0qUPkJUVcGGCGJEny0" +
            "VrTS4r20w5XhR6g/BwnGCrCRr4CX8fFJb1ThqidkcSCb3slhRP6shjiAFacm1NTF" +
            "9RFlKoK+V6ZnWlwCPFDFs11FtWIh3kKQMIIFVTCCBD2gAwIBAgIQQQN40iY2WXoW" +
            "2ybGvRCUizANBgkqhkiG9w0BAQUFADCBtDELMAkGA1UEBhMCVVMxFzAVBgNVBAoT" +
            "DlZlcmlTaWduLCBJbmMuMR8wHQYDVQQLExZWZXJpU2lnbiBUcnVzdCBOZXR3b3Jr" +
            "MTswOQYDVQQLEzJUZXJtcyBvZiB1c2UgYXQgaHR0cHM6Ly93d3cudmVyaXNpZ24u" +
            "Y29tL3JwYSAoYykxMDEuMCwGA1UEAxMlVmVyaVNpZ24gQ2xhc3MgMyBDb2RlIFNp" +
            "Z25pbmcgMjAxMCBDQTAeFw0xNDEyMTEwMDAwMDBaFw0xNTEyMjIyMzU5NTlaMIGG" +
            "MQswCQYDVQQGEwJKUDEPMA0GA1UECBMGTWl5YWdpMRgwFgYDVQQHEw9TZW5kYWkg" +
            "SXp1bWkta3UxFzAVBgNVBAoUDkdyYXBlQ2l0eSBpbmMuMRowGAYDVQQLFBFUb29s" +
            "cyBEZXZlbG9wbWVudDEXMBUGA1UEAxQOR3JhcGVDaXR5IGluYy4wggEiMA0GCSqG" +
            "SIb3DQEBAQUAA4IBDwAwggEKAoIBAQDBAvbKZVuylaQKkQelVQe1yZvPmutMe/B2" +
            "VjZrwI7P3q5qe6W4fDPR2LhFU0Y/B+G2l8RWKuIu+XuZDa+7PpxmyeVHzOgpXam3" +
            "kbEOM70W+p6X67XDgcH2DsdMKHmEHyOlcy1c4T2xo1AyuqnR2S3/xHL0iCr0W7ty" +
            "CzhN5LrsdO4EJG/vq60gVMiSl1PJ1vHPivvbH1qh2D2/BRdiGs1sYZnyHSKAzSso" +
            "696/8CJ940Ho6an2pohzbFPztuiktBHLwkuQhTig0+r73ZwJHpN5Mi1nn/nGv3Gx" +
            "aO+L2sGBrZsNsM8P4XMJQDSEGggMc/uSR0Gd0hIPCy0mfgvBOE/vAgMBAAGjggGN" +
            "MIIBiTAJBgNVHRMEAjAAMA4GA1UdDwEB/wQEAwIHgDArBgNVHR8EJDAiMCCgHqAc" +
            "hhpodHRwOi8vc2Yuc3ltY2IuY29tL3NmLmNybDBmBgNVHSAEXzBdMFsGC2CGSAGG" +
            "+EUBBxcDMEwwIwYIKwYBBQUHAgEWF2h0dHBzOi8vZC5zeW1jYi5jb20vY3BzMCUG" +
            "CCsGAQUFBwICMBkMF2h0dHBzOi8vZC5zeW1jYi5jb20vcnBhMBMGA1UdJQQMMAoG" +
            "CCsGAQUFBwMDMFcGCCsGAQUFBwEBBEswSTAfBggrBgEFBQcwAYYTaHR0cDovL3Nm" +
            "LnN5bWNkLmNvbTAmBggrBgEFBQcwAoYaaHR0cDovL3NmLnN5bWNiLmNvbS9zZi5j" +
            "cnQwHwYDVR0jBBgwFoAUz5mp6nsm9EvJjo/X8AUm7+PSp50wHQYDVR0OBBYEFABa" +
            "8K2l1Hg19YQSqCwFDvhWG46OMBEGCWCGSAGG+EIBAQQEAwIEEDAWBgorBgEEAYI3" +
            "AgEbBAgwBgEBAAEB/zANBgkqhkiG9w0BAQUFAAOCAQEAiMKYWjeOW+VYirEXwgOo" +
            "W1XqjITQiZi+uJqsXWL8N4LBeKDgg6IjOpFoctTaFHdi6XK/T43xieeWV+LGZaqM" +
            "Xn5U6R4J1/DDybiqQwbJO1pIYhLuuXwcG/oPcEBzDH4GNIIxyAENmRH9jyek00hX" +
            "L49uMIe93bMqnJo9vdH6cA7R2hdoxOaav7UATifLw5DeOsLeKjIRupyKToHPSp4M" +
            "iy3RDu1d+CjNTW/rDfSZKk1lzaDapTn+0I2B8JcOyru1t5CBivn9ZD9cakwaV8LA" +
            "RObC5Z7oz/iQKlfGipQSQykSNyIZaxvQiVJqhTYZmdn+UBOYwLz0Hl3ry5zGIqia" +
            "4DCCApowggGCoAMCAQICBA7u7u4wDQYJKoZIhvcNAQEFBQAwDzENMAsGA1UEAwwE" +
            "R0MtMTAeFw0yMDA0MDkxNDEzMzlaFw0yNTA0MDkxNDEzMzlaMA8xDTALBgNVBAMM" +
            "BEdDLTEwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQCQqqyKm92fgDH/" +
            "fODN+T6YlfN0xYdsXDs8STrHPN0KSwYejGyHp4gm476PnTXsxxCktcwqS7akaj69" +
            "OfMxWzL9J5nFA56YxfeJRt8+G99v+PcWPrFVvMP/BUXQeMqUuF2OOPykLupODeF/" +
            "gQVBi93PU2Lw1bb6GZiH3E+6xXciRmNSLBXG7yPagUWoJOmXhRlQNKYm66MZxL1c" +
            "P6IBtLUHTI0ZTFc67dIK4Eo3Kz9Rs7hCBlbmkUh5rsDX76Ligy6V7PPu4Wtggc/4" +
            "BTlPImnegoJDG02tgSphIXNbRTN66p6WimFRctl3JHFndDzGmmVM5Lq5aWIbpLVX" +
            "yVkeclIdAgMBAAEwDQYJKoZIhvcNAQEFBQADggEBAH1TBF6eF6VDAj3d4tYdfZjt" +
            "7Hvp/KSvOOZ0UEpzFzFcoesAz08MsDu6yyinYEa6KzKZh2qTNAQDV/3bdvrI+wE3" +
            "DN/c76CuZRnLQT+MC0/QoxjHLyubJkvXSgUKDq9abMY5KojQ5e7HtSXqqth/YfB5" +
            "e5+eBIzyeILdQFt2e8pbPNLfCecj44kgI3KvUq9n4kbxkBbQ4y8ClXyoR9Zr52mv" +
            "KyxuELBZDZhTYu2dYKdwhbKPWYBY9BQHK6eFouYGsSaOAJy5FNI3CIKVJzFbTthp" +
            "pggiD/9EMFS967P2bcoOmszEcDqA8L3IdBiTnm3qac9ccdd2LIKgI2ISOH9PPyo=";
    }

}
