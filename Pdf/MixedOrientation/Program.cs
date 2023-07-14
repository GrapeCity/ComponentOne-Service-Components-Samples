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
using _PaperKind = GrapeCity.Documents.Common.PaperKind;
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
using static System.Net.Mime.MediaTypeNames;
using C1.Util;
using C1.Word;

namespace MixedOrientation
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
#else
            _pdf.Compression = _CompressionLevel.BestSpeed;
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
            Save(Test(), preview);
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

        private string Test()
        {
            // start a new document
            _doc.Clear();
            _doc.PaperKind = _PaperKind.Letter;
            int cnt = 0;
            var a = Assembly.GetExecutingAssembly();

            // add portrait pages
            _doc.Landscape = false;
            foreach (string res in a.GetManifestResourceNames())
            {
                if (res.EndsWith(".jpg"))
                {
                    if (cnt > 0)
                    {
                        _doc.NewPage();
                    }
                    _Image img = _Image.FromStream(a.GetManifestResourceStream(res));
                    _doc.DrawImage(img, GetPageRect());
                    cnt++;
                }
            }

            // add landscape pages
            cnt = 0;
            _doc.NewPage();
            _doc.Landscape = true;
            foreach (string res in a.GetManifestResourceNames())
            {
                if (res.EndsWith(".jpg"))
                {
                    if (cnt > 0)
                    {
                        _doc.NewPage();
                    }
                    _Image img = _Image.FromStream(a.GetManifestResourceStream(res));
                    _doc.DrawImage(img, GetPageRect());
                    cnt++;
                }
            }

            // save the document
            return "mixed.pdf";
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
                    Console.WriteLine($"Showing {name} document...");
                    Process.Start(new ProcessStartInfo { FileName = path, UseShellExecute = true });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
        }
    }
}
