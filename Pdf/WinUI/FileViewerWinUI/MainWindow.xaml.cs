// Copyright (c) 2023 FIIT B.V. | DeveloperTools (KVK:75050250). All Rights Reserved.

using ABI.Windows.Foundation;
using C1.Excel;
using C1.Pdf;
using C1.Util;
using C1.Word;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.Win32;

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
using static System.Net.Mime.MediaTypeNames;
using static GrapeCity.Documents.Imaging.GcBitmapGraphics;
using Windows.Storage.Pickers;
using Windows.Web.UI;
using Windows.Storage;
using Windows.System;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileViewerWinUI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        bool _loaded = false;
        string _path = string.Empty;
        string _pdfFileName = string.Empty;

        /// <summary></summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        string GetHome()
        {
            var a = Assembly.GetExecutingAssembly();
            var dir = System.IO.Path.GetDirectoryName(a.Location);
            var folder = System.IO.Path.GetDirectoryName(a.Location);
            return System.IO.Path.Combine(dir ?? string.Empty, "Results\\description.pdf");
        }
        string GetResult(string path)
        {
            var a = Assembly.GetExecutingAssembly();
            var dir = System.IO.Path.GetDirectoryName(a.Location);
            dir ??= System.IO.Path.GetDirectoryName(path);
            dir = System.IO.Path.Combine(dir ?? string.Empty, "Results");
            var name = System.IO.Path.GetFileNameWithoutExtension(path).Trim();
            return System.IO.Path.Combine(dir, $"_{name}.pdf");
        }

        void ToHome()
        {
            _pdfFileName = GetHome();
            webView.Source = new Uri($"file://{_pdfFileName}");
        }

        void ExitProgram(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void ShowAbout(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = "About",
                Content = "C1.Pdf .NET Standard",
                CloseButtonText = "Close"
            };

            dialog.XamlRoot = this.Content.XamlRoot;

            await dialog.ShowAsync();
        }
        void HomePage(object sender, RoutedEventArgs e)
        {
            ToHome();
        }
        void RefreshPage(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_path) && File.Exists(_path))
            {
                var pdf = new C1PdfDocument();
                var rc = pdf.PageRectangle;
                if (PrintTo(pdf, rc, _path))
                {
                    // save to file
                    _pdfFileName = GetResult(_path);
                    pdf.Save(_pdfFileName);
                    webView.CoreWebView2.Navigate($"file:///{_pdfFileName}");
                    return;
                }
            }
            ToHome();
        }
        void BackwardPage(object sender, RoutedEventArgs e)
        {
            if (webView.CoreWebView2.CanGoBack)
            {
                webView.CoreWebView2.GoBack();
            }
            else
            {
                webView.Source = new Uri("https://www.grapecity.com");
            }
        }
        void ForwardPage(object sender, RoutedEventArgs e)
        {
            if (webView.CoreWebView2.CanGoForward)
            {
                webView.CoreWebView2.GoForward();
            }
            else
            {
                webView.Source = new Uri("https://www.grapecity.com");
            }
        }

        private async void OpenFile(object sender, RoutedEventArgs e)
        {
            var window = new Microsoft.UI.Xaml.Window();
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".xlsx");
            picker.FileTypeFilter.Add(".xlsm");
            picker.FileTypeFilter.Add(".xls");
            picker.FileTypeFilter.Add(".html");
            picker.FileTypeFilter.Add(".htm");
            picker.FileTypeFilter.Add(".csv");
            picker.FileTypeFilter.Add(".txt");
            picker.FileTypeFilter.Add(".docx");
            picker.FileTypeFilter.Add(".rtf");
            picker.FileTypeFilter.Add(".pdf");
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                var fileName = file.Path.Replace('\\', '/');
                _pdfFileName = PrintToPdf(fileName);
                await webView.EnsureCoreWebView2Async();
                webView.CoreWebView2.Navigate($"file:///{_pdfFileName}");
            }

        }

        string PrintToPdf(string path)
        {
            _path = string.Empty;
            var ext = System.IO.Path.GetExtension(path).Trim();
            switch (ext)
            {
                case ".pdf":
                    return path;
                case ".htm":
                case ".html":
                case ".txt":
                case ".rtf":
                case ".docx":
                case ".xlsm":
                case ".xlsx":
                case ".xls":
                case ".csv":
                    var pdf = new C1PdfDocument();
                    var rc = pdf.PageRectangle;
                    if (PrintTo(pdf, rc, path))
                    {
                        // save to file
                        _path = path;
                        path = GetResult(path);
                        pdf.Save(path);
                        return path;
                    }
                    break;
            }
            return GetHome();
        }

        bool PrintTo(IFlowDocument flow, _Rect rc, string path)
        {
            var ext = System.IO.Path.GetExtension(path).Trim();
            switch (ext)
            {
                case ".htm":
                case ".html":
                    // initialization
                    var html = System.IO.File.ReadAllText(path);

                    //// html comes back with Unicode character codes, other escaped characters, and
                    //// wrapped in double quotes, so I'm using this code to clean it up for what
                    //html = Regex.Unescape(html);
                    //html = html.Remove(0, 1);
                    //html = html.Remove(html.Length - 1, 1);

                    // render html code
                    RenderHtml(flow, rc, html, 1);
                    return true;
                case ".txt":
                case ".rtf":
                case ".docx":
                    // initialization
                    var document = new C1WordDocument();
                    document.Load(path);

                    // render html code
                    RenderDocument(flow, rc, document, 1);
                    return true;
                case ".xlsm":
                case ".xlsx":
                case ".xls":
                case ".csv":
                    // initialization
                    var book = new C1XLBook();
                    if (ext.Equals(".csv"))
                        book.Sheets[0].LoadCsv(path);
                    else
                        book.Load(path);

                    // render all sheet of the Excel Book
                    RenderBook(flow, rc, book);
                    return true;
            }
            return false;
        }

        void RenderBook(IFlowDocument flow, _Rect rcPage, C1XLBook book)
        {
            // get ready to work
            var title = Title;
            DateTime bs = DateTime.Now;

            // print the worknook
            var count = 0;
            foreach (XLSheet sheet in book.Sheets)
            {
                // next page for next sheet
                if (count > 0)
                {
                    flow.NewPage();
                }

                // render sheet
                for (var start = 0F; ;)
                {
                    // render this part
                    this.Title = string.Format("Page {0}", flow.PageCount);
                    count++;

                    _Rect rc = rcPage;
                    if (flow is C1PdfDocument pdf)
                        start = pdf.DrawSheet(sheet, rc, start);
                    else if (flow is C1WordDocument word)
                        start = word.DrawSheet(sheet, rc, start);
                    else
                        start = _Float.MaxValue;

                    // done?
                    if (start >= _Float.MaxValue)
                    {
                        break;
                    }

                    // skip page
                    flow.NewPage();
                }
            }

            // done
            TimeSpan ts = DateTime.Now.Subtract(bs);
            this.Title = string.Format("Done in {0:f2}s", ts.TotalSeconds);
            Thread.Sleep(2000);
            this.Title = title;
        }

        void RenderDocument(IFlowDocument flow, _Rect rcPage, C1WordDocument document, int cols)
        {
            RenderHtml(flow, rcPage, document.ToHtmlText(), cols);
        }

        void RenderHtml(IFlowDocument flow, _Rect rcPage, string text, int cols)
        {
            // get ready to work
            var title = Title;
            DateTime bs = DateTime.Now;

            // get number of columns, create layout array
            var columns = new _Rect[cols];

            // 4 or more columns? switch to landscape
            if (cols >= 4) flow.Landscape = true;

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
            TimeSpan ts = DateTime.Now.Subtract(bs);
            this.Title = string.Format("Done in {0:f2}s", ts.TotalSeconds);
            Thread.Sleep(2000);
            this.Title = title;
        }

        async void SaveAsFile(object sender, RoutedEventArgs e)
        {
            var window = new Microsoft.UI.Xaml.Window();
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            var picker = new FileSavePicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeChoices.Clear();
            picker.FileTypeChoices.Add("PDF Files", new List<string>() { ".pdf" });
            // picker.FileTypeChoices.Add("Word Files", new List<string>() { ".docx" });
            // picker.FileTypeChoices.Add("RTF Files", new List<string>() { ".rtf" });

            // Get the suggested file name from the open file path
            string suggestedFileName = System.IO.Path.GetFileNameWithoutExtension(_pdfFileName);
            string suggestedFileExtension = System.IO.Path.GetExtension(_pdfFileName);

            // Check if the suggested file name is not empty
            if (!string.IsNullOrEmpty(suggestedFileName))
            {
                picker.SuggestedFileName = suggestedFileName;
                picker.DefaultFileExtension = suggestedFileExtension;
            }

            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
            StorageFile file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                IFlowDocument flow = null;
                var ext = file.FileType.ToLower();
                switch (ext)
                {
                    case ".rtf":
                    case ".docx":
                        flow = new C1WordDocument();
                        break;


                }
                if (!string.IsNullOrEmpty(_pdfFileName) && flow != null)
                {
                    var sz = flow.PageSize;
                    var rc = new _Rect(0, 0, sz.Width, sz.Height);
                    if (PrintTo(flow, rc, file.Path))
                    {
                        if (flow is C1WordDocument word)
                            word.Save(file.Path);
                    }
                }
                else
                {
                    StorageFile sourceFile = await StorageFile.GetFileFromPathAsync(_pdfFileName);
                    await sourceFile.CopyAndReplaceAsync(file);
                }
                // Open the saved file
                await Launcher.LaunchFileAsync(file);
            }
        }
        private async void webView_Loading(FrameworkElement sender, object args)
        {

            if (!_loaded)
            {
                _loaded = true;
                await webView.EnsureCoreWebView2Async();
                ToHome();
            }
        }
    }
}
