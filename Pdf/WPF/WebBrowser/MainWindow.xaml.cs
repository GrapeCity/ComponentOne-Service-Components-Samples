using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using Microsoft.Web.WebView2.Core.DevToolsProtocolExtension;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Linq;

using Microsoft.Win32;

using C1.Pdf;
using C1.Word;
using C1.Util;

using _Float = System.Single;
using _Size = System.Drawing.SizeF;
using _Point = System.Drawing.PointF;
using _Rect = System.Drawing.RectangleF;
using _Color = System.Drawing.Color;
using _Matrix = System.Numerics.Matrix3x2;
using _FillMode = GrapeCity.Documents.Drawing.FillMode;
using _DashStyle = GrapeCity.Documents.Drawing.DashStyle;
using _PaperKind = GrapeCity.Documents.Common.PaperKind;
using _FontStyle = C1.Util.FontStyle;
using _Font = C1.Util.Font;
using _Pen = GrapeCity.Documents.Drawing.Pen;
using _PenLineCap = GrapeCity.Documents.Drawing.PenLineCap;
using _PenLineJoin = GrapeCity.Documents.Drawing.PenLineJoin;
using _Bitmap = GrapeCity.Documents.Imaging.GcBitmap;
using _Image = GrapeCity.Documents.Drawing.Image;
using System.Threading;
using Microsoft.Web.WebView2.WinForms;
//using System.Windows.Controls;
//using Microsoft.Web.WebView2.WinForms;

