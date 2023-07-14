//#define RTF

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Reflection;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

using C1.Pdf;

using _Float = System.Single;
using _Size = System.Drawing.SizeF;
using _Point = System.Drawing.PointF;
using _Rect = System.Drawing.RectangleF;
using _Color = System.Drawing.Color;
using _CompressionLevel = C1.Pdf.CompressionLevel;
using _ImageQuality = C1.Pdf.ImageQuality;
using _ImageSizeMode = C1.Util.ImageSizeMode;
using _AttachmentIcon = C1.Pdf.AttachmentIcon;
using _PdfFontType = C1.Pdf.PdfFontType;
using _FillMode = GrapeCity.Documents.Drawing.FillMode;
using _DashStyle = GrapeCity.Documents.Drawing.DashStyle;
using _FontStyle = C1.Util.FontStyle;
using _Font = C1.Util.Font;
using _Pen = GrapeCity.Documents.Drawing.Pen;
using _PenLineCap = GrapeCity.Documents.Drawing.PenLineCap;
using _PenLineJoin = GrapeCity.Documents.Drawing.PenLineJoin;
using _Matrix = System.Numerics.Matrix3x2;
using _Bitmap = GrapeCity.Documents.Imaging.GcBitmap;
using _Image = GrapeCity.Documents.Drawing.Image;

using GrapeCity.Documents.Text;
using C1.Zip.ZLib;
using C1.Word;
using C1.Util;

namespace ManualSamples
{
    class Program
    {
        private Random _rnd;
        private IFlowDocument _doc;
        private C1PdfDocument _pdf;
        private C1WordDocument _word;

        Program()
        {
            // initialization
            _rnd = new Random();

            // C1Pdf
            _pdf = new C1PdfDocument();
#if DEBUGx
            _pdf.Compression = _CompressionLevel.NoCompression;
#endif
            // name and security
            _pdf.DocumentInfo.Producer = "C1.Pdf .NET Standard";
            _pdf.Security.AllowCopyContent = true;
            _pdf.Security.AllowEditAnnotations = true;
            _pdf.Security.AllowEditContent = true;
            _pdf.Security.AllowPrint = true;

            // C1 Word
            _word = new C1WordDocument();
            _word.Info.Author = "C1.Pdf .NET Standard";
            _word.Info.Company = "GrapeCity";

            // default flow document
            _doc = _pdf;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Create PDF samples...");
            bool preview = false;
            foreach (string arg in args)
            {
                if (arg.ToUpper().Equals("SHOW"))
                {
                    preview = true;
                    break;
                }
            }
            var program = new Program();
            program.Tests(preview);
            program._doc = program._word;
            program.Tests(preview);
            Console.WriteLine("PDF test files created.");
        }

        void Tests(bool preview = false)
        {
            Save(HelloWorld(), preview);
            Save(LongText(), preview);
#if RTF
            Save(RichText(), preview);
#endif
            Save(ImageSizeModes(), preview);
            Save(Graphics(), preview);
#if RTF
            Save(Dir(), preview);
#endif
            Save(Landscape(), preview);
            Save(Links(), preview);
            Save(Arc(), preview);
            Save(Bezier(), preview);
            Save(Beziers(), preview);
            Save(Images(), preview);
            Save(DrawLine(), preview);
            Save(DrawLines(), preview);
            Save(DrawPie(), preview);
            Save(Poly(), preview);
            Save(Rect(), preview);
            Save(String(), preview);
#if RTF
            Save(Rtf(), preview);
#endif
            Save(Measure(), preview);
            Save(Pages(), preview);
        }

        // get the current page rectangle (depends on paper size)
        // and apply a 1" margin all around it.
        internal _Rect GetPageRect()
        {
            _Rect rcPage = new _Rect(new _Point(0, 0), _doc.PageSize);
            if (_doc is C1PdfDocument)
            {
                rcPage.Inflate(-72, -72);
            }
            else if (_doc is C1WordDocument)
            {
                var sz = new _Size(rcPage.Size.Width - 144, rcPage.Size.Height - 144);
                rcPage = new _Rect(new _Point(0, 0), sz);
            }
            return rcPage;
        }

        private string HelloWorld()
        {
            // step 1: create the document object
            _doc.Clear();

            // step 2: add content to the page
            _Font font = new("Arial", 12);
            _Rect rc = GetPageRect();
            _doc.DrawString("Hello World!", font, _Color.Black, rc);

            // step 3: save the document to a file
            return "hello world.pdf";
        }

