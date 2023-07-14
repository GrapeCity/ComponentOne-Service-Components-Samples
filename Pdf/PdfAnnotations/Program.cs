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
using GrapeCity.Documents.Text;

namespace PdfAnnotations
{
    class Program
    {
		private C1PdfDocument _c1pdf;

        Program()
        {
            // initialization
            _c1pdf = new C1PdfDocument();
#if DEBUG
            _c1pdf.Compression = _CompressionLevel.NoCompression;
#else
            _c1pdf.Compression = _CompressionLevel.BestSpeed;
#endif
            // name of producer
            _c1pdf.DocumentInfo.Producer = "ComponentOne C1.Pdf";

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
            Save(Test(_PdfFontType.Embedded), preview);
        }

        private string Test(_PdfFontType fontType)
        {
            // create PDF document
            _c1pdf.Clear();
            _c1pdf.FontType = fontType;
            _c1pdf.DocumentInfo.Title = "PDF Annotations";
            Console.WriteLine($"Creating {_c1pdf.DocumentInfo.Title} document...");

            // calculate page rectangle (discounting margins)
            _Rect rcPage = GetPageRect();
            _Rect rc = rcPage;

            // add title
            _Font titleFont = new("Times New Roman", 22, _FontStyle.Italic | _FontStyle.Bold);

            RenderParagraph(_c1pdf.DocumentInfo.Title, titleFont, rcPage, rc);
            rc = rcPage;

            //-----------------------------------------------
            // annotations
            _Rect squareRc = new(rc.X, rc.Y + rc.Height / 10, rc.Width / 3, rc.Height / 30);

            // rectangle note
            PdfSquareNote squareNote = new()
            {
                Contents = "Japanese text",
                FillColor = Color.Yellow,
                Thickness = 2,
                Opacity = 55f,
                PopupOpen = true
            };

            // add
            _c1pdf.AddNote(squareNote, squareRc);

            squareRc = new(rc.X, rc.Y + rc.Height / 5, rc.Width / 3, rc.Height / 10);

            // circle note
            PdfCircleNote circleNote = new()
            {
                Contents = "Next text",
                FillColor = Color.Teal,
                Opacity = 25.78f
            };

            // add
            _c1pdf.AddNote(circleNote, squareRc);

            // line note
            PdfLineNote lineNote = new(new _Point(rc.X, rc.Y + rc.Height / 3), new _Point(rc.X + rc.Width / 3, rc.Y + rc.Height / 3))
            {
                Contents = "Line text",
                Thickness = 3
            };

            // add
            _c1pdf.AddNote(lineNote, lineNote.Rectangle);

            // text note
            _Rect textRc = new(rc.X, rc.Y + rc.Height / 2, rc.Width / 3, rc.Height / 30);

            // circle note
            PdfTextNote textNote = new()
            {
                Contents = "Simple text",
                Opacity = 44
            };

            // add
            _c1pdf.AddNote(textNote, textRc);

            // next page
            _c1pdf.NewPage();

            // line note
            lineNote = new(new PointF(rc.X, rc.Y + rc.Height / 10), new PointF(rc.X + rc.Width / 2, rc.Y + rc.Height / 7))
            {
                Contents = "Line annotation",
                Opacity = 33,
                Thickness = 7,
                PopupOpen = true
            };

            // add
            _c1pdf.AddNote(lineNote, lineNote.Rectangle);

            // text for next page
            rc = rcPage;
            RenderParagraph("Second page with annotation", titleFont, rcPage, rc, false);

            // second pass to number pages
            AddFooters();

            // save to file and show it
            return "annotations";
        }

        // measure a paragraph, skip a page if it won't fit, render it into a rectangle,
        // and update the rectangle for the next paragraph.
        // 
        // optionally mark the paragraph as an outline entry and as a link target.
        //
        // this routine will not break a paragraph across pages. for that, see the Text Flow sample.
        //
        internal RectangleF RenderParagraph(string text, _Font font, _Rect rcPage, _Rect rc, bool outline = false, bool linkTarget = false)
        {
            // if it won't fit this page, do a page break
            rc.Height = _c1pdf.MeasureString(text, font, rc.Width).Height;
            if (rc.Bottom > rcPage.Bottom)
            {
                _c1pdf.NewPage();
                rc.Y = rcPage.Top;
            }

            // draw the string
            _c1pdf.DrawString(text, font, _Color.Black, rc);

            // show bounds (mainly to check word wrapping)
            //_c1pdf.DrawRectangle(new _Pen(_Color.Sienna), rc);

            // add headings to outline
            if (outline)
            {
                _c1pdf.DrawLine(new _Pen(_Color.Black), rc.X, rc.Y, rc.Right, rc.Y);
                _c1pdf.AddBookmark(text, 0, rc.Y);
            }

            // add link target
            if (linkTarget)
            {
                _c1pdf.AddTarget(text, rc);
            }

            // update rectangle for next time
            rc.Offset(0, rc.Height);
            return rc;
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

            StringFormat sfVert = new();
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