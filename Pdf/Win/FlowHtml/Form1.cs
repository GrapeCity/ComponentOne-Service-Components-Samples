using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;

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
using System.Diagnostics;

namespace FlowHtml
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private const string BROWSE_TEXT = "<Browse...>";
        string _html;

        private void Form1_Load(object sender, EventArgs e)
        {
            // populate file list
            _cmbFiles.Items.Clear();
            Assembly a = Assembly.GetExecutingAssembly();
            foreach (string res in a.GetManifestResourceNames())
            {
                if (res.ToLower().EndsWith(".htm"))
                {
                    _cmbFiles.Items.Add(res.Substring("FlowHtml.Resources.".Length));
                }
            }
            _cmbFiles.Items.Add(BROWSE_TEXT);

            // populate columns list
            _cmbColumns.Items.Clear();
            for (int i = 1; i < 6; i++)
            {
                _cmbColumns.Items.Add(i.ToString());
            }

            // populate format list
            _cmbExt.Items.Clear();
            _cmbExt.Items.Add(".docx");
            _cmbExt.Items.Add(".pdf");
            _cmbExt.Items.Add(".rtf");
            _cmbExt.Items.Add(".txt");

            // initialize selection
            _cmbFiles.SelectedIndex = 1;
            _cmbColumns.SelectedIndex = 0;
            _cmbExt.SelectedIndex = 1;
        }

        // show document in Rich Text Box when it's selected
        void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // select file/document
            if (_cmbFiles.Text == BROWSE_TEXT)
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.FileName = "*.htm";
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        using (StreamReader sr = new StreamReader(dlg.FileName))
                            _html = sr.ReadToEnd();
                    }
                }
            }
            else
            {
                Assembly a = Assembly.GetExecutingAssembly();
                foreach (string resName in a.GetManifestResourceNames())
                {
                    if (resName.EndsWith(_cmbFiles.Text))
                    {
                        using (StreamReader sr = new StreamReader(a.GetManifestResourceStream(resName)))
                            _html = sr.ReadToEnd();
                    }
                }
            }

            // show the document in browser
            webBrowser1.DocumentText = _html;
        }

        private _Rect[] _cols;
        private int _currentColumn;

        // create PDF document
        void button1_Click(object sender, EventArgs e)
        {
            if (_cmbExt.Text.Equals(".pdf"))
            {
#if DEBUG
                _c1pdf.Compression = CompressionLevel.NoCompression;
#endif
                RenderHtml(_c1pdf, _c1pdf.PageRectangle);
            }
            else
            {
                using (var c1word = new C1WordDocument())
                {
                    var rc = new _Rect(new _Point(-72, -72), c1word.PageSize);
                    RenderHtml(c1word, rc);
                }
            }
        }
 
        void RenderHtml(IFlowDocument flow, _Rect rcPage)
        {
            // get ready to work
            button1.Enabled = false;
            DateTime bs = DateTime.Now;

            // get number of columns, create layout array
            int cols = int.Parse(_cmbColumns.Text);
            _cols = new _Rect[cols];

            // 4 or more columns? switch to landscape
            if (cols >= 4) _btnLandscape.Checked = true;

            // apply document orientation
            flow.Landscape = _btnLandscape.Checked;

            // create one rectangle per column
            rcPage.Inflate(-50, -50);
            for (int i = 0; i < cols; i++)
            {
                _Rect rcc = rcPage;
                rcc.Width /= cols;
                rcc.Offset(rcc.Width * i, 0);
                rcc.Inflate(-10, -10);
                _cols[i] = rcc;
            }
            
            // get Html to render
            string text = _html;

            // print the HTML string spanning multiple pages
            flow.Clear();
            _currentColumn = 0;
            _Font font = new _Font("Times New Roman", 12);
            _Pen pen = new _Pen(_Color.LightCoral, 0.01f);
            for (var start = 0F; ; )
            {
                // render this part
                _status.Text = string.Format("Page {0} Column {1}", flow.PageCount, _currentColumn + 1);
                Application.DoEvents();
                _Rect rc = _cols[_currentColumn];
                start = flow.DrawStringHtml(text, font, _Color.Black, rc, start);
                flow.DrawRectangle(pen, rc);

                // done?
                if (start >= _Float.MaxValue)
                {
                    break;
                }

                // skip page/column
                _currentColumn++;
                if (_currentColumn >= _cols.Length)
                {
                    _currentColumn = 0;
                    flow.NewPage();
                }
            }

            // done
            _status.Text = "Ready";
            button1.Enabled = true;
            TimeSpan ts = DateTime.Now.Subtract(bs);
            Console.WriteLine("done in {0:f2}s", ts.TotalSeconds);

            // show the result
            string fn = Path.Combine(Application.StartupPath, $"html{_cmbExt.Text}");
            SaveAndShow(flow, fn);
        }

        // just for testing...
        void button2_Click(object sender, EventArgs e)
        {
            string resName = @"FlowHtml.HTMLPage1.htm";
            using (StreamReader sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(resName)))
            {
                _html = sr.ReadToEnd();
            }
            _c1pdf.Clear();
            _Rect rc = new _Rect(100, 100, 500, 0);
            rc.Height = 62;
            _Pen pen = new _Pen(_Color.Red);
            _Font font = new _Font("Arial", 24, _FontStyle.Bold);
            var offset = _c1pdf.DrawStringHtml(_html, font, _Color.Black, rc);
            _c1pdf.DrawRectangle(pen, rc);
            if (offset < int.MaxValue)
            {
                rc.Offset(0, rc.Height + 50);
                _c1pdf.DrawStringHtml(_html, font, _Color.Black, rc, offset);
                _c1pdf.DrawRectangle(pen, rc);
            }
            SaveAndShow(_c1pdf, @"c:\temp\foo.pdf");
        }

        // save current document and show it in Adobe Acrobat
        static void SaveAndShow(IFlowDocument flow, string fileName)
        {
            try
            {
                flow.Save(fileName);
                var psi = new ProcessStartInfo()
                {
                    FileName = fileName,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch
            {
                MessageBox.Show("Can't save, make sure the document is not open.");
            }
        }
    }
}