        private string LongText()
        {
            // step 1: create the document object
            _doc.Clear();

            // content to the page
            _Rect rcPage = GetPageRect();
            var x = rcPage.Left;
            var y = rcPage.Top;

            // step 2: add content to the page
            _Font font = new("Arial", 12);
            _Rect rc = new(x, y, 100, 50);
            var text = "Some long string to be rendered into a small rectangle. ";
            text = text + text + text + text + text + text;

            // center align and clip string
            StringFormat sf = new StringFormat();
            sf.Alignment = HorizontalAlignment.Center;
            sf.LineAlignment = VerticalAlignment.Center;

            _doc.DrawString(text, font, _Color.Black, rc, sf);
            _doc.DrawRectangle(new _Pen(_Color.Gray), rc);

            //using (Graphics g = this.CreateGraphics())
            //{
            //    g.PageUnit = GraphicsUnit.Point;
            //    g.DrawString(text, font, Brushes.Black, rc, sf);
            //    g.DrawRectangle(Pens.Gray, Rectangle.Truncate(rc));
            //}

            // step 3: save the document to a file
            return "long text.pdf";
        }
#if RTF
        private string RichText()
        {
            // step 1: create the document object
            _doc.Clear();

            // step 2: add content to the page
            _Font font = new("Arial", 12);
            _Rect rc = GetPageRect();
            _doc.DrawStringRtf(@"To {\b boldly} go where {\i no one} has gone before!", font, Brushes.Black, rc);

            // step 3: save the document to a file
            return "rich text.pdf";
        }
#endif
        private string ImageSizeModes()
        {
            // step 1: create the C1PdfDocument object
            _doc.Clear();

            // step 2: add content to the page
            _Rect rc = GetPageRect();

            // draw image
            _Image image;
            using (var stream = GetManifestResource("gravitation.jpg"))
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                image = _Image.FromBytes(ms.ToArray());
            }

            // stretch image to fill the rectangle
            _doc.DrawImage(image, rc);

            if (_doc is C1PdfDocument pdf)
            {
                // center image within the rectangle, scale keeping aspect ratio
                pdf.DrawImage(image, rc, ContentAlignment.MiddleCenter, _ImageSizeMode.Scale);

                // center image within the rectangle, keep original size
                pdf.DrawImage(image, rc, ContentAlignment.TopLeft, _ImageSizeMode.Clip);
            }

            // step 3: save the document to a file
            return "image size modes.pdf";
        }

        private string Graphics()
        {
            // create PDF document
            _doc.Clear();

            // content to the page
            _Rect rcPage = GetPageRect();
            var x = rcPage.Left;
            var y = rcPage.Top;

            // set up to draw
            _Rect rc = new(x, y, 300, 200);
            string text = "Hello world of .NET Graphics and pdf.\r\nNice to meet you.";
            _Font font = new("Times New Roman", 12, _FontStyle.Italic | _FontStyle.Underline);
            _Point[] bezierpts = new[]
            {
                new _Point(x + 10f, y + 100f), new _Point(x + 20f, y + 10f),  new _Point(x + 35f, y + 50f),
                new _Point(x + 50f, y + 100f), new _Point(x + 60f, y + 150f), new _Point(x + 65f, y + 100f),
                new _Point(x + 50f, y + 50f)
            };

            // draw to PDF document
            int penWidth = 0;
            int penRGB = 0;
            _doc.FillPie(_Color.Red, rc, 0, 20f);
            _doc.FillPie(_Color.Green, rc, 20f, 30f);
            _doc.FillPie(_Color.Blue, rc, 60f, 12f);
            _doc.FillPie(_Color.Gold, rc, -80f, -20f);
            for (float startAngle = 0; startAngle < 360; startAngle += 40)
            {
                _Color penColor = _Color.FromArgb(penRGB, penRGB, penRGB);
                _Pen pen = new(penColor, penWidth++);
                penRGB = penRGB + 20;
                _doc.DrawArc(pen, rc, startAngle, 40f);
            }
            _doc.DrawRectangle(new _Pen(_Color.Red), rc);
            _doc.DrawBeziers(new _Pen(_Color.Blue), bezierpts);
            _doc.DrawString(text, font, _Color.Black, rc);

            // show it
            return "graphics.pdf";
        }
#if RTF
        private string Dir()
        {
            // get rtf template
            string rtfHdr = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033" +
                @"{\fonttbl{\f0\fswiss\fcharset0 Arial;}{\f1\froman\fprq2\fcharset0 Book Antiqua;}}" +
                @"{\colortbl ;\red0\green0\blue0;}\viewkind4\uc1\pard\f0\fs20\par" +
                @"\pard\tx1440\tx2880\tx4320\tx5760\cf1\b\f1\fs24 Directory Report created on <<TODAY>>\par" +
                @"\ul\par Name\tab Extension\tab Size\tab Date\tab Attributes\par";
            string rtfEntry = @"\cf0\ulnone\b0\f0\fs16 <<NAME>>\tab <<EXT>>\tab <<SIZE>>\tab <<DATE>>\tab <<ATTS>>\par";

            // build rtf string
            StringBuilder sb = new StringBuilder();
            sb.Append(rtfHdr.Replace("<<TODAY>>", DateTime.Today.ToShortDateString()));
            foreach (string file in Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "*.bmp"))
            {
                string s = rtfEntry;
                FileInfo fi = new FileInfo(file);
                s = s.Replace("<<NAME>>", Path.GetFileNameWithoutExtension(file));
                s = s.Replace("<<EXT>>", fi.Extension);
                s = s.Replace("<<SIZE>>", string.Format("{0:#,##0}", fi.Length));
                s = s.Replace("<<DATE>>", fi.LastWriteTime.ToShortDateString());
                s = s.Replace("<<ATTS>>", fi.Attributes.ToString());
                sb.Append(s);
            }
            sb.Append('}');

