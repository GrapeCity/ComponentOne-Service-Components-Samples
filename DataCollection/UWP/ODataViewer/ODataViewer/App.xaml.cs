using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ODataViewer
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            //C1.DataConnector.LicenseManager.Key = License.Key;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }

    public static class License
    {
        public const string Key =
            "AB4BHgIeB3dPAEQAYQB0AGEAVgBpAGUAdwBlAHIAh+sngNNVWiPLmjV4nLGV3Fnt" +
            "86TuibQwz0MOdZnhLMnQETjSwbJgjsI6GZe5YzoBHMfAzZ7yTLwgXM3BXQexhFnH" +
            "WZSNHF7neFPx7oZKUsna3EfA9PMDzafka6qc0IeogK80WYP0+SGVClX0QS9EJnmU" +
            "ztZXANlTOycyuJHenv/dHBGOsnrUgzy41vDrqXHpya7F5VddcknfHGtKAvlnjZBo" +
            "krrSU5qMR+HCxAAqNYy9i4G0uXFNlmGEQr7DboaIE/ljcz+ODlZezPBimgBLfQSs" +
            "HRV6YbxDvpWoAXt4mr5AjyPi6e8TXwWNnUYiisiS6awf925fgXxGK2XNa5W2MLoM" +
            "mxHDW4cD29TDL7B5/MD0ti4gw1u96PheS2kbldFFaXR+uABHV+CRZcdT9PqmQ1Rj" +
            "M1G5HpMsw7YSWtlyinnFc7V//rfxBCdqoyJeeycEBtHDpCxgIFbgexthfKyqyaDm" +
            "d6u1bBNrRlg0oRc5e/xx/8XhYU3EXl9SfSvPTHK8eRV8slbf4Oj79MhoORmRbVJx" +
            "DKMde0ddAo/wqPsAU1xMkTbfzvzWOyRbvwvDiglnrKTI6bVA49pnE6ln9g9vQtgw" +
            "BxiITHAniMl/KNuHFRjn+z8lEDNUi4TvwdGQSch1FnjKenqh9cQMHO4D/NB0jrwq" +
            "QXX736pg1B/Ud9wAztMwggVVMIIEPaADAgECAhBBA3jSJjZZehbbJsa9EJSLMA0G" +
            "CSqGSIb3DQEBBQUAMIG0MQswCQYDVQQGEwJVUzEXMBUGA1UEChMOVmVyaVNpZ24s" +
            "IEluYy4xHzAdBgNVBAsTFlZlcmlTaWduIFRydXN0IE5ldHdvcmsxOzA5BgNVBAsT" +
            "MlRlcm1zIG9mIHVzZSBhdCBodHRwczovL3d3dy52ZXJpc2lnbi5jb20vcnBhIChj" +
            "KTEwMS4wLAYDVQQDEyVWZXJpU2lnbiBDbGFzcyAzIENvZGUgU2lnbmluZyAyMDEw" +
            "IENBMB4XDTE0MTIxMTAwMDAwMFoXDTE1MTIyMjIzNTk1OVowgYYxCzAJBgNVBAYT" +
            "AkpQMQ8wDQYDVQQIEwZNaXlhZ2kxGDAWBgNVBAcTD1NlbmRhaSBJenVtaS1rdTEX" +
            "MBUGA1UEChQOR3JhcGVDaXR5IGluYy4xGjAYBgNVBAsUEVRvb2xzIERldmVsb3Bt" +
            "ZW50MRcwFQYDVQQDFA5HcmFwZUNpdHkgaW5jLjCCASIwDQYJKoZIhvcNAQEBBQAD" +
            "ggEPADCCAQoCggEBAMEC9splW7KVpAqRB6VVB7XJm8+a60x78HZWNmvAjs/ermp7" +
            "pbh8M9HYuEVTRj8H4baXxFYq4i75e5kNr7s+nGbJ5UfM6CldqbeRsQ4zvRb6npfr" +
            "tcOBwfYOx0woeYQfI6VzLVzhPbGjUDK6qdHZLf/EcvSIKvRbu3ILOE3kuux07gQk" +
            "b++rrSBUyJKXU8nW8c+K+9sfWqHYPb8FF2IazWxhmfIdIoDNKyjr3r/wIn3jQejp" +
            "qfamiHNsU/O26KS0EcvCS5CFOKDT6vvdnAkek3kyLWef+ca/cbFo74vawYGtmw2w" +
            "zw/hcwlANIQaCAxz+5JHQZ3SEg8LLSZ+C8E4T+8CAwEAAaOCAY0wggGJMAkGA1Ud" +
            "EwQCMAAwDgYDVR0PAQH/BAQDAgeAMCsGA1UdHwQkMCIwIKAeoByGGmh0dHA6Ly9z" +
            "Zi5zeW1jYi5jb20vc2YuY3JsMGYGA1UdIARfMF0wWwYLYIZIAYb4RQEHFwMwTDAj" +
            "BggrBgEFBQcCARYXaHR0cHM6Ly9kLnN5bWNiLmNvbS9jcHMwJQYIKwYBBQUHAgIw" +
            "GQwXaHR0cHM6Ly9kLnN5bWNiLmNvbS9ycGEwEwYDVR0lBAwwCgYIKwYBBQUHAwMw" +
            "VwYIKwYBBQUHAQEESzBJMB8GCCsGAQUFBzABhhNodHRwOi8vc2Yuc3ltY2QuY29t" +
            "MCYGCCsGAQUFBzAChhpodHRwOi8vc2Yuc3ltY2IuY29tL3NmLmNydDAfBgNVHSME" +
            "GDAWgBTPmanqeyb0S8mOj9fwBSbv49KnnTAdBgNVHQ4EFgQUAFrwraXUeDX1hBKo" +
            "LAUO+FYbjo4wEQYJYIZIAYb4QgEBBAQDAgQQMBYGCisGAQQBgjcCARsECDAGAQEA" +
            "AQH/MA0GCSqGSIb3DQEBBQUAA4IBAQCIwphaN45b5ViKsRfCA6hbVeqMhNCJmL64" +
            "mqxdYvw3gsF4oOCDoiM6kWhy1NoUd2Lpcr9PjfGJ55ZX4sZlqoxeflTpHgnX8MPJ" +
            "uKpDBsk7WkhiEu65fBwb+g9wQHMMfgY0gjHIAQ2ZEf2PJ6TTSFcvj24wh73dsyqc" +
            "mj290fpwDtHaF2jE5pq/tQBOJ8vDkN46wt4qMhG6nIpOgc9KngyLLdEO7V34KM1N" +
            "b+sN9JkqTWXNoNqlOf7QjYHwlw7Ku7W3kIGK+f1kP1xqTBpXwsBE5sLlnujP+JAq" +
            "V8aKlBJDKRI3IhlrG9CJUmqFNhmZ2f5QE5jAvPQeXevLnMYiqJrgMIICmjCCAYKg" +
            "AwIBAgIEDu7u7jANBgkqhkiG9w0BAQUFADAPMQ0wCwYDVQQDDARHQy0xMB4XDTIw" +
            "MDQwOTE0MTMzOVoXDTI1MDQwOTE0MTMzOVowDzENMAsGA1UEAwwER0MtMTCCASIw" +
            "DQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAI0Ffc9ARUD72leUWYmYHa9O60xL" +
            "yLIJugLNmAS9j1ar+69qZHviBZcY/NJtewCkBtzOOItKdTjhvWIguTHaImt30E+o" +
            "9lRs2Gqzqz9OtBvz6X/wqbiAAtQbNZIFR59rgrmjAes/g8dHQlDATlbfhk/W95WM" +
            "l8n8uLTkh3t4mBM9ITlwOSFacMpgNMXEX0Z7x4RQIeTClXgvMmlnfPerXqWWVzP5" +
            "/fG+HY32WECcDDxcSKqPmMTgjknVYv0r8xv7Nnme2mG9Z8zuoAYcDoTEoophe4AI" +
            "azvrjeuzjiaNumFAOeXx8oMjOlxqJJrrNuIs0UdVc3/LLVXXleUXQY1vEncCAwEA" +
            "ATANBgkqhkiG9w0BAQUFAAOCAQEASgCBdThJeq5r6RHxrAS+vTz50gBy7eaqbd8J" +
            "AOsr+FdLupwthUQAMRYpqoziVtvg72AgX6qZcI03/nvUK+iJjb4IwgpArI4Cghkp" +
            "f0LBe87hlfwV/cIGgPXVwtu4Fpfs/sLTGGoYMnZJNxJpoap5aPQAKA9MT9Kpy5cE" +
            "03U7mw4xBfut9Htx446uMxqgOgGmj710jTdrzzX7bKKACjAvTbjjYma6pMs7HpdI" +
            "zA0bWo0ZhF4F1cAY4YbHW4KggUAw6GmP2rxzuYJD5KqaWval9l8+mKwQ/Cgv3PEy" +
            "2nj5r56HnBX7cwxQyvWJxYo/TmB2fmNtcYQjlA5qMOyU9eMnuA==";
    }

}
