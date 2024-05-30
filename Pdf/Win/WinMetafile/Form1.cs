// Copyright (c) 2023 FIIT B.V. | DeveloperTools (KVK:75050250). All Rights Reserved.

using GrapeCity.Documents.Imaging;
using GrapeCity.Documents.Svg;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

using _PaperKind = GrapeCity.Documents.Common.PaperKind;
using _FontStyle = System.Drawing.FontStyle;
using _Font = System.Drawing.Font;

using C1.Win.Pdf;

namespace ExcelViewerWin
{
    /// <summary>
    /// Interaction logic for Form1.Designer.cs
    /// </summary>
    public partial class Form1 : Form
    {
        private C1PdfDocument _c1pdf = new C1PdfDocument();

        /// <summary>
        /// Constructor for Form1
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        //=============================================================================
        // populate list with emf files in the executable directory
        private void Form1_Load(object sender, System.EventArgs e)
        {
            string path = Dir;
            foreach (string fileName in Directory.GetFiles(path, "*.emf"))
            {
                listBox1.Items.Add(Path.GetFileName(fileName));
            }
            foreach (string fileName in Directory.GetFiles(path, "*.wmf"))
            {
                listBox1.Items.Add(Path.GetFileName(fileName));
            }
            Text = "Windows metafies";
        }
        private string Dir
        {
            get
            {
                var dir = Path.GetDirectoryName(Application.ExecutablePath);
                return (dir != null) ? Path.Combine(dir, "Samples") : "Samples";
            }
        }

        //=============================================================================
        // create a document with lots of metafiles in it
        private void button1_Click(object sender, System.EventArgs e)
        {
            DumpMetas("*.emf");
        }
        private void button4_Click(object sender, System.EventArgs e)
        {
            if (listBox1 != null && listBox1.SelectedItem != null)
                DumpMetas(listBox1.SelectedItem.ToString());
        }
        private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string path = Dir;
            //string path = Path.GetDirectoryName(Application.ExecutablePath);
            pictureBox1.Image = Metafile.FromFile(path + "\\" + listBox1.SelectedItem.ToString());
        }

        string tempdir = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\") + 1);

        private void DumpMetas(string mask)
        {
            Cursor = Cursors.WaitCursor;

            _c1pdf.Clear();
            //_c1pdf.UniqueFontsPerPage = true;
            _c1pdf.FontType = C1.Pdf.PdfFontType.Embedded;
            _c1pdf.Compression = C1.Pdf.CompressionLevel.NoCompression;
            //_c1pdf.UseFontShaping = false;
            _c1pdf.ParseEmfPlus = checkBox1.Checked;
            _c1pdf.Compression = C1.Pdf.CompressionLevel.BestCompression;
            var rnd = new Random();
            var font = new _Font("Courier New", 9, _FontStyle.Bold);

            // look for emf files in the executable directory
            //string path = Path.GetDirectoryName(Application.ExecutablePath);
            var path = Dir;
            var first = true;
            var files = Directory.GetFiles(path, mask);
            foreach (string fileName in files)
            {
                Text = string.Format("Exporting {0}...", Path.GetFileName(fileName));
                Application.DoEvents();

                // new page
                if (!first)
                {
                    _c1pdf.NewPage();
                }
                first = false;

                // load metafile
                var meta = (Metafile)Metafile.FromFile(fileName);

                // get metafile size in points
                var szPage = GetImageSizeInPoints(meta);
                Console.WriteLine("Adding page {0:f2}\" x {1:f2}\"", szPage.Width / 72f, szPage.Height / 72f);

                // size page to metafile
                _c1pdf.PageSize = szPage;

                // draw metafile on the page
                var rc = _c1pdf.PageRectangle;
                //_c1pdf.FillRectangle(Brushes.AntiqueWhite, rc);
                try
                {
                    // safety draw metafile
                    _c1pdf.DrawImage(meta, rc);
                }
                catch { }
                //_c1pdf.DrawString(fileName, font, Brushes.Black, rc);

                // draw thumbnail at random place
                //rc.Width  /= 5;
                //rc.Height /= 5;
                //rc.X = rnd.Next((int)(szPage.Width  - rc.Width));
                //rc.Y = rnd.Next((int)(szPage.Height - rc.Height));
                //_c1pdf.FillRectangle(Brushes.White, rc);
                //_c1pdf.DrawImage(meta, rc);
                //_c1pdf.DrawRectangle(Pens.Black, rc);

                // add outline entry if there's more than one page
                if (files.Length > 1)
                    _c1pdf.AddBookmark(Path.GetFileName(fileName), 0, 0);
            }

            // show the result
            Text = "Saving...";
            SaveAndShow(tempdir + "metas.pdf");
            Cursor = Cursors.Default;
            Text = "Ready";
        }