            // render it
            _doc.Clear();
            _Rect rc = GetPageRect();
            _doc.DrawStringRtf(sb.ToString(), Font, Brushes.Black, rc);

            // save and show
            return "dir.pdf";
        }
#endif
        private string Landscape()
        {
            // initialize
            _doc.Clear();
            _Font font = new("Arial", 12);

            // create a 10 page document, make page 5 landscape
            for (int i = 0; i < 10; i++)
            {
                if (i > 0)
                {
                    _doc.NewPage();
                }
                _doc.Landscape = (i == 4);

                _Rect rc = GetPageRect();
                _doc.DrawString("Hello", font, _Color.Black, rc);
                _doc.DrawRectangle(new _Pen(_Color.Black), rc);
            }

            // save and show
            return "landscape.pdf";
        }

        private string Links()
        {
            // initialize
            _doc.Clear();

            // content to the page
            _Rect rcPage = GetPageRect();
            var x = rcPage.Left;
            var y = rcPage.Top;

            // create a regular (external) hyperlink 
            _Rect rc = new(x, y, 200, 15);
            _Font font = new("Arial", 10, _FontStyle.Underline);
            if (_doc is C1PdfDocument pdf)
            {
                pdf.AddLink("http://www.grapecity.com/componentone", rc);
            }
            else if (_doc is C1WordDocument word)
            {
                word.AddLink("http://www.grapecity.com/componentone");
            }
            _doc.DrawString("Visit GrapeCity ComponentOne", font, _Color.Blue, rc);

            // create a link target
            if (_doc is C1PdfDocument c1pdf)
            {
                c1pdf.AddTarget("#myLink", rc);
            }
            else if (_doc is C1WordDocument c1word)
            {
                c1word.AddBookmark("#myLink");
            }

            // add a few pages
            for (int i = 0; i < 5; i++)
            {
                _doc.NewPage();
            }

            // add a link to the target
            if (_doc is C1PdfDocument pdf1)
            {
                pdf1.AddLink("#myLink", rc);
            }
            else if (_doc is C1WordDocument word1)
            {
                word1.AddLink("#myLink");
            }
            _doc.FillRectangle(_Color.BlanchedAlmond, rc);
            _doc.DrawString("Local link: back to page 1...", font, _Color.Blue, rc);

            // save and show
            return "links.pdf";
        }

        private string Arc()
        {
            // initialize
            _doc.Clear();

            // content to the page
            _Rect rcPage = GetPageRect();
            var x = rcPage.Left;
            var y = rcPage.Top;
            _Rect rc = new(x, y, 200, 180);

            _doc.DrawEllipse(new _Pen(_Color.Gray), rc);
            _doc.DrawArc(new _Pen(_Color.Black, 4), rc, 0, 45);
            _doc.DrawArc(new _Pen(_Color.Red, 4), rc, 0, -45);

            // save and show
            return "arc.pdf";
        }

        private string Bezier()
        {
            // initialize
            _doc.Clear();

            // content to the page
            _Rect rcPage = GetPageRect();
            var x = rcPage.Left;
            var y = rcPage.Top;

            // create points
            _Point[] pts = new[]
            {
                new _Point(x + 100, y + 100), new _Point(x + 120,  y + 30),
                new _Point(x + 200, y + 140), new _Point(x + 230,  y + 20),
            };

            // draw Bezier spline
            _doc.DrawBeziers(new _Pen(_Color.Blue, 4), pts);

            // show points
            _doc.DrawLines(new _Pen(_Color.Gray), pts);
            foreach (_Point pt in pts)
            {
                _doc.DrawRectangle(new _Pen(_Color.Red), pt.X - 2, pt.Y - 2, 4, 4);
            }

            // save and show
            return "bezier.pdf";
        }

        private string Beziers()
        {
            // initialize
            _doc.Clear();

            // content to the page
            _Rect rcPage = GetPageRect();
            var x = rcPage.Left;
            var y = rcPage.Top;

            // create points
            _Point[] pts = new[]
            {
                new _Point(x + 50f, y + 100f), new _Point(x + 100f, y + 10f),  new _Point(x + 220f, y + 50f),
                new _Point(x + 360f, y + 100f), new _Point(x + 450f, y + 150f), new _Point(x + 490f, y + 250f),
                new _Point(x + 360f, y + 300f)
            };

            // draw Bezier spline
            _doc.DrawBeziers(new _Pen(_Color.Blue, 4), pts);

            // show points
            _doc.DrawLines(new _Pen(_Color.Gray), pts);
            for (int i = 0; i < pts.Length; i++)
            {
                _Color brush = (i % 3 == 0) ? _Color.Red : _Color.Green;
                _doc.FillRectangle(brush, pts[i].X - 2, pts[i].Y - 2, 4, 4);
            }

            // save document
            return "beziers.pdf";
        }

        private string Images()
        {
            // step 1: clear the document object
            _doc.Clear();

            // step 2: add content to the page
            _Rect rcPage = GetPageRect();
            var x = rcPage.Left;
            var y = rcPage.Top;
            _Rect rc = new(x, y, 150, 90);

            // draw image
            _Image img;
            using (var stream = GetManifestResource("gravitation.jpg"))
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                img = _Image.FromBytes(ms.ToArray());
            }

            // stretch image to fill the rectangle
            _doc.DrawImage(img, rc);
            _doc.DrawRectangle(new _Pen(_Color.Black), rc);

            // render in actual size, clipping if necessary
            rc.Offset(rc.Width + 20, 0);
            if (_doc is C1PdfDocument pdf)
            {
                pdf.DrawImage(img, rc, ContentAlignment.MiddleLeft, _ImageSizeMode.Clip);
            }
            else if (_doc is C1WordDocument)
            {
                _doc.DrawImage(img, rc, rc);
            }
            _doc.DrawRectangle(new _Pen(_Color.Black), rc);

            // scale the image to fit the rectangle while preserving the aspect ratio
            rc.Offset(rc.Width + 20, 0);
            if (_doc is C1PdfDocument c1pdf)
            {
                c1pdf.DrawImage(img, rc, ContentAlignment.MiddleLeft, _ImageSizeMode.Scale);
            }
            else if (_doc is C1WordDocument)
            {
                _doc.DrawImage(img, rc);
            }
            _doc.DrawRectangle(new _Pen(_Color.Black), rc);

            // step 3: save the document to a file
            return "images.pdf";
        }

        private string DrawLine()
        {
            // clear the document object
            _doc.Clear();

            // content to the page
            _Rect rcPage = GetPageRect();
            var x = rcPage.Left;
            var y = rcPage.Top;

            // create a thin (hairline) black pen
            _Pen thinPen = new(_Color.Black, 0);

            // create a thick (3pt) blue pen
            _Pen thickPen = new(_Color.Blue, 3);

            // create a thick (2pt) dotted red pen
            _Pen dotPen = new(_Color.Red, 2);
            dotPen.DashStyle = _DashStyle.Dot;

            // draw some lines
            _doc.DrawLine(thinPen, x + 100, y + 100, x + 300, y + 100);
            _doc.DrawLine(thickPen, x + 100, y + 120, x + 300, y + 120);
            _doc.DrawLine(dotPen, x + 100, y + 140, x + 300, y + 140);

            // save the document to a file
            return "drawline.pdf";
        }

        private string DrawLines()
        {
            // clear the document object
            _doc.Clear();

            // content to the page
            _Rect rcPage = GetPageRect();
            var x = rcPage.Left;
            var y = rcPage.Top;

            // create some points
            var points = new _Point[20];
            Random rnd = new();
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new _Point(x + rnd.Next(100, 500), y + rnd.Next(100, 200));
            }

            // draw lines
            _doc.DrawLines(new _Pen(_Color.Black), points);

            // show points
            foreach (_Point pt in points)
            {
                _doc.DrawRectangle(new _Pen(_Color.Red), pt.X - 3, pt.Y - 3, 6, 6);
            }

            // save the document to a file
            return "drawlines.pdf";
        }

        private string DrawPie()
        {
            // clear the document object
            _doc.Clear();

            // content to the page
            _Rect rcPage = GetPageRect();
            var x = rcPage.Left;
            var y = rcPage.Top;

            // create an array with some brushes
            var brushes = new _Color[]
            {
                _Color.Red, _Color.Green, _Color.Blue,
                _Color.Yellow, _Color.Crimson, _Color.Aquamarine
            };

            // setup rectangle and initialize angles
            _Rect rc = new(x, y, 180, 150);
            float startAngle = 0;
            float sweepAngle = -90; // << counter-clockwise

            // draw pie
            _Pen pen = new(_Color.Black);
            foreach (_Color brush in brushes)
            {
                _doc.FillPie(brush, rc, startAngle, sweepAngle);
                _doc.DrawPie(pen, rc, startAngle, sweepAngle);
                startAngle += sweepAngle;
                sweepAngle /= 2;
            }

            // save the document to a file
            return "drawpie.pdf";
        }

        private string Poly()
        {
            // clear the document object
            _doc.Clear();

            // content to the page
            _Rect rcPage = GetPageRect();
            var x = rcPage.Left;
            var y = rcPage.Top;

            // create an array with some points
            _Point[] points = new[]
            {
                new _Point(x + 100, y + 100), new _Point(x + 300, y + 100), new _Point(x + 200, y + 200)
            };

            // fill and draw a polygon
            _doc.FillPolygon(_Color.Beige, points);
            _doc.DrawPolygon(new _Pen(_Color.Red), points);

            // save the document to a file
            return "poly.pdf";
        }

        private string Rect()
        {
            // clear the document object
            _doc.Clear();

            // content to the page
            _Rect rcPage = GetPageRect();
            var x = rcPage.Left;
            var y = rcPage.Top;

            // create a rectangle
            _Rect rc = new(x, y, 200, 150);

            // draw and fill rectangles with round corners
            for (int corner = 10; corner < 100; corner += 20)
            {
                SizeF sz = new SizeF(corner, corner / 2);
                _doc.FillRectangle(_Color.Beige, rc, sz);
                _doc.DrawRectangle(new _Pen(_Color.Blue), rc, sz);
            }

            // draw a regular rectangle
            _doc.DrawRectangle(new _Pen(_Color.Red), rc);

            // save the document to a file
            return "rect.pdf";
        }

        private string String()
        {
            // clear the document object
            _doc.Clear();

            // content to the page
            _Rect rcPage = GetPageRect();
            var x = rcPage.Left;
            var y = rcPage.Top;

            // set up to draw
            _Font font = new("Tahoma", 14);
            _Rect rc = new(x, y, 150, 28);

            // draw string using default options (left-top alignment, no clipping)
            string text = "This string is being rendered using the default options.";
            _doc.DrawString(text, font, _Color.Black, rc);
            _doc.DrawRectangle(new _Pen(_Color.Black), rc);

            // create StringFormat to center align and clip
            StringFormat sf = new StringFormat();
            sf.Alignment = HorizontalAlignment.Center;
            sf.LineAlignment = VerticalAlignment.Center;
            sf.FormatFlags &= ~StringFormatFlags.NoClip;

            // render again using custom options
            rc.Offset(0, rc.Height + 30);
            text = "This string is being rendered using custom options.";
            _doc.DrawString(text, font, _Color.Black, rc, sf);
            _doc.DrawRectangle(new _Pen(_Color.Black), rc);

            // save the document to a file
            return "string.pdf";
        }
