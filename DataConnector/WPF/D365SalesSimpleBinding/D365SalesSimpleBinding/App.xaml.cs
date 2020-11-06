using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace D365SalesSimpleBinding
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
            "ADQBNAI0B41EADMANgA1AFMAYQBsAGUAcwBTAGkAbQBwAGwAZQBCAGkAbgBkAGkA" +
            "bgBnADBqDIpjEYMtaym+UkOcq6htKBoxI5QgoQdcRICsZjiqRS/FBgW4t627cSzQ" +
            "wBwgEP1Mn2VSjxWUEg5Oz8MTIPQ5G0cqwl5UGTOhXjjsNMqd7fwgCjjTDQPloNA5" +
            "+2Zdyj8PWxLnSlWQABDpsg9mndLlkk4Mthll+Gn0tfFB+lPGRYlWlP7wNyHHhAcD" +
            "Z7MNwoRRX9RLwnd7eGdp9WTnC2f5TU7OW096VuOPHUNVr8lU9KZ59Rr+FUTBuoML" +
            "iuoOqDXJke060bEq5v0ncg1r/uApqaXKYCueo2wJ1l0h1WOzvVmn1+UwgxA5AaFj" +
            "AAKZmHLZj5/nY3gk2xHQ9y6Eo5IXuVdAZibYTLGqCO1ETYMt96p10mFxlieLZvB5" +
            "j5XXvsciK4/CKRxhhZI3uTKB3MumqD3qpkX0h5xL7oVz4rZT3cg9fSm3ujTWNUyc" +
            "ObL+Df9Ugp55Ms0Tzeim0Xd29UJYqOqFKgfWdX9gfoNMfKWFcdhT14fluaCpAEFs" +
            "9e9MV4F2ktS7H9VzCuHTUse1dHFvD685LbAxPr75v2t20GU5N8WD5RBKAjrtLRqg" +
            "fDD7Z1GIjYBbha2PRcqd0u0r9G2kps6oyQ7vtXkQAuvfx8wNXSRKx9F7zbsUdPBY" +
            "pCZHyNm/7dpjUCEJbS6mFN39A7hi7oTUpnzUZADsI/6t2XORMIIFVTCCBD2gAwIB" +
            "AgIQQQN40iY2WXoW2ybGvRCUizANBgkqhkiG9w0BAQUFADCBtDELMAkGA1UEBhMC" +
            "VVMxFzAVBgNVBAoTDlZlcmlTaWduLCBJbmMuMR8wHQYDVQQLExZWZXJpU2lnbiBU" +
            "cnVzdCBOZXR3b3JrMTswOQYDVQQLEzJUZXJtcyBvZiB1c2UgYXQgaHR0cHM6Ly93" +
            "d3cudmVyaXNpZ24uY29tL3JwYSAoYykxMDEuMCwGA1UEAxMlVmVyaVNpZ24gQ2xh" +
            "c3MgMyBDb2RlIFNpZ25pbmcgMjAxMCBDQTAeFw0xNDEyMTEwMDAwMDBaFw0xNTEy" +
            "MjIyMzU5NTlaMIGGMQswCQYDVQQGEwJKUDEPMA0GA1UECBMGTWl5YWdpMRgwFgYD" +
            "VQQHEw9TZW5kYWkgSXp1bWkta3UxFzAVBgNVBAoUDkdyYXBlQ2l0eSBpbmMuMRow" +
            "GAYDVQQLFBFUb29scyBEZXZlbG9wbWVudDEXMBUGA1UEAxQOR3JhcGVDaXR5IGlu" +
            "Yy4wggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDBAvbKZVuylaQKkQel" +
            "VQe1yZvPmutMe/B2VjZrwI7P3q5qe6W4fDPR2LhFU0Y/B+G2l8RWKuIu+XuZDa+7" +
            "PpxmyeVHzOgpXam3kbEOM70W+p6X67XDgcH2DsdMKHmEHyOlcy1c4T2xo1AyuqnR" +
            "2S3/xHL0iCr0W7tyCzhN5LrsdO4EJG/vq60gVMiSl1PJ1vHPivvbH1qh2D2/BRdi" +
            "Gs1sYZnyHSKAzSso696/8CJ940Ho6an2pohzbFPztuiktBHLwkuQhTig0+r73ZwJ" +
            "HpN5Mi1nn/nGv3GxaO+L2sGBrZsNsM8P4XMJQDSEGggMc/uSR0Gd0hIPCy0mfgvB" +
            "OE/vAgMBAAGjggGNMIIBiTAJBgNVHRMEAjAAMA4GA1UdDwEB/wQEAwIHgDArBgNV" +
            "HR8EJDAiMCCgHqAchhpodHRwOi8vc2Yuc3ltY2IuY29tL3NmLmNybDBmBgNVHSAE" +
            "XzBdMFsGC2CGSAGG+EUBBxcDMEwwIwYIKwYBBQUHAgEWF2h0dHBzOi8vZC5zeW1j" +
            "Yi5jb20vY3BzMCUGCCsGAQUFBwICMBkMF2h0dHBzOi8vZC5zeW1jYi5jb20vcnBh" +
            "MBMGA1UdJQQMMAoGCCsGAQUFBwMDMFcGCCsGAQUFBwEBBEswSTAfBggrBgEFBQcw" +
            "AYYTaHR0cDovL3NmLnN5bWNkLmNvbTAmBggrBgEFBQcwAoYaaHR0cDovL3NmLnN5" +
            "bWNiLmNvbS9zZi5jcnQwHwYDVR0jBBgwFoAUz5mp6nsm9EvJjo/X8AUm7+PSp50w" +
            "HQYDVR0OBBYEFABa8K2l1Hg19YQSqCwFDvhWG46OMBEGCWCGSAGG+EIBAQQEAwIE" +
            "EDAWBgorBgEEAYI3AgEbBAgwBgEBAAEB/zANBgkqhkiG9w0BAQUFAAOCAQEAiMKY" +
            "WjeOW+VYirEXwgOoW1XqjITQiZi+uJqsXWL8N4LBeKDgg6IjOpFoctTaFHdi6XK/" +
            "T43xieeWV+LGZaqMXn5U6R4J1/DDybiqQwbJO1pIYhLuuXwcG/oPcEBzDH4GNIIx" +
            "yAENmRH9jyek00hXL49uMIe93bMqnJo9vdH6cA7R2hdoxOaav7UATifLw5DeOsLe" +
            "KjIRupyKToHPSp4Miy3RDu1d+CjNTW/rDfSZKk1lzaDapTn+0I2B8JcOyru1t5CB" +
            "ivn9ZD9cakwaV8LARObC5Z7oz/iQKlfGipQSQykSNyIZaxvQiVJqhTYZmdn+UBOY" +
            "wLz0Hl3ry5zGIqia4DCCApowggGCoAMCAQICBA7u7u4wDQYJKoZIhvcNAQEFBQAw" +
            "DzENMAsGA1UEAwwER0MtMTAeFw0yMDA0MDkxNDEzMzlaFw0yNTA0MDkxNDEzMzla" +
            "MA8xDTALBgNVBAMMBEdDLTEwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIB" +
            "AQCSoXI9kMzEiHoUtY5fGrHNzH6qpF9Sw1E39vIDiO4fb5B0eGEJyZk/r7KjLZ6b" +
            "aCmAEA0M5mBiEYybfpQf4blyqgC+zYLufwBTam2ttl7crwQi/+pTD7HKFevKPAdj" +
            "CqeqzkjodyQMYfNjXL6IWDu1Oe1nYcefahowQm/MlpTHb8BXjRaAvFPvAhnHy37m" +
            "jrFG8OcK0UA0LKaXhOCaTTZihIAUx03wnc7xFRh+bVsIfVbg27HV+C7ewcW3ZOLr" +
            "no9Gp5Gemj9PxjOCX1w6rPnH31DBCJzeWWZNerW8HVloGjTVh+yUHcgv0nG37J7h" +
            "xoWwcOpJTDLBsFWXZ2eKYaF/AgMBAAEwDQYJKoZIhvcNAQEFBQADggEBAEmQzbsc" +
            "hSP7QhJzd0mB6Bs3bAPpUl2HDF4TnjsUctK3Dq8RGhnove+yqs6QZvP3oYHyy2OX" +
            "P5odmNohr79xYNS1QSZT9IyIL/aUssWTujLKMyzzIQZd8zQl73i64Pc2HLNS272g" +
            "TuS0pYTCt2PnsQL8ZM6itCp3q9/hsg7OXbkM8iV8IsnAzSf1hozUCamly68F44L2" +
            "4NUJlfRE2zS5l9ALji3mUVXSk6xnyhggc8itFYUB2yPl5/tfCwIatH0vZni4xC7p" +
            "zCVXJtVBvpAFoB6CJ5jvY3RQovGWAkxSNQNKVau/ZZxkExGz36ybfYEYLM7155i3" +
            "rZ2OUUcHMfY6bIU=";
    }

}