        //=============================================================================
        // create a document with some rtf in it
        private void button2_Click(object sender, System.EventArgs e)
        {
            _c1pdf.Clear();
            _c1pdf.PaperKind = _PaperKind.Letter;
            _c1pdf.Compression = C1.Pdf.CompressionLevel.NoCompression;

            // load rtf
            var sr = new StreamReader(Application.StartupPath + @"\data.rtf");
            var rtfText = sr.ReadToEnd();
            sr.Close();

            // render rtf onto the page as usual text
            var rc = _c1pdf.PageRectangle;
            rc.Inflate(-72, -72);
            _c1pdf.DrawStringRtf(rtfText, null, null, rc);
            _c1pdf.DrawRectangle(Pens.BlueViolet, rc);

            // render again on a new page
            _c1pdf.NewPage();
            rc = _c1pdf.PageRectangle;
            rc.X += 50; rc.Width -= 180;
            rc.Y += 50; rc.Height -= 180;

            // this time measure the text first and adjust the height of the
            // rectangle to fit exactly.
            rc.Size = _c1pdf.MeasureStringRtf(rtfText, null, rc.Width);
            _c1pdf.DrawStringRtf(rtfText, null, null, rc);
            _c1pdf.DrawRectangle(Pens.BlueViolet, rc);

            // done
            SaveAndShow(tempdir + "rtf.pdf");
        }

        //=============================================================================
        // test arcs and pies
        private void button3_Click(object sender, System.EventArgs e)
        {
            _c1pdf.Clear();
            _c1pdf.Compression = C1.Pdf.CompressionLevel.NoCompression;

            int penWidth = 0;
            int penRGB = 0;
            Rectangle rc = new Rectangle(0, 0, 300, 200);

            string text = "Hello world of .NET Graphics and PDF.\r\nNice to meet you.";
            _Font font = new _Font("Times New Roman", 12, _FontStyle.Italic | _FontStyle.Underline);

            // start, c1, c2, end1, c3, c4, end
            PointF[] bezierPoints = new PointF[]
            {
                new PointF(10f, 100f), new PointF(20f, 10f),  new PointF(35f, 50f),
                new PointF(50f, 100f), new PointF(60f, 150f), new PointF(65f, 100f),
                new PointF(50f, 50f)
            };

            // draw to .NET Graphics object
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.FillRectangle(Brushes.White, 0, 0, bmp.Width, bmp.Height);
                g.FillPie(Brushes.Red, rc, 0, 20f);
                g.FillPie(Brushes.Green, rc, 20f, 30f);
                g.FillPie(Brushes.Blue, rc, 60f, 12f);
                g.FillPie(Brushes.Gold, rc, -80f, -20f);
                for (float sa = 0; sa < 360; sa += 40)
                {
                    Color penColor = Color.FromArgb(penRGB, penRGB, penRGB);
                    Pen pen = new Pen(penColor, penWidth++);
                    penRGB = penRGB + 20;
                    g.DrawArc(pen, rc, sa, 40f);
                }
                g.DrawRectangle(Pens.Red, rc);
                g.DrawBeziers(Pens.Blue, bezierPoints);
                g.DrawString(text, font, Brushes.Black, rc);
                g.Dispose();
            }
            this.pictureBox1.Image = bmp;
            this.pictureBox1.Refresh();

            // draw to pdf document
            penWidth = 0;
            penRGB = 0;
            if (true)
            {
                C1PdfDocument g = _c1pdf;
                g.FillPie(Brushes.Red, rc, 0, 20f);
                g.FillPie(Brushes.Green, rc, 20f, 30f);
                g.FillPie(Brushes.Blue, rc, 60f, 12f);
                g.FillPie(Brushes.Gold, rc, -80f, -20f);
                for (float sa = 0; sa < 360; sa += 40)
                {
                    Color penColor = Color.FromArgb(penRGB, penRGB, penRGB);
                    Pen pen = new Pen(penColor, penWidth++);
                    penRGB = penRGB + 20;
                    g.DrawArc(pen, rc, sa, 40f);
                }
                g.DrawRectangle(Pens.Red, rc);
                g.DrawBeziers(Pens.Blue, bezierPoints);
                g.DrawString(text, font, Brushes.Black, rc);
            }

            // show pdf document
            SaveAndShow(tempdir + "arcpie.pdf");
        }

        // save current document and show it in Adobe Acrobat
        private void SaveAndShow(string fileName)
        {
            try
            {
                _c1pdf.Save(fileName);
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

        internal static SizeF GetImageSizeInPoints(Image img)
        {
            SizeF sz = SizeF.Empty;

            // PhysicalDimension returns himetric for Metafiles,
            // pixels for all other image types
            if (img is Metafile mf)
            {
                // always use 'logical' resolution of 96 dpi for display metafiles
                if (mf.GetMetafileHeader().IsDisplay())
                {
                    sz.Width = (float)Math.Round(img.Width * 72f / 96f, 2);
                    sz.Height = (float)Math.Round(img.Height * 72f / 96f, 2);
                    return sz;
                }

                // other metafiles have PhysicalDimension stored in HiMetric
                sz = mf.PhysicalDimension;
                sz.Width = (float)Math.Round(sz.Width * 72f / 2540f, 2);
                sz.Height = (float)Math.Round(sz.Height * 72f / 2540f, 2);
                return sz;
            }

            // other images have the resolution stored in them
            sz.Height = (float)Math.Round(sz.Height * 72f / img.VerticalResolution, 2);
            sz.Width = (float)Math.Round(sz.Width * 72f / img.HorizontalResolution, 2);
            return sz;
        }
    }
}
