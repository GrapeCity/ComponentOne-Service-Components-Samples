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
using C1.Util;

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

using static System.Net.Mime.MediaTypeNames;
using GrapeCity.Documents.Drawing;

namespace Security
{
    class Program
    {
		private C1PdfDocument _c1pdf;

        Program()
        {
            // initialization
            _c1pdf = new C1PdfDocument();
#if DEBUGx
            _c1pdf.Compression = _CompressionLevel.NoCompression;
#else
            _c1pdf.ConformanceLevel = PdfAConformanceLevel.PdfA2a;
#endif
            // name of producer
            _c1pdf.DocumentInfo.Producer = "GrapeCity C1.Pdf";

            // security defaults
            _c1pdf.Security.AllowCopyContent = true;
            _c1pdf.Security.AllowEditAnnotations = true;
            _c1pdf.Security.AllowEditContent = true;
            _c1pdf.Security.AllowPrint = true;
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
            new Program().Tests(preview);
            Console.WriteLine("PDF test files created.");
        }

        void Tests(bool preview = false)
        {
            var owner = "Owner";
            var user = "User";
            Save(Test(owner, user), preview);
            _c1pdf.Dispose();
        }

        private string Test(string owner, string user)
        {
            Console.WriteLine($"Please Using owner password: \"{owner}\" or user password: \"{user}\" in a result");
            _c1pdf.Security.UserPassword = user;
            _c1pdf.Security.OwnerPassword = owner;

            _c1pdf.DocumentInfo.Title = "Security sample";

            _Rect rc = _c1pdf.PageRectangle;
            rc.Inflate(-96, -96);
            rc.Offset(-24, -24);
            _c1pdf.DrawRectangle(new _Pen(_Color.DarkGreen), rc, new _Size(72, 72));
            rc.Inflate(-72, -72);

            _Font font = new("Tahoma", 12);
            string text = string.Format("Owner password is '{0}'\r\nUser password is '{1}'", owner, user);
            _c1pdf.DrawString(text, font, _Color.Black, rc);

            AddFooters();

            return "security";
        }

        //================================================================================
        // add page footers to a document
        //
        // this method is called by all samples in this project. it scans the document
        // and adds a 'page n of m' footer to each page. the footers are rendered as 
        // vertical text along the right edge of the document.
        //
        // adding content to an existing page is easy: just set the CurrentPage property
        // to point to an existing page and write into it as usual.
        //
        private void AddFooters()
        {
            _Font fontHorz = new("Tahoma", 7, _FontStyle.Bold);
            _Font fontVert = new("Viner Hand ITC", 14, _FontStyle.Bold);

            StringFormat sfRight = new()
            {
                Alignment = HorizontalAlignment.Right
            };

            StringFormat sfVert = new StringFormat();
            sfVert.FormatFlags |= StringFormatFlags.DirectionVertical;
            sfVert.Alignment = HorizontalAlignment.Center;

            for (int page = 0; page < _c1pdf.Pages.Count; page++)
            {
                // select page we want (could change PageSize)
                _c1pdf.CurrentPage = page;

                // build rectangles for rendering text
                _Rect rcPage = GetPageRect();
                _Rect rcFooter = rcPage;
                rcFooter.Y = rcFooter.Bottom + 6;
                rcFooter.Height = 12;
                _Rect rcVert = rcPage;
                rcVert.X = rcPage.Right + 6;

                // add left-aligned footer
                string text = _c1pdf.DocumentInfo.Title;
                _c1pdf.DrawString(text, fontHorz, _Color.Gray, rcFooter);

                // add right-aligned footer
                text = string.Format("Page {0} of {1}", page + 1, _c1pdf.Pages.Count);
                _c1pdf.DrawString(text, fontHorz, _Color.Gray, rcFooter, sfRight);

                // add vertical text
                text = _c1pdf.DocumentInfo.Title + " (document created using the C1Pdf .NET component)";
                _c1pdf.DrawString(text, fontVert, _Color.LightGray, rcVert, sfVert);

                // draw lines on bottom and right of the page
                _c1pdf.DrawLine(_Color.Gray, rcPage.Left, rcPage.Bottom, rcPage.Right, rcPage.Bottom);
                _c1pdf.DrawLine(_Color.Gray, rcPage.Right, rcPage.Top, rcPage.Right, rcPage.Bottom);
            }
        }

        // get the current page rectangle (depends on paper size)
        // and apply a 1" margin all around it.
        internal _Rect GetPageRect()
        {
            _Rect rcPage = _c1pdf.PageRectangle;
            rcPage.Inflate(-72, -72);
            return rcPage;
        }

        // save to file and show it if is need
        void Save(string name, bool preview = false)
        {
            try
            {
                // name
                if (!name.ToLower().EndsWith(".pdf"))
                {
                    name += ".pdf";
                }
                Console.WriteLine($"Saving {name} PDF document...");

                // save
                var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var path = Path.Combine(dir, "Results", name);
                _c1pdf.Save(path);
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
    }
}
