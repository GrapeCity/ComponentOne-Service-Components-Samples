// Copyright (c) 2023 FIIT B.V. | DeveloperTools (KVK:75050250). All Rights Reserved.

using C1.Excel;
using C1.Util;
using GrapeCity.Documents.Imaging;
using GrapeCity.Documents.Svg;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Xml;

namespace ExcelViewerWin
{
    /// <summary>
    /// Interaction logic for Form1.Designer.cs
    /// </summary>
    public partial class Form1 : Form
    {
        XLSheet _sheet;
        bool _svg = true;
        bool _loaded = false;
        string _path = string.Empty;
        string _result = string.Empty;
        string _svgFileName = string.Empty;

        /// <summary>
        /// Constructor for Form1
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the path of the home directory
        /// </summary>
        /// <returns>The path of the home directory</returns>
        private string GetHomePath()
        {
            var a = Assembly.GetExecutingAssembly();
            var dir = System.IO.Path.GetDirectoryName(a.Location);
            var folder = System.IO.Path.GetDirectoryName(a.Location);
            return System.IO.Path.Combine(dir ?? string.Empty, "Results/Description.xlsx");
        }

        /// <summary>
        /// Gets the SVG result
        /// </summary>
        /// <returns>The SVG result or Url if _sheet is null</returns>
        private string GetSvgResult()
        {
            if (_sheet != null)
                return PrintToSvg(_sheet);
            return "https://developer.mescius.com/componentone";
        }

        /// <summary>
        /// Navigates to the home page
        /// </summary>
        private void ToHome()
        {
            _svgFileName = LoadBook(GetHomePath());
            webBrowser1.CoreWebView2.Navigate($"file:///{_svgFileName}");
        }

        /// <summary>
        /// Navigates to a specific sheet
        /// </summary>
        /// <param name="sheet">The sheet to navigate to</param>
        private void ToSheet(XLSheet sheet)
        {
            _sheet = sheet;
            _svgFileName = GetSvgResult();
            webBrowser1.CoreWebView2.Navigate($"file:///{_svgFileName}");
        }

