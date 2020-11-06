using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EFCoreSamples
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
            "ACwBLAIsB4VPAEQAYQB0AGEARQBGAEMAbwByAGUAUwBhAG0AcABsAGUAcwBWqU7w" +
            "yN+d41In25jsGzmJTCIhw9+7NDMdaEbi2U+HNpo6PtcOSHnbM4s2gSUgxffAMggS" +
            "m4E2w7a3YXBT1DXD+BPhDywMtgvSUGJX8zAIY/ndPhieW51vu5UYbivWdBHevLMR" +
            "+rNoymQFJR5bvL4olYvxyqQ5EaFGzVTfj+Yyt87yrQaurw1nWnXhx0wKKtD9aVo6" +
            "lQrA2zVGRlAU8afiyoxq0/JQcgRK9P8KEplNwX54NDcC9wzYa8/o2dk644BL4GXP" +
            "ZU/qQqZxEX3s6Htgx8gw0voTMKRUah6BzYsKraGx0vUvuIv2lM6pxTG+1O6bdRhy" +
            "pp2dIAMDd2gBPcV+jh688gRTlOMzzrQ+BnOgyfoGzjwjFjZhLCuys0dh33ABGZXV" +
            "O8gbEyXxK4TfbERzS2Yiot1cpcVZ1rT+5u9i4OR49foTUlZJbntmXNIQSTtrOhcF" +
            "TBfnPhqaTqQb2vdJ0Pni6vw/EysqjE25lK3ERIz1Z0lYGASuNcP+QZVLJc+vqGYe" +
            "VhUL6wM8Z3qe4uUEXLhi3ccxsZdg8lITtL/6+DF00WADrUdx2h+g84xTrEupJrbv" +
            "HRyJKJHiV9skDPE/lBta/2HmiolWhQ55tM8E7kcIGQuQ6+jS1MuJVb0eK6p8DutO" +
            "z147VvoStQK9M+GeOPlJsBrC3n0hNUKQTtlnejCCBVUwggQ9oAMCAQICEEEDeNIm" +
            "Nll6Ftsmxr0QlIswDQYJKoZIhvcNAQEFBQAwgbQxCzAJBgNVBAYTAlVTMRcwFQYD" +
            "VQQKEw5WZXJpU2lnbiwgSW5jLjEfMB0GA1UECxMWVmVyaVNpZ24gVHJ1c3QgTmV0" +
            "d29yazE7MDkGA1UECxMyVGVybXMgb2YgdXNlIGF0IGh0dHBzOi8vd3d3LnZlcmlz" +
            "aWduLmNvbS9ycGEgKGMpMTAxLjAsBgNVBAMTJVZlcmlTaWduIENsYXNzIDMgQ29k" +
            "ZSBTaWduaW5nIDIwMTAgQ0EwHhcNMTQxMjExMDAwMDAwWhcNMTUxMjIyMjM1OTU5" +
            "WjCBhjELMAkGA1UEBhMCSlAxDzANBgNVBAgTBk1peWFnaTEYMBYGA1UEBxMPU2Vu" +
            "ZGFpIEl6dW1pLWt1MRcwFQYDVQQKFA5HcmFwZUNpdHkgaW5jLjEaMBgGA1UECxQR" +
            "VG9vbHMgRGV2ZWxvcG1lbnQxFzAVBgNVBAMUDkdyYXBlQ2l0eSBpbmMuMIIBIjAN" +
            "BgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwQL2ymVbspWkCpEHpVUHtcmbz5rr" +
            "THvwdlY2a8COz96uanuluHwz0di4RVNGPwfhtpfEViriLvl7mQ2vuz6cZsnlR8zo" +
            "KV2pt5GxDjO9Fvqel+u1w4HB9g7HTCh5hB8jpXMtXOE9saNQMrqp0dkt/8Ry9Igq" +
            "9Fu7cgs4TeS67HTuBCRv76utIFTIkpdTydbxz4r72x9aodg9vwUXYhrNbGGZ8h0i" +
            "gM0rKOvev/AifeNB6Omp9qaIc2xT87bopLQRy8JLkIU4oNPq+92cCR6TeTItZ5/5" +
            "xr9xsWjvi9rBga2bDbDPD+FzCUA0hBoIDHP7kkdBndISDwstJn4LwThP7wIDAQAB" +
            "o4IBjTCCAYkwCQYDVR0TBAIwADAOBgNVHQ8BAf8EBAMCB4AwKwYDVR0fBCQwIjAg" +
            "oB6gHIYaaHR0cDovL3NmLnN5bWNiLmNvbS9zZi5jcmwwZgYDVR0gBF8wXTBbBgtg" +
            "hkgBhvhFAQcXAzBMMCMGCCsGAQUFBwIBFhdodHRwczovL2Quc3ltY2IuY29tL2Nw" +
            "czAlBggrBgEFBQcCAjAZDBdodHRwczovL2Quc3ltY2IuY29tL3JwYTATBgNVHSUE" +
            "DDAKBggrBgEFBQcDAzBXBggrBgEFBQcBAQRLMEkwHwYIKwYBBQUHMAGGE2h0dHA6" +
            "Ly9zZi5zeW1jZC5jb20wJgYIKwYBBQUHMAKGGmh0dHA6Ly9zZi5zeW1jYi5jb20v" +
            "c2YuY3J0MB8GA1UdIwQYMBaAFM+Zqep7JvRLyY6P1/AFJu/j0qedMB0GA1UdDgQW" +
            "BBQAWvCtpdR4NfWEEqgsBQ74VhuOjjARBglghkgBhvhCAQEEBAMCBBAwFgYKKwYB" +
            "BAGCNwIBGwQIMAYBAQABAf8wDQYJKoZIhvcNAQEFBQADggEBAIjCmFo3jlvlWIqx" +
            "F8IDqFtV6oyE0ImYvriarF1i/DeCwXig4IOiIzqRaHLU2hR3Yulyv0+N8Ynnllfi" +
            "xmWqjF5+VOkeCdfww8m4qkMGyTtaSGIS7rl8HBv6D3BAcwx+BjSCMcgBDZkR/Y8n" +
            "pNNIVy+PbjCHvd2zKpyaPb3R+nAO0doXaMTmmr+1AE4ny8OQ3jrC3ioyEbqcik6B" +
            "z0qeDIst0Q7tXfgozU1v6w30mSpNZc2g2qU5/tCNgfCXDsq7tbeQgYr5/WQ/XGpM" +
            "GlfCwETmwuWe6M/4kCpXxoqUEkMpEjciGWsb0IlSaoU2GZnZ/lATmMC89B5d68uc" +
            "xiKomuAwggKaMIIBgqADAgECAgQO7u7uMA0GCSqGSIb3DQEBBQUAMA8xDTALBgNV" +
            "BAMMBEdDLTEwHhcNMjAwNTA4MDkyOTEwWhcNMjUwNTA4MDkyOTEwWjAPMQ0wCwYD" +
            "VQQDDARHQy0xMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAgYGOkxan" +
            "ydORXqlVSkapqhf9FAj+LPyug+R9fRyE6ROUu8Bl/3DYt3gqfQltLnE5J/81qoU3" +
            "LS4OI92QpOX3wJe5xJ0jwgnVtlUhmHfgVljzG/L52L32KgwfO/TefG7pkMRyic1g" +
            "jaTswbQK3keAV7BVR4/m0SuaA+1rDZvs+S8kDUq51aX95RzH6HKHvVadQXop8QSj" +
            "egYjwzGt6U/YE6vK9XdR8Ys8p43ubiDJQ8GhCpex7O71qRYa3vWVaVHjl5awmqxI" +
            "0uEIm0ZBYIoh1eU/D3cU6ZsCP2wEgeP13y3nmmX9sT3c6Qb7NBovVizlv6bRZnz2" +
            "d9zyoJRO2kYZCQIDAQABMA0GCSqGSIb3DQEBBQUAA4IBAQBhPQW5VKalAVYvZCD+" +
            "KJyuPLRGOv4PdIGcGpAXKl/h7IGNgZ10VU1Lt882LfsQNPnESymPDvNOOQg9EHay" +
            "p2qDr28bEbypcR60ckFsH0s8eOGE+nen78+Q4+uXtEK9X1H8vmhAxZFSauh2OpNL" +
            "xee29K7fRI032QJgEc2jJx4wwoctvfEctFl7HUXvqARYcsQuGR22NChITSRWrKr2" +
            "xFzJ2scUqD3ax4+oDXNuqZzGc+x/BEemjCRXXSTNYwmHBb69HtjyXh4WKQJq3ifY" +
            "EyRUJJFacD+9O+P/n0JzN2hlamUmVujcQjM2fBLnBfthNXDPUlo7eKQo9yjZi0C9" +
            "BV5T";
    }


}
