//#define RTF

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
//using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Reflection;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

using C1.Pdf;
using C1.Word;
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
using C1.Word.Objects;
using System.Reflection.PortableExecutable;

namespace CreatePdf
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
#if DEBUG
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
            _word.Info.Company = "MESCIUS";

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
            Save(Quotes(), preview);
            Save(NorthWindTables(), preview);
            if (_doc is C1PdfDocument)
            {
                Save(Images(_ImageQuality.Low), preview);
                Save(Images(_ImageQuality.Medium), preview);
                Save(Images(_ImageQuality.High), preview);
            }
            Save(Images(_ImageQuality.Default), preview);
            Save(ImageAlignment("bruegel.jpg"), preview);
            Save(ImageAlignment("gravitation.jpg"), preview);
            Save(AllFonts(), preview);
            Save(TableContents(), preview);
            Save(FileAttachments(), preview);
            Save(TextFlow(), preview);
#if RTF
            Save(TextAlignment(), preview);
#endif
            // with restore default page size
            Save(PaperSizes(), preview);
            _doc.PaperKind = _PaperKind.Letter;
            _doc.Landscape = false;
            if (_doc is C1WordDocument)
            {
                _word.Dispose();
            }
            else
            {
                _pdf.Dispose();
            }
        }

        //================================================================================
        // create document with quotations from www.quoteland.com
        //
        // shows how to render text with some formatting and how to handle page breaks.
        // this sample grabs content from the web, so it requires an active Internet
        // connection.
        //
        private string Quotes()
        {
            // clear PDF or WORD documents
            _doc.Clear();
            var title = "Quotes from www.quoteland.com";
            if (_doc is C1PdfDocument pdf)
            {
                pdf.DocumentInfo.Title = title;
            }
            else if (_doc is C1WordDocument word)
            {
                word.Info.Title = title;
            }
            Console.WriteLine($"Creating {title}...");

            // get some content from the web
            List<string> authorsQuotes = new();
			try
			{
				// literary quotations
				GetContent(authorsQuotes, "http://www.quoteland.com/topic.asp?CATEGORY_ID=208");

				// 'no-topic' quotations
				GetContent(authorsQuotes, "http://www.quoteland.com/topic.asp?CATEGORY_ID=195");
			}
			catch
			{
               throw new Exception("Failed to retrieve content from the web.");
			}

			// calculate page rectangle (discounting margins)
			_Rect rcPage = GetPageRect();
            _Rect rc = rcPage;

            // initialize output parameters
            _Font hdrFont = new("Book Antiqua", 14, _FontStyle.Bold);
            _Font txtFont = new("Book Antiqua", 10, _FontStyle.Italic);

            // add title
            _Font titleFont = new("Tahoma", 24, _FontStyle.Bold);
			rc = RenderParagraph(title, titleFont, rcPage, rc);

			// build document
			string author = string.Empty;
			foreach (string s in authorsQuotes)
			{
				string[] authorQuote = s.Split('\t');
				
				// render header (author)
				if (author != authorQuote[0])
				{
					author = authorQuote[0];
					rc.Y += 20;
					rc = RenderParagraph(author, hdrFont, rcPage, rc, true);
				}
				
				// render body text (quote)
				string text = authorQuote[1];
				rc.X = rcPage.X + 36; // << indent body text by 1/2 inch
				rc.Width = rcPage.Width - 40;
				rc = RenderParagraph(text, txtFont, rcPage, rc);
				rc.X = rcPage.X; // << restore indent
				rc.Width = rcPage.Width;
				rc.Y += 12; // << add 12pt spacing after each quote
			}

			// second pass to number pages
			AddFooters();

            // done
            return "quotes";
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

		// measure a paragraph, skip a page if it won't fit, render it into a rectangle,
		// and update the rectangle for the next paragraph.
		// 
		// optionally mark the paragraph as an outline entry and as a link target.
		//
		// this routine will not break a paragraph across pages. for that, see the Text Flow sample.
		//
		internal _Rect RenderParagraph(string text, _Font font, _Rect rcPage, _Rect rc, bool outline, bool linkTarget)
		{
			// if it won't fit this page, do a page break
			rc.Height = _doc.MeasureString(text, font, rc.Width).Height;
			if (rc.Bottom > rcPage.Bottom)
			{
                _doc.NewPage();
				rc.Y = rcPage.Top;
			}

			// draw the string
			_doc.DrawString(text, font, _Color.Black, rc);

			// show bounds (mainly to check word wrapping)
			//_doc.DrawRectangle(Pens.Sienna, rc);

			// add headings to outline
			if (outline)
			{
				_doc.DrawLine(_Color.Black, rc.X, rc.Y, rc.Right, rc.Y);
                if (_doc is C1PdfDocument pdf)
                {
                    pdf.AddBookmark(text, 0, rc.Y);
                }
                else if (_doc is C1WordDocument word)
                {
                    word.AddBookmark(text);
                }
            }

            // add link target
            if (linkTarget)
			{
                if (_doc is C1PdfDocument pdf)
                {
                    pdf.AddTarget(text, rc);
                }
                else if (_doc is C1WordDocument word)
                {
                    word.AddLink(text);
                }
            }

            // update rectangle for next time
            rc.Offset(0, rc.Height);
			return rc;
		}
		internal _Rect RenderParagraph(string text, _Font font, _Rect rcPage, _Rect rc, bool outline)
		{
			return RenderParagraph(text, font, rcPage, rc, outline, false);
		}
		internal _Rect RenderParagraph(string text, _Font font, _Rect rcPage, _Rect rc)
		{
			return RenderParagraph(text, font, rcPage, rc, false, false);
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
			_Font fontHorz = new _Font("Tahoma", 7, _FontStyle.Bold);
            _Font fontVert = new _Font("Viner Hand ITC", 14, _FontStyle.Bold);

            StringFormat sfRight = new()
            {
                Alignment = HorizontalAlignment.Right
            };

            StringFormat sfVert  = new StringFormat();
			sfVert.FormatFlags |= StringFormatFlags.DirectionVertical;
			sfVert.Alignment = HorizontalAlignment.Center;

			for (int page = 0; page < _doc.PageCount; page++)
			{
				// select page we want (could change PageSize)
				_doc.CurrentPage = page;

				// build rectangles for rendering text
				_Rect rcPage = GetPageRect();
				_Rect rcFooter = rcPage;
				rcFooter.Y = rcFooter.Bottom + 6;
				rcFooter.Height = 12;
				_Rect rcVert = rcPage;
				rcVert.X = rcPage.Right + 6;

                // add left-aligned footer
                string title = string.Empty;
                if (_doc is C1PdfDocument pdf)
                {
                    title = pdf.DocumentInfo.Title;
                }
                else if (_doc is C1WordDocument word)
                {
                    title = word.Info.Title;
                }
				_doc.DrawString(title, fontHorz,_Color.Gray, rcFooter);

				// add right-aligned footer
				var text = string.Format("Page {0} of {1}", page+1, _doc.PageCount);
				_doc.DrawString(text, fontHorz,_Color.Gray, rcFooter, sfRight);

				// add vertical text
				text = title + " (document created using the C1Pdf .NET component)";
				_doc.DrawString(text, fontVert,_Color.LightGray, rcVert, sfVert);

				// draw lines on bottom and right of the page
				_doc.DrawLine(_Color.Gray, rcPage.Left, rcPage.Bottom, rcPage.Right, rcPage.Bottom);
				_doc.DrawLine(_Color.Gray, rcPage.Right, rcPage.Top, rcPage.Right, rcPage.Bottom);
			}
		}
		private void GetContent(List<string> authorsQuotes, string url)
		{
			// get some content from the web
            Console.WriteLine("Loading data from '{0}'...", url);
			var req = (HttpWebRequest)WebRequest.Create(url);
			HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
			StreamReader sr = new StreamReader(resp.GetResponseStream());
            string content = sr.ReadToEnd();
			resp.Close();

            // break up into authors and quotes
            Console.WriteLine("Parsing quotes...");
            int index = -1;
            var quote = string.Empty;
            List<string> links = new();
            var pattern = @"</?([a-z]+)[^>]*(?<!/)>";
            Regex rx = new(pattern, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
			MatchCollection mc = rx.Matches(content);
            foreach (Match m in mc)
            {
                if (index != -1 && m.Value.StartsWith("</font"))
                {
                    var txt = content.Substring(index, m.Index - index);
                    if (!txt.StartsWith("<"))
                    {
                        foreach (var link in links)
                        {
                            txt = txt.Replace(link, string.Empty);
                        }
                        txt = Regex.Replace(txt, "<BR ?/?>", Environment.NewLine, RegexOptions.IgnoreCase);
                        if (quote.Length > 0 && txt.StartsWith("-"))
                        {
                            // set authors quotes
                            //if (authorsQuotes.Count < 11)
                            authorsQuotes.Add(txt.Substring(1) + '\t' + quote);
                            quote = string.Empty;
                        }
                        else if (quote.Length == 0)
                        {
                            quote = txt;
                        }
                    }
                    links.Clear();
                    index = -1;
                }
                if (index != -1)
                {
                    if (Regex.IsMatch(m.Value, "</?a", RegexOptions.Singleline | RegexOptions.IgnoreCase))
                    {
                        links.Add(m.Value);
                    }
                    else if (Regex.IsMatch(m.Value, "</?b", RegexOptions.Singleline | RegexOptions.IgnoreCase))
                    {
                        links.Add(m.Value);
                    }
                    else if (Regex.IsMatch(m.Value, "</?i", RegexOptions.Singleline | RegexOptions.IgnoreCase))
                    {
                        links.Add(m.Value);
                    }
                    else if (Regex.IsMatch(m.Value, "</?u", RegexOptions.Singleline | RegexOptions.IgnoreCase))
                    {
                        links.Add(m.Value);
                    }
                }
                if (m.Value.StartsWith("<font"))
                {
                    index = m.Index + m.Value.Length;
                    links.Clear();
                }
            }

            // sort by author
            Console.WriteLine("Sorting quotes...");
            authorsQuotes.Sort();
		}

		//================================================================================
		// create document with tables from northwind
		//
		// the sample renders a few tables from the nwind database. although C1Pdf doesn't
		// have any table-specific commands, it is easy to render tables using the 
		// MeasureString and DrawString methods. the RenderTable method in the sample 
		// measures each row, creates a new page when necessary, then renders the rows
		// one by one, with row headers, cell alignment, indentation, and gridlines.
		//
		private string NorthWindTables()
		{
            // clear PDF or WORD documents
            _doc.Clear();
            var title = "NorthWind Tables";
            if (_doc is C1PdfDocument pdf)
            {
                pdf.DocumentInfo.Title = title;
            }
            else if (_doc is C1WordDocument word)
            {
                word.Info.Title = title;
            }
            Console.WriteLine($"Creating {title}...");

            // read NorthWind xml data 
            DataSet ds = new DataSet();
            var path = Path.Combine("Data", "GcNWind.xml");
            _ = ds.ReadXml(path);

            // calculate page rectangle (discounting margins)
            _Rect rcPage = GetPageRect();
			_Rect rc = rcPage;

			// add title
			_Font titleFont = new _Font("Tahoma", 24, _FontStyle.Bold);
			rc = RenderParagraph(title, titleFont, rcPage, rc, false);

			// render some tables
			RenderTable(rc, rcPage, ds, "Customers", new string[] { "CompanyName", "ContactName", "Country", "Address", "Phone" });
			_doc.NewPage();
			rc = rcPage;
			RenderTable(rc, rcPage, ds, "Products",  new string[] { "ProductName", "QuantityPerUnit", "UnitPrice", "UnitsInStock", "UnitsOnOrder" });
			_doc.NewPage();
			rc = rcPage;
			RenderTable(rc, rcPage, ds, "Employees", new string[] { "FirstName", "LastName", "Country", "Notes" });

			// second pass to number pages
			AddFooters();

            // done
            return "nwind";
        }
		private _Rect RenderTable(_Rect rc, _Rect rcPage, DataSet ds, string source, string[] fields)
		{
			// get data
			Console.WriteLine("Retrieving data for {0}...", source);
   //         string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
   //         string conn = string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}\ComponentOne Samples\Common\C1NWind.mdb;", path);
			//OleDbDataAdapter da = new OleDbDataAdapter("select * from " + source, conn);
			//DataTable dt = new DataTable();
			//da.Fill(dt);
            DataTable dt = ds.Tables[source];
            Debug.Assert(dt != null);

            // select fonts
            _Font hdrFont = new _Font("Tahoma", 10, _FontStyle.Bold);
			_Font txtFont = new _Font("Tahoma", 8);

            // build table
            Console.WriteLine("Creating {0} table...", source);
            if (_doc is C1PdfDocument pdf)
            {
                pdf.AddBookmark(source, 0, rc.Y);
            }
            else if (_doc is C1WordDocument word)
            {
                word.AddBookmark(source);
            }
			rc = RenderParagraph("NorthWind " + source, hdrFont, rcPage, rc, false);

			// build table
			rc = RenderTableHeader(hdrFont, rc, fields);
            foreach (DataRow dr in dt.Rows)
            {
                rc = RenderTableRow(txtFont, hdrFont, rcPage, rc, fields, dr);
            }

			// done
			return rc;
		}
		private _Rect RenderTableHeader(_Font font, _Rect rc, string[] fields)
		{
			// calculate cell width (same for all columns)
			_Rect rcCell = rc;
			rcCell.Width = rc.Width / fields.Length;
			rcCell.Height = 0;

			// calculate cell height (max of all columns)
			foreach (string field in fields)
			{
				float height = _doc.MeasureString(field, font, rcCell.Width).Height;
				rcCell.Height = Math.Max(rcCell.Height, height);
			}

			// render header cells
			foreach (string field in fields)
			{
				_doc.FillRectangle(_Color.Black, rcCell);
				_doc.DrawString(field, font, _Color.White, rcCell);
				rcCell.Offset(rcCell.Width, 0);
			}

			// update rectangle and return it
			rc.Offset(0, rcCell.Height);
			return rc;
		}
		private _Rect RenderTableRow(_Font font, _Font hdrFont, _Rect rcPage, _Rect rc, string[] fields, DataRow dr)
		{
			// calculate cell width (same for all columns)
			_Rect rcCell = rc;
			rcCell.Width = rc.Width / fields.Length;
			rcCell.Height = 0;

			// calculate cell height (max of all columns)
			rcCell.Inflate(-4, 0);
			foreach (string field in fields)
			{
				string text = dr[field].ToString();
				float height = _doc.MeasureString(text, font, rcCell.Width).Height;
				rcCell.Height = Math.Max(rcCell.Height, height);
			}
			rcCell.Inflate(4, 0);

			// break page if we have to
			if (rcCell.Bottom > rcPage.Bottom)
			{
				_doc.NewPage();
				rc = RenderTableHeader(hdrFont, rcPage, fields);
				rcCell.Y = rc.Y;
			}

            // center vertically just to show how
            StringFormat sf = new()
            {
                LineAlignment = VerticalAlignment.Center
            };

            // render data cells
            _Pen pen = new(_Color.Gray, 0.1f);
			foreach (string field in fields)
			{
				// get content
				string text = dr[field].ToString();

				// set horizontal alignment
				double d;
				sf.Alignment = (double.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out d))
					? HorizontalAlignment.Right
					: HorizontalAlignment.Left;

				// render cell
				_doc.DrawRectangle(pen, rcCell);
				rcCell.Inflate(-4, 0);
				_doc.DrawString(text, font,_Color.Black, rcCell, sf);
				rcCell.Inflate(4, 0);
				rcCell.Offset(rcCell.Width, 0);
			}

			// update rectangle and return it
			rc.Offset(0, rcCell.Height);
			return rc;
		}

        //================================================================================
        // create document with lots of images
        //
        // this sample creates a document with several pictures of different types and 
        // saves three versions of the document with different qualities (high, low, and
        // default).
        //
        private string Images(_ImageQuality quality)
        {
            // clear PDF or WORD documents
            _doc.Clear();
            var title = "Images";
            if (_doc is C1PdfDocument pdf)
            {
                pdf.DocumentInfo.Title = title;
            }
            else if (_doc is C1WordDocument word)
            {
                word.Info.Title = title;
            }
            Console.WriteLine($"Creating {title}...");

            // calculate page rectangle (discounting margins)
            _Rect rcPage = GetPageRect();
            _Rect rc = rcPage;
            rc.Inflate(-10, 0);

            // add title
            _Font titleFont = new _Font("Tahoma", 24, _FontStyle.Bold);
            rc = RenderParagraph(title, titleFont, rcPage, rc, false);

            _Font hdrFont = new("Tahoma", 16, _FontStyle.Bold);
            //_Font ftrFont = new("Tahoma", 8);

            // render louvre images
            rc = RenderParagraph("Louvre Images", hdrFont, rcPage, rc, true);
            foreach (string res in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                if (res.ToLower().IndexOf("louvre") > -1)
                {
                    rc = RenderImage(rcPage, rc, res);
                }
            }
            _doc.NewPage();
            rc.Y = rcPage.Y;

            // render Escher images
            rc = RenderParagraph("Escher Images", hdrFont, rcPage, rc, true);
            foreach (string res in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                if (res.ToLower().IndexOf("escher") > -1)
                {
                    rc = RenderImage(rcPage, rc, res);
                }
            }
			_doc.NewPage();
			rc.Y = rcPage.Y;

			// render icons
			rc = RenderParagraph("Icons (transparent)", hdrFont, rcPage, rc, true);
            foreach (string res in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                if (res.ToLower().IndexOf("icons") > -1)
                {
                    rc = RenderImage(rcPage, rc, res);
                }
            }

			// render everything else
			rc = RenderParagraph("Other", hdrFont, rcPage, rc, true);
            foreach (string res in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                if (res.ToLower().IndexOf("other") > -1)
                {
                    rc = RenderImage(rcPage, rc, res);
                }
            }

			// second pass to number pages
			AddFooters();

            if (_doc is C1PdfDocument c1pdf)
            {
                // quality
                c1pdf.ImageQuality = quality;
                switch (quality)
                {
                    case _ImageQuality.Low:
                        return "imagesLow";
                    case _ImageQuality.High:
                        return "imagesHigh";
                }
            }

            // done
            return "images";
        }
        private _Rect RenderImage(_Rect rcPage, _Rect rc, string resName)
		{
			// get image
			Assembly a = Assembly.GetExecutingAssembly();
			_Image img = _Image.FromStream(a.GetManifestResourceStream(resName));

            // calculate image height
            // based on image size and page size
            rc.Height = Math.Min(img.Height / 96f * 72, rcPage.Height * .3f);

			// skip page if necessary
			if (rc.Bottom > rcPage.Bottom)
			{
				_doc.NewPage();
				rc.Y = rcPage.Y;
			}

			// add bookmark
			string[] arr = resName.Split('.');
			string picName = string.Format("{0}.{1}", arr[arr.Length-2], arr[arr.Length-1]);
            if (_doc is C1PdfDocument pdf)
            {
                pdf.AddBookmark(picName, 1, rc.Y);
            }
            else if (_doc is C1WordDocument word)
            {
                word.AddBookmark(picName);
            }

			// draw solid background (mainly to see transparency)
			rc.Inflate(+2, +2);
			_doc.FillRectangle(_Color.Gray, rc);
			rc.Inflate(-2, -2);

            // draw image (keep aspect ratio)
            if (_doc is C1PdfDocument c1pdf)
            {
                c1pdf.DrawImage(img, rc, ContentAlignment.MiddleCenter, _ImageSizeMode.Scale);
            }
            else if (_doc is C1WordDocument c1word)
            {
                c1word.DrawImage(img, rc);
            }

			// draw caption
			_Font font = new _Font("Tahoma", 9);
			rc.Y = rc.Bottom + 3;
			rc.Height = 2 * font.Size;
			rc.Offset(+.3f, +.3f);
			_doc.DrawString(picName, font, _Color.Yellow, rc);
			rc.Offset(-.3f, -.3f);
			_doc.DrawString(picName, font, _Color.Black, rc);

			// update rectangle
			rc.Y = rc.Bottom + 20;
			return rc;
		}

		//================================================================================
		// create document showing image some alignment options
		//
		// the DrawImage method allows you to specify the content alignment and scaling
		// mode for images. the content alignment parameter sets the position of the 
		// image within the given rectangle (left top, right bottom, etc). the scaling 
		// mode sets the sizing of the image within the rectangle (clip, stretch, scale).
		//
		private string ImageAlignment(string name)
		{
            // clear PDF or WORD documents
            _doc.Clear();
            var title = "Image Alignment";
            if (_doc is C1PdfDocument pdf)
            {
                pdf.DocumentInfo.Title = title;
            }
            else if (_doc is C1WordDocument word)
            {
                word.Info.Title = title;
            }
            Console.WriteLine($"Creating {title}...");

            // do it
            foreach (string res in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                if (res.ToLower().IndexOf(name) > -1)
                {
                    Assembly a = Assembly.GetExecutingAssembly();
                    _Image img = _Image.FromStream(a.GetManifestResourceStream(res));
                    ShowImageAlignment(img);
                }
            }

            // done
            return "imgAlign_" + Path.GetFileNameWithoutExtension(name);
		}
        private void ShowImageAlignment(_Image img)
        {
            // calculate page rectangle (discounting margins)
            _Rect rcPage = GetPageRect();
            _Rect rc = rcPage;
            rc.Inflate(-10, 0);

            // add title
            string title = string.Empty;
            if (_doc is C1PdfDocument pdf)
            {
                title = pdf.DocumentInfo.Title;
            }
            else if (_doc is C1WordDocument word)
            {
                title = word.Info.Title;
            }
            _Font font = new _Font("Tahoma", 9);
            _Font titleFont = new _Font("Tahoma", 24, _FontStyle.Bold);
            rc = RenderParagraph(title, titleFont, rcPage, rc, false);

            // pens
            var penBlack = new _Pen(_Color.Black);
            var penLightGray = new _Pen(_Color.LightGray);

            // draw images clipping
            rc.Y += 30;
            rc = RenderParagraph("Change ContentAlignment, clip images.", font, rcPage, rc);
            rc.Y += 5;
            rc.Height = 100;
            foreach (ContentAlignment ca in Enum.GetValues(typeof(ContentAlignment)))
            {
                if (_doc is C1PdfDocument c1pdf)
                {
                    c1pdf.DrawImage(img, rc, ca, _ImageSizeMode.Clip);
                }
                else if (_doc is C1WordDocument c1word)
                {
                    c1word.DrawImage(img, rc);
                }
            }
            rc.Inflate(+2, +2);
            _doc.DrawRectangle(penLightGray, rc);
            rc.Inflate(-2, -2);
            _doc.DrawRectangle(penBlack, rc);

            // draw images scaling
            rc.Y += rc.Height + 20;
            rc.Inflate(-40, -40);
            rc = RenderParagraph("Change ContentAlignment, scale images.", font, rcPage, rc);
            rc.Y += 5;
            rc.Height = 100;
            if (_doc is C1PdfDocument pdfDocument)
            {
                pdfDocument.DrawImage(img, rc, ContentAlignment.TopLeft, _ImageSizeMode.Scale);
                pdfDocument.DrawImage(img, rc, ContentAlignment.TopRight, _ImageSizeMode.Scale);
                pdfDocument.DrawImage(img, rc, ContentAlignment.TopCenter, _ImageSizeMode.Scale);
            }
			rc.Inflate(+2,+2);
			_doc.DrawRectangle(penLightGray, rc);
			rc.Inflate(-2,-2);
			_doc.DrawRectangle(penBlack, rc);
		}

		//================================================================================
		// create document with all installed fonts
		//
		// the sample enumerates all fonts available in the system and writes one line
		// using each font.
		//
		string AllFonts()
		{
            // clear PDF or WORD documents
            _doc.Clear();
            var title = "All Fonts";
            if (_doc is C1PdfDocument pdf)
            {
                pdf.DocumentInfo.Title = title;
            }
            else if (_doc is C1WordDocument word)
            {
                word.Info.Title = title;
            }
            Console.WriteLine($"Creating {title}...");
            _Rect rcPage = GetPageRect();

			// add title
			_Rect rc = rcPage;
			_Font titleFont = new _Font("Tahoma", 24, _FontStyle.Bold);
			rc = RenderParagraph(title, titleFont, rcPage, rc, false);

			// draw text in many fonts
			_Font font = new _Font("Tahoma", 9);
			rc.Height = 11;
            var sfc = GrapeCity.Documents.Text.FontCollection.SystemFonts;
			foreach (var f in sfc)
			{
				// create font
				_Font sample = _Font.Create(f, 9);
				if (sample == null) continue;

				// show font
				_doc.DrawString(sample.Name, font,_Color.Black, rc);
				rc.X += 120; rc.Width -= 120;
				_doc.DrawString("The quick brown fox jumped over the lazy dog. 1234567890!", sample, _Color.Black, rc);
				rc.X -= 120; rc.Width += 120;
				_doc.DrawLine(_Color.LightGray, rc.X, rc.Bottom, rc.X + 130, rc.Bottom);

				// move cursor and skip page if necessary
				rc.Y += rc.Height;
				if (rc.Bottom > _doc.PageSize.Height - 72)
				{
					rc.Y = rcPage.Y;
					_doc.NewPage();
				}
			}

			// show footers as usual
			AddFooters();

            // done
            return "allFonts";
		}

		//================================================================================
		// create a document with all paper sizes
		//
		// PDF allows you to create documents with different page sizes and orientation.
		// this sample enumerates the members of the PaperKind enum type and creates
		// a document with all paper sizes and random orientation.
		//
		private string PaperSizes()
		{
            // clear PDF or WORD documents
            _doc.Clear();
            var title = "Paper Sizes";
            if (_doc is C1PdfDocument pdf)
            {
                pdf.DocumentInfo.Title = title;
            }
            else if (_doc is C1WordDocument word)
            {
                word.Info.Title = title;
            }
            Console.WriteLine($"Creating {title}...");
            
			// add title
			_Font titleFont = new _Font("Tahoma", 24, _FontStyle.Bold);
			_Rect rc = GetPageRect();
			RenderParagraph(title, titleFont, rc, rc, false);

			// create constant font and StringFormat objects
			_Font font = new _Font("Tahoma", 18);
			StringFormat sf = new StringFormat();
			sf.Alignment = HorizontalAlignment.Center;
			sf.LineAlignment = VerticalAlignment.Center;

			// create one page with each paper size
			bool firstPage = true;
			foreach (_PaperKind pk in Enum.GetValues(typeof(_PaperKind)))
			{
				// skip custom size
				if (pk == _PaperKind.Custom) continue;

				// add new page for every page after the first one
				if (!firstPage) _doc.NewPage();
				firstPage = false;

				// set paper kind and orientation
				_doc.PaperKind = pk;
				_doc.Landscape = !_doc.Landscape;
				
				// draw some content on the page
				rc = GetPageRect();
				rc.Inflate(-6, -6);
				string text = string.Format("PaperKind: [{0}];\r\nLandscape: [{1}];\r\nFont: [Tahoma 18pt]",
					_doc.PaperKind, _doc.Landscape);
				_doc.DrawString(text, font, _Color.Black, rc, sf);
				_doc.DrawRectangle(_Color.Blue, rc);
			}

			// show footers as usual
			AddFooters();

            // done
            return "pageSizes";

		}

        //================================================================================
        // create a document with a table of contents
        //
        // the sample keeps a list of bookmarks added to the document as it is rendered.
        // when the document is done, the sample uses the list to create a table of 
        // contents with links to each topic. finally, the sample moves the table of
        // contents to the start of the document using the Pages collection.
        //
        private string TableContents()
		{
            // clear PDF or WORD documents
            _doc.Clear();
            var title = "Document with Table of Contents";
            if (_doc is C1PdfDocument pdf)
            {
                pdf.DocumentInfo.Title = title;
            }
            else if (_doc is C1WordDocument word)
            {
                word.Info.Title = title;
            }
            Console.WriteLine($"Creating {title}...");
            
			// add title
			_Font titleFont = new _Font("Tahoma", 24, _FontStyle.Bold);
			_Rect rcPage = GetPageRect();
			_Rect rc = RenderParagraph(title, titleFont, rcPage, rcPage, false);
			rc.Y += 12;
			
			// create nonsense document
			List<string[]> bkmk = new();
			_Font headerFont = new _Font("Arial", 14, _FontStyle.Bold);
			_Font bodyFont = new _Font("Times New Roman", 11);
			for (int i = 0; i < 30; i++)
			{
				// create ith header (as a link target and outline entry)
				string header = string.Format("{0}. {1}", i + 1, BuildRandomTitle());
				rc = RenderParagraph(header, headerFont, rcPage, rc, true, true);

				// save bookmark to build TOC later
				int pageNumber = _doc.CurrentPage + 1;
				bkmk.Add(new string[] { pageNumber.ToString(), header });

				// create some text
				rc.X += 36;
				rc.Width -= 36;
				for (int j = 0; j < 3 + _rnd.Next(20); j++)
				{
					string text = BuildRandomParagraph();
					rc = RenderParagraph(text, bodyFont, rcPage, rc);
					rc.Y += 6;
				}
				rc.X -= 36;
				rc.Width += 36;
				rc.Y += 20;
			}

			// number pages (before adding TOC)
			AddFooters();

			// start Table of Contents
			_doc.NewPage();					// start TOC on a new page
			int tocPage = _doc.CurrentPage;	// save page index (to move TOC later)
			rc = RenderParagraph("Table of Contents", titleFont, rcPage, rcPage, true);
			rc.Y += 12;
			rc.X += 30;
			rc.Width -= 40;

			// render Table of Contents
			_Pen dottedPen = new _Pen(_Color.Gray, 1.5f);
			dottedPen.DashStyle = _DashStyle.Dot;
            StringFormat sfRight = new()
            {
                Alignment = HorizontalAlignment.Right
            };
            rc.Height = bodyFont.Height;
			foreach (string[] entry in bkmk)
			{
				// get bookmark info
				string page   = entry[0];
				string header = entry[1];

				// render header name and page number
				_doc.DrawString(header, bodyFont,_Color.Black, rc);
				_doc.DrawString(page, bodyFont,_Color.Black, rc, sfRight);
#if true
				// connect the two with some dots (looks better than a dotted line)
				string dots = ". ";
				float wid = _doc.MeasureString(dots, bodyFont).Width;
				float x1  = rc.X + _doc.MeasureString(header, bodyFont).Width + 8;
				float x2  = rc.Right - _doc.MeasureString(page, bodyFont).Width - 8;
				float x = rc.X;
                for (rc.X = x1; rc.X < x2; rc.X += wid)
                {
                    _doc.DrawString(dots, bodyFont, _Color.Gray, rc);
                }
				rc.X = x;
#else
				// connect with a dotted line (another option)
				float x1 = rc.X + _doc.MeasureString(header, bodyFont).Width + 5;
				float x2 = rc.Right - _doc.MeasureString(page, bodyFont).Width  - 5;
				float y  = rc.Top + bodyFont.Size;
				_doc.DrawLine(dottedPen, x1, y, x2, y);
#endif
                // add local hyperlink to entry
                if (_doc is C1PdfDocument c1pdf)
                {
                    c1pdf.AddLink("#" + header, rc);
                }
                else if (_doc is C1WordDocument c1word)
                {
                    c1word.AddLink("#" + header);
                }

                // move on to next entry
                rc.Offset(0, rc.Height);
				if (rc.Bottom > rcPage.Bottom)
				{
					_doc.NewPage();
					rc.Y = rcPage.Y;
				}
			}

            // move table of contents to start of document
            if (_doc is C1PdfDocument pdfDocument)
            {
                PdfPage[] arr = new PdfPage[pdfDocument.Pages.Count - tocPage];
                pdfDocument.Pages.CopyTo(tocPage, arr, 0, arr.Length);
                pdfDocument.Pages.RemoveRange(tocPage, arr.Length);
                pdfDocument.Pages.InsertRange(0, arr);
            }

            // done
            return "toc";
        }
		private string BuildRandomTitle()
		{
			string[] a1 = "Learning|Explaining|Mastering|Forgetting|Examining|Understanding|Applying|Using|Destroying".Split('|');
			string[] a2 = "Music|Tennis|Golf|Zen|Diving|Modern Art|Gardening|Architecture|Mathematics|Investments|.NET|Java".Split('|');
			string[] a3 = "Quickly|Painlessly|The Hard Way|Slowly|Painfully|With Panache".Split('|');
#pragma warning disable CA5394 // Do not use insecure randomness
            return string.Format("{0} {1} {2}", a1[_rnd.Next(a1.Length - 1)], a2[_rnd.Next(a2.Length - 1)], a3[_rnd.Next(a3.Length - 1)]);
#pragma warning restore CA5394 // Do not use insecure randomness
		}
		private string BuildRandomParagraph()
		{
			StringBuilder sb = new StringBuilder();
#pragma warning disable CA5394 // Do not use insecure randomness
            for (int i = 0; i < 5 + _rnd.Next(10); i++)
#pragma warning restore CA5394 // Do not use insecure randomness
            {
                sb.AppendFormat(BuildRandomSentence());
            }
			return sb.ToString();
		}
		private string BuildRandomSentence()
		{
			string[] a1 = "Artists|Movie stars|Musicians|Politicians|Computer programmers|Modern thinkers|Gardeners|Experts|Some people|Hockey players".Split('|');
			string[] a2 = "know|seem to think about|care about|often discuss|dream about|hate|love|despise|respect|long for|pay attention to|embrace".Split('|');
			string[] a3 = "the movies|chicken soup|tea|many things|sushi|my car|deep thoughs|tasteless jokes|vaporware|cell phones|hot dogs|ballgames".Split('|');
			string[] a4 = "incessantly|too much|easily|without reason|rapidly|sadly|randomly|vigorously|more than usual|with enthusiasm|shamelessly|on Tuesdays".Split('|');
#pragma warning disable CA5394 // Do not use insecure randomness
            return string.Format("{0} {1} {2} {3}. ", a1[_rnd.Next(a1.Length - 1)], a2[_rnd.Next(a2.Length - 1)], a3[_rnd.Next(a3.Length - 1)], a4[_rnd.Next(a4.Length - 1)]);
#pragma warning restore CA5394 // Do not use insecure randomness
        }

		//================================================================================
		// create a document with file attachments
		// 
		string FileAttachments()
		{
            // clear PDF or WORD documents
            _doc.Clear();
            var title = "File Attachments";
            if (_doc is C1PdfDocument pdf)
            {
                pdf.DocumentInfo.Title = title;
            }
            else if (_doc is C1WordDocument word)
            {
                word.Info.Title = title;
            }
            Console.WriteLine($"Creating {title}...");

            // add title
            _Font titleFont = new _Font("Tahoma", 24, _FontStyle.Bold);
			_Font bodyFont  = new _Font("Tahoma", 10);
			_Rect rcPage = GetPageRect();
			_Rect rc = RenderParagraph(title, titleFont, rcPage, rcPage, false);
			rc.Y += rc.Height;

			// attach some files
			string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			int pos = path.IndexOf(@"\bin");
			if (pos > -1) path = path.Substring(0, pos);
			string[] files = Directory.GetFiles(path);
			foreach (string file in files)
			{
				_Rect rcAttach = new _Rect(rc.X, rc.Y, rc.Height, rc.Height);
				_doc.FillRectangle(_Color.Gray, rcAttach);
                if (_doc is C1PdfDocument c1pdf)
                {
                    c1pdf.AddAttachment(file, rcAttach);
                }
                else if (_doc is C1WordDocument c1word)
                {
                    // TODO:
                }

				rc.X += rc.Height;
				RenderParagraph(Path.GetFileName(file), bodyFont, rcPage, rc, false);
				rc.X -= rc.Height;
				rc.Y += 2 * rc.Height;
			}

            // done
            return "attachments";
		}

		//================================================================================
		// create a document with flowing text
		// 
		// the sample renders a long string into a multi-page 2-column document.
		//
		// this sample uses the return value of the DrawString method, which returns the
		// index of the first character that did not fit into the box. with this value,
		// it is easy to continue rendering after each break.
		//
		string TextFlow()
		{
			// load long string from resource file
			string text = "Resource not found...";
			Assembly a = Assembly.GetExecutingAssembly();
			foreach (string res in a.GetManifestResourceNames())
			{
				if (res.ToLower().IndexOf("flow.txt") > -1)
				{
					StreamReader sr = new StreamReader(a.GetManifestResourceStream(res));
					text = sr.ReadToEnd();
                    break;
				}
			}
			text = text.Replace("\t", "   ");
			text = string.Format("{0}\r\n\r\n---oOoOoOo---\r\n\r\n{0}", text);

            // clear PDF or WORD documents
            _doc.Clear();
            var title = "Text Flow";
            if (_doc is C1PdfDocument pdf)
            {
                pdf.DocumentInfo.Title = title;
            }
            else if (_doc is C1WordDocument word)
            {
                word.Info.Title = title;
            }
            Console.WriteLine($"Creating {title}...");

            // add title
            _Font titleFont = new _Font("Tahoma", 24, _FontStyle.Bold);
			_Font bodyFont  = new _Font("Tahoma", 9);
			_Rect rcPage = GetPageRect();
			_Rect rc = RenderParagraph(title, titleFont, rcPage, rcPage, false);
			rc.Y += titleFont.Size + 6;
			rc.Height = rcPage.Height - rc.Y;

			// create two columns for the text
			_Rect rcLeft = rc;
			rcLeft.Width = rcPage.Width/2 - 12;
			rcLeft.Height = 300;
			rcLeft.Y = (rcPage.Y + rcPage.Height - rcLeft.Height)/2;
			_Rect rcRight = rcLeft;
			rcRight.X = rcPage.Right - rcRight.Width;

            // start with left column
			rc = rcLeft;
            var penSilver = new _Pen(_Color.Silver);

            // render string spanning columns and pages
            for (;;)
			{
				// render as much as will fit into the rectangle
				rc.Inflate(-3, -3);
				int nextChar = _doc.DrawString(text, bodyFont,_Color.Black, rc);
				rc.Inflate(+3, +3);
				_doc.DrawRectangle(penSilver, rc);

                // break when done
                if (nextChar >= text.Length)
                {
                    break;
                }

				// get rid of the part that was rendered
				text = text.Substring(nextChar);

				// switch to right-side rectangle
				if (rc.Left == rcLeft.Left)
				{
					rc = rcRight;
				}
				else // switch to left-side rectangle on the next page
				{
					_doc.NewPage();
					rc = rcLeft;
				}
			}

            // done
            return "flow";
		}
#if RTF
		//================================================================================
		// create a document with different kinds of text alignment
		//	
		string TextAlignment()
		{
			// create pdf document
			_doc.Clear();
			_doc.DocumentInfo.Title = "Text Alignment";
            Console.WriteLine($"Creating {_doc.DocumentInfo.Title}...");

            // create objects
            StringFormat sf = new();
			_Font titleFont = new _Font("Tahoma", 24, _FontStyle.Bold);
			_Font font = new _Font("Tahoma", 10);
			_Rect rcPage = GetPageRect();

			// render title
			RenderParagraph(_doc.DocumentInfo.Title, titleFont, rcPage, rcPage, false);

			// draw cross-hair
			_Pen pen = new _Pen(Color.Black, 0.1f);
			_Point center = new _Point(rcPage.X+rcPage.Width / 2, rcPage.Y + rcPage.Height/2);
			_doc.DrawLine(pen, center.X, rcPage.Y, center.X, rcPage.Bottom);
			_doc.DrawLine(pen, rcPage.X, center.Y, rcPage.Right, center.Y);

			// draw some text
			string text = "This is some sample text that will be rendered in rectangles on the page.";
            var penBlack = new _Pen(Color.Black);

            sf.Alignment = HorizontalAlignment.Right;
			sf.LineAlignment = VerticalAlignment.Bottom;
			_Rect rc = new _Rect(center.X-100, center.Y-100, 100, 100);
			_doc.FillRectangle(Color.LightGoldenrodYellow, rc);
			_doc.DrawString("TOP LEFT: " + text, font,_Color.Black, rc, sf);
			_doc.DrawRectangle(penBlack, rc);

			sf.LineAlignment = VerticalAlignment.Top;
			rc.Offset(0, rc.Height);
			_doc.FillRectangle(Color.LightSalmon, rc);
			_doc.DrawString("BOTTOM LEFT: " + text, font,_Color.Black, rc, sf);
			_doc.DrawRectangle(penBlack, rc);

			sf.Alignment = HorizontalAlignment.Left;
			rc.Offset(rc.Width, 0);
			_doc.FillRectangle(Color.LightSeaGreen, rc);
			_doc.DrawString("BOTTOM RIGHT: " + text, font,_Color.Black, rc, sf);
			_doc.DrawRectangle(penBlack, rc);

			sf.LineAlignment = VerticalAlignment.Bottom;
			rc.Offset(0, -rc.Height);
			_doc.FillRectangle(Color.LightSteelBlue, rc);
			_doc.DrawString("TOP RIGHT: " + text, font,_Color.Black, rc, sf);
			_doc.DrawRectangle(penBlack, rc);

			// render some rtf as well
			string qbf = "The quick brown fox jumped over the lazy dog. ";
			qbf = qbf + qbf + qbf + qbf;
			text =  @"This is some {\b RTF} text. It will render into the rectangle as usual.\par\par" +
					@"\qr And {\i this paragraph} will be {\b\i right-aligned}. {\fs12 " + qbf + @"\par\par}" +
					@"\qc And {\i this paragraph} will be {\b\i center-aligned}. {\fs12 " + qbf + @"\par\par}" +
					@"\qj And {\i this paragraph} will be {\b\i justified}. {\fs12 " + qbf + @"\par\par Done.}";
			rc.Location = new PointF(rcPage.X + 12, rcPage.Y + 50);
			rc.Size = _doc.MeasureStringRtf(text, font, rc.Width * 2);
			_doc.FillRectangle(Color.DarkBlue, rc);
			_doc.DrawStringRtf(text, font,_Color.White, rc);
			_doc.DrawRectangle(penBlack, rc);

			// and some rtf with bullets
			text = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fswiss\fcharset0 Arial;}{\f1\fnil\fcharset2 Symbol;}}" +
				   @"\viewkind4\uc1\pard\f0\fs20 Here's a bullet list:\par" +
				   @"\par" +
				   @"\pard{\pntext\f1\'B7\tab}{\*\pn\pnlvlblt\pnf1\pnindent0{\pntxtb\'B7}}\fi-360\li360 Item 1\par" +
				   @"{\pntext\f1\'B7\tab}Item 2\par" +
				   @"{\pntext\f1\'B7\tab}Item 3\par" +
				   @"{\pntext\f1\'B7\tab}Item 4\par" +
				   @"\pard\par" +
				   @"}";
			rc.Location = new PointF(rcPage.X + 250, rcPage.Y + 50);
			rc.Size = _doc.MeasureStringRtf(text, font, rc.Width);
			_doc.DrawStringRtf(text, font,_Color.White, rc);
			_doc.DrawRectangle(penBlack, rc);

			// box the whole thing
			pen = new Pen(Color.Black, 1);
			pen.DashStyle = DashStyle.Dot;
			_doc.DrawRectangle(pen, rcPage);

            // done
            return "align";
		}
#endif
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
