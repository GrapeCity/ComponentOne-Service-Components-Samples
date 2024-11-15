// Copyright (c) 2024 FIIT B.V. | DeveloperTools (KVK:75050250). All Rights Reserved.

using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using C1.Util;
using C1.Excel;

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
using _Pen = GrapeCity.Documents.Drawing.Pen;
using _PenLineCap = GrapeCity.Documents.Drawing.PenLineCap;
using _PenLineJoin = GrapeCity.Documents.Drawing.PenLineJoin;
using _Bitmap = GrapeCity.Documents.Imaging.GcBitmap;
using _Image = GrapeCity.Documents.Drawing.Image;

using GrapeCity.Documents.Text;
using static System.Net.Mime.MediaTypeNames;
using static GrapeCity.Documents.Imaging.GcBitmapGraphics;
using GrapeCity.Documents.Svg;
using System.Xml;
using static System.Collections.Specialized.BitVector32;
using System.Security.Policy;
using System.Reflection.Metadata;
using GrapeCity.Documents.Common;
using GrapeCity.Documents.Imaging;
using System.Reflection.PortableExecutable;

namespace FileViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        XLSheet? _sheet;
        bool _svg = true;
        bool _svgXL = false;
        bool _loaded = false;
        string _path = string.Empty;
        string _result = string.Empty;
        string _svgFileName = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
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
            return "about:blank";
        }

        void ToHome()
        {
            _svgFileName = LoadBook(GetHomePath());
            webView.CoreWebView2.Navigate($"file:///{_svgFileName}");
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

        void ShowAbout(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("C1.Excel .NET Standard");
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
        }
        void ForwardPage(object sender, RoutedEventArgs e)
        {
            if (webView.CoreWebView2.CanGoForward)
            {
                webView.CoreWebView2.GoForward();
            }
        }

        private void ChangeSheet(object sender, RoutedEventArgs e)
        {
            if (_loaded && sender is MenuItem mi && mi.Tag is XLSheet sheet)
            {
                foreach (MenuItem mis in ((MenuItem)mi.Parent).Items)
                {
                    mis.IsChecked = false;
                }
                mi.IsChecked = true;
                ToSheet(sheet);
            }
        }

        private void SvgOrBitmap(object sender, RoutedEventArgs e)
        {
            if (_loaded && sender is MenuItem mi)
            {
                foreach (MenuItem mis in ((MenuItem)mi.Parent).Items)
                {
                    mis.IsChecked = false;
                }
                mi.IsChecked = true;
                _result = mi.Name;
                _svg = _result.StartsWith("SVG");
                _svgXL = _result.Equals("SVGXL");
                if (_sheet != null)
                {
                    ToSheet(_sheet);
                }
            }
        }
        async void OpenFile(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "Excel XLSX Files (*.xlsx)|*.xlsx";
            dlg.Filter += "|Excel Macro Files (*.xlsm)|*.xlsm";
            dlg.Filter += "|Excel XLS Files (*.xls)|*.xls";
            dlg.Filter += "|Comma Separated Values Files (*.csv)|*.csv";
            if (dlg.ShowDialog() == true)
            {
                var fileName = dlg.FileName.Replace('\\', '/');
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
                    sheetsMenu.Header = name;
                    sheetsMenu.Items.Clear();
                    Title = $"{Title.Split('(')[0].Trim()} ({name})";

                    // HTML result
                    foreach (var tag in "SVG|PNG|JPEG|GIF|SVGXL".Split('|'))
                    {
                        var mi = new MenuItem();
                        mi.Header = mi.Name = tag;
                        mi.Click += (s, e) => SvgOrBitmap(s, e);
                        mi.IsChecked = (tag == "SVG");
                        resultMenu.Items.Add(mi);
                    }

                    // add sheet menu
                    var count = 0;
                    foreach (XLSheet sheet in book.Sheets)
                    {
                        var mi = new MenuItem();
                        mi.Header = sheet.Name;
                        mi.Tag = sheet;
                        mi.Click += (s, e) => ChangeSheet(s, e);
                        if (sheetsMenu.Items.Count == 0)
                        {
                            mi.IsChecked = true;
                            _sheet = sheet;
                        }
                        sheetsMenu.Items.Add(mi);
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
            IRendering gr = _svg
                ? (_svgXL ? new SvgRendering(wpx, hpx) : new SvgGraphicsRendering(wpx, hpx))
                : new BitmapGraphicsRendering(wpx, hpx);
            var sr = new XLSheetRendering(sheet, gr);

            // output file
            var fileName = $"{sheetsMenu.Header}_{sheet.Name}";
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
                        string txt;
                        if (_svgXL)
                        {
                            txt = ((SvgRendering)gr).ToText();
                        }
                        else
                        {
                            var sb = new StringBuilder();
                            var svgDocument = ((SvgGraphicsRendering)gr).ToDocument();
                            svgDocument.Save(sb, settings);
                            txt = sb.ToString();
                        }
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
            ((IDisposable)gr).Dispose();

            // done
            return path;
        }

        void SaveAsFile(object sender, RoutedEventArgs e)
        {
            // sanity
            if (_sheet == null) return;

            // save as dialog
            var dlg = new SaveFileDialog
            {
                Filter = "Excel XLSX Files (*.xlsx)|*.xlsx"
            };
            dlg.Filter += "|Excel Macro Files (*.xlsm)|*.xlsm";
            dlg.Filter += "|Excel XLS Files (*.xls)|*.xls";
            dlg.Filter += "|Comma Separated Values Files (*.csv)|*.csv";
            if (dlg.ShowDialog() == true)
            {
                // saving
                var fileName = dlg.FileName.Replace('\\', '/');
                var ext = System.IO.Path.GetExtension(fileName).Trim();
                if (ext.Equals(".csv"))
                    _sheet.SaveCsv(dlg.FileName);
                else
                    _sheet.Book.Save(dlg.FileName);
                Process.Start(new ProcessStartInfo { FileName = fileName, UseShellExecute = true });
            }
        }

        private void webView_ContentLoading(object sender, Microsoft.Web.WebView2.Core.CoreWebView2ContentLoadingEventArgs e)
        {
            if (!_loaded)
            {
                _loaded = true;
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