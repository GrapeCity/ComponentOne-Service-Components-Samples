// Copyright (c) 2023 FIIT B.V. | DeveloperTools (KVK:75050250). All Rights Reserved.

using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.System;

using Microsoft.Win32;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using C1.Excel;
using C1.Util;

using _Float = System.Single;
using _Size = System.Drawing.SizeF;
using _Point = System.Drawing.PointF;
using _Rect = System.Drawing.RectangleF;
using _Color = System.Drawing.Color;
using _Matrix = System.Numerics.Matrix3x2;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ExcelViewerWinUI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        XLSheet _sheet;
        bool _svg = true;
        bool _loaded = false;
        string _path = string.Empty;
        string _result = string.Empty;
        string _svgFileName = string.Empty;

        public MainWindow()
        {
            this.InitializeComponent();
        }

        string GetHomePath()
        {
            var a = Assembly.GetExecutingAssembly();
            var dir = System.IO.Path.GetDirectoryName(a.Location);
            var folder = System.IO.Path.GetDirectoryName(a.Location);
            return System.IO.Path.Combine(dir ?? string.Empty, "Results/Description.xlsx");
        }
        string GetSvgResult()
        {
            if (_sheet != null)
                return PrintToSvg(_sheet);
            return "https://developer.mescius.com/componentone";
        }

        void ToHome()
        {
            _svgFileName = LoadBook(GetHomePath());
            webView.Source = new Uri($"file://{_svgFileName}");

        }
        void ToSheet(XLSheet sheet)
        {
            _sheet = sheet;
            _svgFileName = GetSvgResult();
            webView.CoreWebView2.Navigate($"file:///{_svgFileName}");
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
                Content = "C1.Excel .NET Standard",
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
            if (_sheet != null)
            {
                ToSheet(_sheet);
            }
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

        private void ChangeSheet(object sender, RoutedEventArgs e)
        {
            if (_loaded && sender is ToggleMenuFlyoutItem mi && mi.Tag is XLSheet sheet)
            {
                foreach (var item in _sheetsMenu.Items)
                {
                    if (item is ToggleMenuFlyoutItem menuItem)
                    {
                        menuItem.IsChecked = false;
                    }
                }
                mi.IsChecked = true;
                ToSheet(sheet);
            }
        }

        private void SvgOrBitmap(object sender, RoutedEventArgs e)
        {
            if (_loaded && sender is ToggleMenuFlyoutItem mi)
            {
                foreach (var item in _resultMenu.Items)
                {
                    if (item is ToggleMenuFlyoutItem menuItem)
                    {
                        menuItem.IsChecked = false;
                    }
                }
                mi.IsChecked = true;
                _result = mi.Name;
                _svg = (_result == "SVG");
                if (_sheet != null)
                {
                    ToSheet(_sheet);
                }
            }
        }

        async void OpenFile(object sender, RoutedEventArgs e)
        {
            var window = new Microsoft.UI.Xaml.Window();
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".xlsx");
            picker.FileTypeFilter.Add(".xlsm");
            picker.FileTypeFilter.Add(".xls");
            picker.FileTypeFilter.Add(".csv");
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                var fileName = file.Path.Replace('\\', '/');
                _svgFileName = LoadBook(fileName);
                await webView.EnsureCoreWebView2Async();
                webView.CoreWebView2.Navigate($"file:///{_svgFileName}");
            }
        }

        string LoadBook(string path)
        {
            _path = string.Empty;
            var ext = System.IO.Path.GetExtension(path).Trim();
            switch (ext)
            {
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

                    // clear sheet menu
                    var name = System.IO.Path.GetFileName(path);
                    _sheetsMenu.Text = name;
                    _sheetsMenu.Items.Clear();
                    _resultMenu.Items.Clear();
                    Title = $"{Title.Split('(')[0].Trim()} ({name})";

                    // HTML result
                    foreach (var tag in "SVG|PNG|JPEG|GIF".Split('|'))
                    {
                        var mi = new ToggleMenuFlyoutItem();
                        mi.Text = mi.Name = tag;
                        mi.Click += (s, e) => SvgOrBitmap(s, e);
                        mi.IsChecked = (tag == "SVG");
                        _resultMenu.Items.Add(mi);
                    }

                    // add sheet menu
                    var count = 0;
                    foreach (XLSheet sheet in book.Sheets)
                    {
                        var mi = new ToggleMenuFlyoutItem();
                        mi.Text = sheet.Name;
                        mi.Tag = sheet;
                        mi.Click += (s, e) => ChangeSheet(s, e);
                        if (_sheetsMenu.Items.Count == 0)
                        {
                            mi.IsChecked = true;
                            _sheet = sheet;
                        }
                        _sheetsMenu.Items.Add(mi);
                        count++;
                    }
                    break;
            }
            return GetSvgResult();
        }

        string PrintToSvg(XLSheet sheet)
        {
            // paper size
            var paper = new _Size(595.3f, 841.9f);      // A4 by default
            if (sheet.PrintSettings != null)
            {
                paper = sheet.PrintSettings.PaperSize;
                if (sheet.PrintSettings.Landscape)
                {
                    paper = new _Size(paper.Height, paper.Width);
                }
            }

            // convert to pixels
            int wpx = C1XLBook.TwipsToPixels(20 * paper.Width);
            int hpx = C1XLBook.TwipsToPixels(20 * paper.Height);

            // render sheet
            var r = new _Rect(0, 0, wpx, hpx);
            GraphicsRendering gr = _svg
                ? new SvgGraphicsRendering(wpx, hpx)
                : new BitmapGraphicsRendering(wpx, hpx);
            var sr = new XLSheetRendering(sheet, gr);

            // output file
            var fileName = $"{_sheetsMenu.Text}_{sheet.Name}";
            fileName = fileName.Replace('.', '_');
            var dir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = System.IO.Path.Combine(dir ?? string.Empty, "Results", $"{fileName}.html");

            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = Encoding.UTF8;
                settings.Indent = true;
                var doc = XmlWriter.Create(fs, settings);

                // create new XHTML document with body and head tags
                doc.WriteStartElement("html");
                doc.WriteStartElement("head");

                // add title from file name
                var title = sheet.Name;

                // to avoid unclosed title tag
                doc.WriteElementString("title", string.IsNullOrEmpty(title) ? " " : title);

                // add UTF-8 encoding
                doc.WriteStartElement("meta");
                doc.WriteAttributeString("charset", "utf-8");
                doc.WriteEndElement();

                // styles
                doc.WriteStartElement("style");
                doc.WriteAttributeString("type", "text/css");
                doc.WriteString(GetResourceText("Result.css"));
                doc.WriteEndElement();

                // close head
                doc.WriteEndElement();

                // add body
                doc.WriteStartElement("body");

                // add content element
                doc.WriteStartElement("div");
                doc.WriteAttributeString("id", "content");

                // sheet layout
                sr.Layout(paper.Width, paper.Height);

                // render sheet
                int count = 0;
                var rc = new _Rect(0, 0, paper.Width, paper.Height);
                for (var start = 0F; ;)
                {
                    // clear bitmaps
                    if (!_svg)
                    {
                        gr.SetFill(_Color.White);
                        gr.DrawRect(r.X, r.Y, r.Width, r.Height);
                    }

                    // one page rendering
                    start = sr.Render(rc, start);

                    // section is created for page content
                    doc.WriteStartElement("section");

                    // set page ID
                    doc.WriteAttributeString("id", $"page{1 + count}");

                    // make page-like view
                    doc.WriteAttributeString("style", $"width:{wpx}px;height:{hpx}px;");

                    // save image
                    if (_svg)
                    {
                        var sb = new StringBuilder();
                        var svgDocument = ((SvgGraphicsRendering)gr).ToDocument();
                        svgDocument.Save(sb, settings);
                        var txt = sb.ToString();
                        int idx = txt.IndexOf("<svg");
                        if (idx > 0)
                        {
                            txt = txt.Substring(idx);
                        }
                        doc.WriteRaw(txt);
                    }
                    else
                    {
                        doc.WriteStartElement("img");
                        using (var ms = new MemoryStream())
                        {
                            var result = _result.ToLower();
                            if (result == "jpeg")
                            {
                                ((BitmapGraphicsRendering)gr).Bitmap.SaveAsJpeg(ms);
                            }
                            else if (result == "gif")
                            {
                                ((BitmapGraphicsRendering)gr).Bitmap.SaveAsGif(ms);
                            }
                            else
                            {
                                ((BitmapGraphicsRendering)gr).Bitmap.SaveAsPng(ms);
                                result = "png";
                            }
                            doc.WriteAttributeString("src", $"data:image/{result};base64, {System.Convert.ToBase64String(ms.ToArray())}");
                        }
                        doc.WriteAttributeString("alt", $"Page {1 + count}");
                        doc.WriteEndElement();
                    }

                    // close tags
                    doc.WriteEndElement(); // section

                    // done?
                    if (start == _Float.MaxValue)
                    {
                        break;
                    }

                    // page counter
                    count++;
                }

                doc.WriteEndElement(); // div#content

                doc.WriteEndElement(); // body
                doc.WriteEndElement(); // html

                // save html document
                doc.Flush();
            }

            // disposing
            gr.Dispose();

            // done
            return path;
        }

        async void SaveAsFile(object sender, RoutedEventArgs e)
        {
            if (_sheet == null) return;

            var window = new Microsoft.UI.Xaml.Window();
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            var picker = new FileSavePicker();
            picker.FileTypeChoices.Clear();
            picker.FileTypeChoices.Add("Excel Macro Files (*.xlsm)", new List<string>() { ".xlsm" });
            picker.FileTypeChoices.Add("Excel XLS Files (*.xls)", new List<string>() { ".xls" });
            picker.FileTypeChoices.Add("Comma Separated Values Files (*.csv)", new List<string>() { ".csv" });
            // Get the suggested file name from the open file path
            string suggestedFileName = System.IO.Path.GetFileNameWithoutExtension(_svgFileName);
            string suggestedFileExtension = System.IO.Path.GetExtension(_svgFileName);

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
                var ext = System.IO.Path.GetExtension(file.Path).Trim();
                if (ext.Equals(".csv"))
                    _sheet.SaveCsv(file.Path);
                else
                    _sheet.Book.Save(file.Path);
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

        static string GetResourceText(string resource)
        {
            resource = resource.ToLower();
            var assembly = Assembly.GetExecutingAssembly();
            foreach (string res in assembly.GetManifestResourceNames())
            {
                if (res.ToLower().EndsWith(resource))
                {
                    var stream = assembly.GetManifestResourceStream(res);
                    if (stream != null)
                    {
                        using (stream)
                        {
                            var reader = new StreamReader(stream, Encoding.UTF8, true);
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}