namespace C1.WebBrowser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool _isNavigating = false;
        public static RoutedCommand CallCDPMethodCommand = new RoutedCommand();
        DevToolsProtocolHelper _cdpHelper;
        DevToolsProtocolHelper cdpHelper
        {
            get
            {
                if (webView == null || webView.CoreWebView2 == null)
                {
                    throw new Exception("Initialize WebView before using DevToolsProtocolHelper.");
                }

                if (_cdpHelper == null)
                {
                    _cdpHelper = webView.CoreWebView2.GetDevToolsProtocolHelper();
                }

                return _cdpHelper;
            }
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        void GoToPageCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = webView != null && !_isNavigating;
        }

        async void GoToPageCmdExecuted(object target, ExecutedRoutedEventArgs e)
        {
            await webView.EnsureCoreWebView2Async();
            webView.CoreWebView2.Navigate((string)e.Parameter);
        }

        void ExitProgram(object sender, RoutedEventArgs e)
        {
            //webView.Dispose();
            Close();
        }

        void ShowAbout(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("C1.Pdf .NET Standard");
        }
        void HomePage(object sender, RoutedEventArgs e)
        {
            webView.Source = new Uri("about:blank");
        }
        async void RefreshPage(object sender, RoutedEventArgs e)
        {
            await cdpHelper.Page.ReloadAsync();
        }

        void BackwardPage(object sender, RoutedEventArgs e)
        {
            if (webView.CoreWebView2.CanGoBack)
            {
                webView.CoreWebView2.GoBack();
            }
        }
        void ForwardPage(object sender, RoutedEventArgs e)
        {
            if (webView.CoreWebView2.CanGoForward)
            {
                webView.CoreWebView2.GoForward();
            }
        }

        async void OpenFile(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "HTML Files (*.html)|*.html|HTML Files (*.htm)|*.htm"
            };
            if (dlg.ShowDialog() == true)
            {
                var fileName = dlg.FileName.Replace('\\', '/');
                await webView.EnsureCoreWebView2Async();
                webView.CoreWebView2.Navigate($"file:///{fileName}");
            }
        }

        //---------------------------------------------------------------------
        #region -- save to PDF, DOCX, RTF and etc.

        async void SaveAsPage(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf|MS Word Files (*.docx)|*.docx|RTF Files (*.rtf)|*.rtf"
            };
            if (dlg.ShowDialog() == true)
            {
                IFlowDocument flow = null;
                try
                {
                    // create document
                    _Rect rcPage;
                    var fileName = dlg.FileName;
                    var ext = System.IO.Path.GetExtension(fileName);
                    switch (ext)
                    {
                        case ".pdf":
                        default:
                            flow = new C1PdfDocument();
                            rcPage = ((C1PdfDocument)flow).PageRectangle;
                            break;
                        case ".docx":
                        case ".rtf":
                            flow = new C1WordDocument();
                            rcPage = new _Rect(new _Point(-72, -72), flow.PageSize);
                            break;
                    }
                    // get actual html code
                    var html = await webView.ExecuteScriptAsync("document.documentElement.outerHTML;");
                    //html = await JsonSerializer.DeserializeAsync<string>(task);

                    // html comes back with Unicode character codes, other escaped characters, and
                    // wrapped in double quotes, so I'm using this code to clean it up for what
                    html = Regex.Unescape(html);
                    html = html.Remove(0, 1);
                    html = html.Remove(html.Length - 1, 1);

                    // render html code
                    RenderHtml(flow, rcPage, html, 1);

                    // save to file
                    using (var stream = dlg.OpenFile())
                    {
                        flow.Save(stream, ext);
                    }
                    Process.Start(new ProcessStartInfo { FileName = fileName, UseShellExecute = true });
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                }
                finally
                {
                    if (flow is C1PdfDocument pdf)
                    {
                        pdf.Dispose();
                    }
                    else if (flow is C1WordDocument word)
                    {
                        word.Dispose();
                    }
                }
            }
        }

        void RenderHtml(IFlowDocument flow, _Rect rcPage, string text, int cols)
        {
            // get ready to work
            var title = Title;
            //button1.Enabled = false;
            DateTime bs = DateTime.Now;

            // get number of columns, create layout array
            var columns = new _Rect[cols];

            // 4 or more columns? switch to landscape
            if (cols >= 4) flow.Landscape = true;

            // apply document orientation
            //_c1pdf.Landscape = _btnLandscape.Checked;

            // create one rectangle per column
            rcPage.Inflate(-50, -50);
            for (int i = 0; i < cols; i++)
            {
                _Rect rcc = rcPage;
                rcc.Width /= cols;
                rcc.Offset(rcc.Width * i, 0);
                rcc.Inflate(-10, -10);
                columns[i] = rcc;
            }

            // print the HTML string spanning multiple pages
            flow.Clear();
            var _currentColumn = 0;
            _Font font = new _Font("Times New Roman", 12);
            //_Pen pen = new _Pen(_Color.LightCoral, 0.01f);
            for (var start = 0F; ;)
            {
                // render this part
                this.Title = string.Format("Page {0} Column {1}", flow.PageCount, _currentColumn + 1);
                //Application.DoEvents();
                _Rect rc = columns[_currentColumn];
                start = flow.DrawStringHtml(text, font, _Color.Black, rc, start);
                //flow.DrawRectangle(pen, rc);

                // done?
                if (start >= _Float.MaxValue)
                {
                    break;
                }

                // skip page/column
                _currentColumn++;
                if (_currentColumn >= columns.Length)
                {
                    _currentColumn = 0;
                    flow.NewPage();
                }
            }

            // done
            //button1.Enabled = true;
            TimeSpan ts = DateTime.Now.Subtract(bs);
            this.Title = string.Format("Done in {0:f2}s", ts.TotalSeconds);
            Thread.Sleep(2000);
            this.Title = title;

            // show the result
            //string fn = Path.Combine(Application.StartupPath, "html.pdf");
            //_c1pdf.Save(fn);
            //System.Diagnostics.Process.Start(fn);
        }
        #endregion

        #region CDP_COMMANDS
        async void ShowFPSCounter(object sender, RoutedEventArgs e)
        {
            await cdpHelper.Overlay.SetShowFPSCounterAsync(true);
        }

        async void HideFPSCounter(object sender, RoutedEventArgs e)
        {
            await cdpHelper.Overlay.SetShowFPSCounterAsync(false);
        }

        async void SetPageScaleTo4(object sender, RoutedEventArgs e)
        {
            await cdpHelper.Emulation.SetPageScaleFactorAsync(4);
        }
       
        async void ResetPageScale(object sender, RoutedEventArgs e)
        {
            await cdpHelper.Emulation.SetPageScaleFactorAsync(1);
        }

        async void CaptureSnapshot(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine(await cdpHelper.Page.CaptureSnapshotAsync());
        }

        async void GetAllCookies(object sender, RoutedEventArgs e) 
        {
            Network.Cookie[] cookies = await cdpHelper.Network.GetAllCookiesAsync();
            StringBuilder cookieResult = new StringBuilder(cookies.Count() + " cookie(s) received\n");
            foreach (var cookie in cookies)
            {
                cookieResult.Append($"\n{cookie.Name} {cookie.Value} {(cookie.Session ? "[session cookie]" : cookie.Expires.ToString("G"))}");
            }
            MessageBox.Show(cookieResult.ToString(), "Cookies");
        }

        async void AddOrUpdateCookie(object target, RoutedEventArgs e)
        {
            bool cookie = await cdpHelper.Network.SetCookieAsync("CookieName", "CookieValue", null, "https://www.bing.com/");
            MessageBox.Show(cookie ? "Cookie is added/updated successfully" : "Error adding/updating cookie", "Cookies");
        }

        async void ClearAllCookies(object sender, RoutedEventArgs e)
        {
            await cdpHelper.Network.ClearBrowserCookiesAsync();
            MessageBox.Show("Browser cookies are deleted", "Cookies");
        }

        async void SetGeolocation(object sender, RoutedEventArgs e)
        {
            double latitude = 36.553085;
            double longitude = 103.97543;
            double accuracy = 1;
            await cdpHelper.Emulation.SetGeolocationOverrideAsync(latitude, longitude, accuracy);
            MessageBox.Show("Overridden the Geolocation Position", "Geolocation");
        }

        async void ClearGeolocation(object sender, RoutedEventArgs e)
        {
            await cdpHelper.Emulation.ClearGeolocationOverrideAsync();
            MessageBox.Show("Cleared overridden Geolocation Position", "Geolocation");
        }
        #endregion

        #region CDP_EVENTS
        async void SubscribeToDataReceived(object sender, RoutedEventArgs e)
        {
            await cdpHelper.Network.EnableAsync();
            cdpHelper.Network.DataReceived += PrintDataReceived;
            MessageBox.Show("Subscribed to DataReceived Event!", "DataReceived");
        }

        void UnsubscribeFromDataReceived(object sender, RoutedEventArgs e)
        {
            cdpHelper.Network.DataReceived -= PrintDataReceived;
            MessageBox.Show("Unsubscribed from DataReceived Event!", "DataReceived");
        }

        void PrintDataReceived(object sender, Network.DataReceivedEventArgs args)
        {
            Trace.WriteLine(String.Format("DataReceived Event Args - Timestamp: {0}   Request Id: {1}   DataLength: {2}", args.Timestamp, args.RequestId, args.DataLength));
        }

        async void SubscribeToAnimationCreated(object sender, RoutedEventArgs e)
        {
            await cdpHelper.Animation.EnableAsync();
            cdpHelper.Animation.AnimationCreated += PrintAnimationCreated;
            MessageBox.Show("Subscribed to AnimationCreated Event!", "AnimationCreated");
        }

        void UnsubscribeFromAnimationCreated(object sender, RoutedEventArgs e)
        {
            cdpHelper.Animation.AnimationCreated -= PrintAnimationCreated;
            MessageBox.Show("Unsubscribed from AnimationCreated Event!", "AnimationCreated");
        }

        void PrintAnimationCreated(object sender, Animation.AnimationCreatedEventArgs args)
        {
            Trace.WriteLine(String.Format("AnimationCreated Event Args - Id: {0}", args.Id));
        }

        async void SubscribeToDocumentUpdated(object sender, RoutedEventArgs e)
        {
            await cdpHelper.DOM.EnableAsync();
            cdpHelper.DOM.DocumentUpdated += PrintDocumentUpdated;
            MessageBox.Show("Subscribed to DocumentUpdated Event!", "DocumentUpdated");
        }

        void UnsubscribeFromDocumentUpdated(object sender, RoutedEventArgs e)
        {
            cdpHelper.DOM.DocumentUpdated -= PrintDocumentUpdated;
            MessageBox.Show("Unsubscribed from DocumentUpdated Event!", "DocumentUpdated");
        }

        void PrintDocumentUpdated(object sender, DOM.DocumentUpdatedEventArgs args)
        {
            Trace.WriteLine("DocumentUpdated Event Args - No Parameters", "DocumentUpdated");
        }

        async void SubscribeToDownloadWillBegin(object sender, RoutedEventArgs e)
        {
            await cdpHelper.Page.EnableAsync();
            cdpHelper.Page.DownloadWillBegin += PrintDownloadWillBegin;
            MessageBox.Show("Subscribed to DownloadWillBegin Event!", "DownloadWillBegin");
        }

        void UnsubscribeFromDownloadWillBegin(object sender, RoutedEventArgs e)
        {
            cdpHelper.Page.DownloadWillBegin -= PrintDownloadWillBegin;
            MessageBox.Show("Unsubscribed from DownloadWillBegin Event!", "DownloadWillBegin");
        }

        void PrintDownloadWillBegin(object sender, Page.DownloadWillBeginEventArgs args)
        {
            Trace.WriteLine(String.Format("DownloadWillBegin Event Args - FrameId: {0}   Guid: {1}   URL: {2}", args.FrameId, args.Guid, args.Url));
        }
        #endregion
    }
}