#if RTF
        private string Rtf()
        {
            // clear the document object
            _doc.Clear();

            // set up to draw
            _Font font = new("Tahoma", 14);
            _Rect rc = new(100, 100, 300, 28);

            // measure RTF text and adjust the rectangle to fit
            string text = @"Short {\b RTF} snippet with some {\b bold} and some {\i italics} in it.";
            rc.Y = rc.Bottom + 12;
            rc.Height = _doc.MeasureStringRtf(text, font, rc.Width).Height;

            // render RTF snippet
            _doc.DrawStringRtf(text, font, _Color.Blue, rc);
            _doc.DrawRectangle(new _Pen(_Color.Black), rc);

            // save the document to a file
            return "rtf.pdf";
        }
#endif
        private string Measure()
        {
            // clear the document object
            _doc.Clear();

            // content to the page
            _Rect rcPage = GetPageRect();
            var x = rcPage.Left;
            var y = rcPage.Top;

            // set up to draw
            var text = "We all came down to Montreux, by the Lake Geneva shoreline.";
            _Font font = new("Tahoma", 12);
            _Rect rc = new(x, y, 0, 0);

            // measure text on a single line
            rc.Size = _doc.MeasureString(text, font);
            _doc.DrawString(text, font, _Color.Black, rc);
            _doc.DrawRectangle(new _Pen(_Color.LightGray), rc);

            // update rectangle for next sample
            rc.Y = rc.Bottom + 12;
            rc.Width = 120;

            // measure text that wraps
            rc.Size = _doc.MeasureString(text, font, rc.Width);
            _doc.DrawString(text, font, _Color.Black, rc);
            _doc.DrawRectangle(new _Pen(_Color.LightGray), rc);

            // save the document to a file
            return "measure.pdf";
        }

        private string Pages()
        {
            // clear the document object
            _doc.Clear();

            // set up to draw
            _Font font = new("Tahoma", 12);
            _Rect rc = GetPageRect();

            // create document with 5 numbered pages
            for (int i = 0; i < 5; i++)
            {
                if (i > 0)
                {
                    _doc.NewPage();
                }
                _doc.DrawString("Page " + i.ToString(), font, _Color.Black, rc);
                _doc.DrawRectangle(new _Pen(_Color.LightGray), rc);
            }

            // Deprecated: move the last page to the front of the document
            if (_doc is C1PdfDocument pdf)
            {
                PdfPage last = pdf.Pages[pdf.Pages.Count - 1];
                pdf.Pages.Remove(last);
                pdf.Pages.Insert(0, last);
            }

            // save the document to a file
            return "pages.pdf";
        }

        // save to file and show it if is need
        void Save(string name, bool preview = false)
        {
            try
            {
                // name
                if (_doc is C1PdfDocument)
                {
                    if (!name.ToLower().EndsWith(".pdf"))
                    {
                        name += ".pdf";
                    }
                }
                else if (_doc is C1WordDocument)
                {
                    name = name.Replace(".pdf", ".docx");
                    if (!name.ToLower().EndsWith(".docx"))
                    {
                        name += ".docx";
                    }
                }
                Console.WriteLine($"Saving {name} document...");

                // save
                var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var path = Path.Combine(dir, "Results", name);
                if (_doc is C1PdfDocument pdf)
                {
                    pdf.Save(path);
                }
                else if (_doc is C1WordDocument word)
                {
                    word.Save(path);
                    word.Save(path.Replace(".docx", ".rtf"));
                }
                Console.WriteLine($"Saved: {path}");

                // show
                if (preview)
                {
                    Console.WriteLine($"Showing {name} PDF document...");
                    Process.Start(new ProcessStartInfo { FileName = path, UseShellExecute = true });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
        }

        public static Stream GetManifestResource(string resource)
        {
            resource = resource.ToLower();
            Assembly a = Assembly.GetExecutingAssembly();
            foreach (string res in a.GetManifestResourceNames())
            {
                if (res.ToLower().EndsWith(resource))
                {
                    return a.GetManifestResourceStream(res);
                }
            }
            return null;
        }
    }
}