        /// <summary>
        /// Exits the program
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The event arguments</param>
        public void ExitProgram(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Shows information about the program
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The event arguments</param>`    
        public void ShowAbout(object sender, EventArgs e)
        {
            MessageBox.Show("C1.Excel .NET Standard");
        }

        /// <summary>
        /// Navigates to the home page
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The event arguments</param>
        public void HomePage(object sender, EventArgs e)
        {
            ToHome();
        }

        /// <summary>
        /// Refreshes the page
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The event arguments</param>
        public void RefreshPage(object sender, EventArgs e)
        {
            if (_sheet != null)
            {
                ToSheet(_sheet);
            }
        }

        /// <summary>
        /// Navigates backward in the page of the viewer
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The event arguments</param>
        public void BackwardPage(object sender, EventArgs e)
        {
            if (webBrowser1.CoreWebView2.CanGoBack)
            {
                webBrowser1.CoreWebView2.GoBack();
            }
        }

        /// <summary>
        /// Navigates forward in the page of the viewer
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The event arguments</param>
        public void ForwardPage(object sender, EventArgs e)
        {
            if (webBrowser1.CoreWebView2.CanGoBack)
            {
                webBrowser1.CoreWebView2.GoBack();
            }
        }

        /// <summary>
        /// Changes the active sheet
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The event arguments</param>
        private void ChangeSheet(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem mi && mi.Tag is XLSheet sheet)
            {
                foreach (ToolStripMenuItem item in sheetsMenu.DropDownItems)
                {
                    if (item != mi && item.Checked)
                        item.Checked = false;
                }

                mi.Checked = true;

                _sheet = sheet;

                ToSheet(sheet);
            }
        }

        /// <summary>
        /// Changes the output format between SVG and Bitmap
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The event arguments</param>
        private void SvgOrBitmap(object sender, EventArgs e)
        {
            if (_loaded && sender is ToolStripMenuItem clickedItem)
            {
                foreach (ToolStripMenuItem item in htmlResultToolStripMenuItem.DropDownItems)
                {
                    if (item != clickedItem && item.Checked)
                        item.Checked = false;
                }

                clickedItem.Checked = true;
                _svg = (clickedItem.Text == "SVG");
                _result = clickedItem.Text;
                ToSheet(_sheet);
            }
        }

        /// <summary>
        /// Opens a file
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The event arguments</param>
        public async void OpenFile(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "Excel XLSX Files (*.xlsx)|*.xlsx";
            dlg.Filter += "|Excel Macro Files (*.xlsm)|*.xlsm";
            dlg.Filter += "|Excel XLS Files (*.xls)|*.xls";
            dlg.Filter += "|Comma Separated Values Files (*.csv)|*.csv";
            dlg.ShowDialog();
            var fileName = dlg.FileName.Replace('\\', '/');
            _svgFileName = LoadBook(fileName);
            await webBrowser1.EnsureCoreWebView2Async();
            webBrowser1.CoreWebView2.Navigate($"file:///{_svgFileName}");

        }

        /// <summary>
        /// Loads a workbook from a file
        /// </summary>
        /// <param name="path">The path of the workbook file</param>
        /// <returns>The path of the SVG result</returns>
        private string LoadBook(string path)
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
                    sheetsMenu.Text = name;
                    sheetsMenu.DropDownItems.Clear();
                    // HTML result
                    foreach (var tag in "SVG|PNG|JPEG|GIF".Split('|'))
                    {
                        var mi = new ToolStripMenuItem();
                        mi.Text = tag;
                        mi.Click += (s, e) => SvgOrBitmap(s, e);
                        mi.Checked = (tag == "SVG");
                        htmlResultToolStripMenuItem.DropDownItems.Add(mi);
                    }

                    // add sheet menu
                    var count = 0;
                    foreach (XLSheet sheet in book.Sheets)
                    {
                        var mi = new ToolStripMenuItem();
                        mi.Text = sheet.Name;
                        mi.Tag = sheet;
                        mi.Click += (s, e) => ChangeSheet(s, e);
                        if (sheetsMenu.DropDownItems.Count == 0)
                        {
                            mi.Checked = true;
                            _sheet = sheet;
                        }
                        sheetsMenu.DropDownItems.Add(mi);
                        count++;
                    }
                    break;
            }
            return GetSvgResult();
        }

        /// <summary>
        /// Prints the sheet to SVG format
        /// </summary>
        /// <param name="sheet">The Excle workbook to print</param>
        /// <returns>The path of the SVG result</returns>
        public string PrintToSvg(XLSheet sheet)
        {
            // Paper size
            var paper = new SizeF(595.3f, 841.9f); // A4 by default
            if (sheet.PrintSettings != null)
            {
                paper = sheet.PrintSettings.PaperSize;
                if (sheet.PrintSettings.Landscape)
                {
                    paper = new SizeF(paper.Height, paper.Width);
                }
            }

            // Convert to pixels
            int wpx = C1XLBook.TwipsToPixels(20 * (int)paper.Width);
            int hpx = C1XLBook.TwipsToPixels(20 * (int)paper.Height);

            // Render sheet
            var r = new Rectangle(0, 0, wpx, hpx);
            GraphicsRendering gr = _svg
                ? new SvgGraphicsRendering(wpx, hpx)
                : new BitmapGraphicsRendering(wpx, hpx);
            var sr = new XLSheetRendering(sheet, gr);

            // Output file
            var fileName = $"{sheetsMenu?.Name}_{sheet.Name}";
            fileName = fileName.Replace('.', '_');
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = System.IO.Path.Combine(dir ?? string.Empty, "Results", $"{fileName}.html");

            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = Encoding.UTF8;
                settings.Indent = true;
                var doc = XmlWriter.Create(fs, settings);

                // Create new XHTML document with body and head tags
                doc.WriteStartElement("html");
                doc.WriteStartElement("head");

                // Add title from file name
                var title = sheet.Name;

                // To avoid unclosed title tag
                doc.WriteElementString("title", string.IsNullOrEmpty(title) ? " " : title);

                // Add UTF-8 encoding
                doc.WriteStartElement("meta");
                doc.WriteAttributeString("charset", "utf-8");
                doc.WriteEndElement();

                // Styles
                doc.WriteStartElement("style");
                doc.WriteAttributeString("type", "text/css");
                doc.WriteString(GetResourceText("Result.css")); // Make sure you have this method implemented
                doc.WriteEndElement();

                // Close head
                doc.WriteEndElement();

                // Add body
                doc.WriteStartElement("body");

                // Add content element
                doc.WriteStartElement("div");
                doc.WriteAttributeString("id", "content");

                // Sheet layout
                sr.Layout(paper.Width, paper.Height);

                // Render sheet
                int count = 0;
                var rc = new Rectangle(0, 0, (int)paper.Width, (int)paper.Height);
                for (var start = 0F; ;)
                {
                    // Clear bitmaps
                    if (!_svg)
                    {
                        gr.SetFill(Color.White);
                        gr.DrawRect(r.X, r.Y, r.Width, r.Height);
                    }

                    // One page rendering
                    start = sr.Render(rc, start);

                    // Section is created for page content
                    doc.WriteStartElement("section");

                    // Set page ID
                    doc.WriteAttributeString("id", $"page{1 + count}");

                    // Make page-like view
                    doc.WriteAttributeString("style", $"width:{wpx}px;height:{hpx}px;");

                    // Save image
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
                            doc.WriteAttributeString("src", $"data:image/{result};base64,{Convert.ToBase64String(ms.ToArray())}");
                        }
                        doc.WriteAttributeString("alt", $"Page {1 + count}");
                        doc.WriteEndElement();
                    }

                    // Close tags
                    doc.WriteEndElement(); // Section

                    // Done?
                    if (start == float.MaxValue)
                    {
                        break;
                    }

                    // Page counter
                    count++;
                }

                doc.WriteEndElement(); // Div#content

                doc.WriteEndElement(); // Body
                doc.WriteEndElement(); // Html

                // Save HTML document
                doc.Flush();
            }

            // Disposing
            gr.Dispose();

            // Done
            return path;
        }

        /// <summary>
        /// Saves the sheet as a file
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The event arguments</param>
        public void SaveAsFile(object sender, EventArgs e)
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
            dlg.ShowDialog();
            var fileName = dlg.FileName.Replace('\\', '/');
            var ext = System.IO.Path.GetExtension(fileName).Trim();
            if (ext.Equals(".csv"))
                _sheet.SaveCsv(dlg.FileName);
            else
                _sheet.Book.Save(dlg.FileName);
            Process.Start(new ProcessStartInfo { FileName = fileName, UseShellExecute = true });
        }

        /// <summary>
        /// Event handler for content loading in the web view
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The event arguments</param>
        private void webView_ContentLoading(object sender, Microsoft.Web.WebView2.Core.CoreWebView2ContentLoadingEventArgs e)
        {
            if (!_loaded)
            {
                _loaded = true;
                ToHome();
            }
        }

        /// <summary>
        /// Retrieves text from an embedded resource
        /// </summary>
        /// <param name="resource">The name of the resource</param>
        /// <returns>The text content of the resource</returns>
        private static string GetResourceText(string resource)
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
