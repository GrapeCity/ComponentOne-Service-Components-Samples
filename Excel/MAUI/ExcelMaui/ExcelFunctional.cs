using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using C1.Excel;

using _Color = System.Drawing.Color;
using _Bitmap = GrapeCity.Documents.Imaging.GcBitmap;
using _Image = GrapeCity.Documents.Drawing.Image;

namespace ExcelMaui
{
    internal class Functional
    {
        //-------------------------------------------------------------------
        #region ** fields & ctor

        internal C1XLBook _book;

        internal Functional(C1XLBook book)
        {
            // workbook
            _book = book;

            // get the sheet that was created by default, give it a name
            XLSheet sheet = _book.Sheets[0];
            sheet.Name = "C1";

            XLStyle titleStyle = new XLStyle(_book);
            titleStyle.Font = new XLFont("Tahoma", 24, true, false);
            titleStyle.AlignHorz = XLAlignHorz.Center;
            titleStyle.AlignVert = XLAlignVert.Center;

            XLStyle textStyle = new XLStyle(_book);
            textStyle.Font = new XLFont("Tahoma", 8, false, true);
            textStyle.AlignHorz = XLAlignHorz.Center;

            sheet[1, 1].Value = "ComponentOne Excel";
            sheet[1, 1].Style = titleStyle;
            sheet.MergedCells.Add(1, 1, 10, 7);

            Type type = typeof(C1XLBook);
            Assembly assembly = type.Assembly;

            sheet[11, 0].Value = assembly.FullName;
            sheet[11, 0].Style = textStyle;
            sheet.MergedCells.Add(11, 0, 1, 9);

            _book.NamedRanges.Add("Print_Area", new XLCellRange(sheet, 1, 10, 1, 7));
        }

        #endregion

        //-------------------------------------------------------------------
        #region ** data types (test sheet)

        internal void DataTypes()
        {
            // add the sheet with name
            XLSheet sheet = _book.Sheets.Add("data types");

            // column width in twips
            sheet.Columns[2].Width = 1200;
            sheet.Columns[3].Width = 900;

            // create style for comments
            XLStyle commentStyle = new XLStyle(_book);
            commentStyle.Font = new XLFont("Tahoma", 9, false, true);
            commentStyle.ForeColor = _Color.Navy;

            // create style for money values
            XLStyle floatStyle = new XLStyle(_book);
            floatStyle.Format = "#,##0.00";

            // create style for date values
            XLStyle dateStyle = new XLStyle(_book);
            dateStyle.Format = XLStyle.FormatDotNetToXL(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);

            // create style for time values
            XLStyle timeStyle = new XLStyle(_book);
            timeStyle.Format = XLStyle.FormatDotNetToXL(CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern);

            // integer data type
            sheet[1, 0].Value = "Integer:";
            sheet[1, 0].Style = commentStyle;
            sheet[1, 2].Value = 123;

            // float data type
            sheet[2, 0].Value = "Float:";
            sheet[2, 0].Style = commentStyle;
            sheet[2, 2].Value = 123.0089;
            sheet[2, 3].Value = 123.0089;
            sheet[2, 3].Style = floatStyle;

            // date and time data type
            sheet[3, 0].Value = "Date & time:";
            sheet[3, 0].Style = commentStyle;
            sheet[3, 2].Value = DateTime.Now;
            sheet[3, 2].Style = dateStyle;
            sheet[3, 3].Value = DateTime.Now;
            sheet[3, 3].Style = timeStyle;

            // Boolean data type
            sheet[4, 0].Value = "Boolean:";
            sheet[4, 0].Style = commentStyle;
            sheet[4, 2].Value = true;
            sheet[4, 3].Value = false;

            // error
            sheet[5, 0].Value = "Error:";
            sheet[5, 0].Style = commentStyle;
            sheet[5, 2].Value = new Exception("#N/A");

            // images as data types
            XLPictureShape picture;
            sheet[10, 0].Value = "Picture as jpeg (photo), bitmap (windows), png or gif (internet) and metafiles (print):";
            sheet[10, 0].Style = commentStyle;

            // create images
            var spbImage = GetBitmap(GetManifestResource("spb.jpg"));
            var googleImage = GetBitmap(GetManifestResource("google.bmp"));
            var babyImage = GetBitmap(GetManifestResource("baby.png"));

            // picture image
            picture = new XLPictureShape(googleImage);
            picture.LineColor = _Color.Aqua;
            picture.LineWidth = 100;

            // List "Images" -- three methods add images
            sheet.PrintSettings.Header = "&LGoogle:&G&D&CHeader Center&R&P";
            sheet.PrintSettings.Footer = "&C&Z";
            sheet.PrintSettings.HeaderPictureLeft = picture;

            // first method
            picture = new XLPictureShape(googleImage, 0, 0, 2200, 5000);
            picture.DashedLineStyle = XLShapeDashedLineStyle.Solid;
            picture.LineStyle = XLShapeLineStyle.Simple;
            picture.LineColor = _Color.BlueViolet;
            picture.Transparent = _Color.Transparent;
            picture.Rotation = 90.0f;
            picture.LineWidth = 10;
            picture.LeftClip = -15;
            picture.TopClip = -15;
            sheet[4, 6].Value = picture;

            // second method
            picture = new XLPictureShape(spbImage, 100, 3000, 8000, 5000);
            picture.Brightness = .55f;
            picture.Contrast = .5f;
            sheet.Shapes.Add(picture);

            // third method
            picture = new XLPictureShape(babyImage);
            picture.DashedLineStyle = XLShapeDashedLineStyle.Dot;
            picture.Rotation = 30.0f;
            picture.LineColor = _Color.Aqua;
            picture.LineWidth = 100;
            sheet[15, 2].Value = picture;

            // vertical orientation for CJK font (it is necessary using vertical font with @ prefix)
            XLStyle cjkStyle = new XLStyle(_book);
            cjkStyle.Font = new XLFont("@MS UI Gothic", 10);
            cjkStyle.Rotation = 180;
            sheet[1, 5].Value = "CJK:";
            sheet[1, 5].Style = commentStyle;
            sheet[2, 5].Value = "わたしわ　さらさんだすえ";
            sheet[2, 5].Style = cjkStyle;
            sheet.MergedCells.Add(2, 5, 8, 1);

            // rich text format (RTF)
            sheet[6, 0].Value = "RTF:";
            sheet[6, 0].Style = commentStyle;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"{\rtf1{\fonttbl{\f0\fnil Arial;}{\f1\fnil Calibri;}}");
            sb.Append(@"{\colortbl \red0\green0\blue0;\red255\green0\blue0;\red0\green0\blue0;}");
            sb.Append(@"\pard\cf0\f0\fs20\u1055 ?\u1088 ?\cf1 \u1080 ?\u1074 ?\u1077 ?\cf2 \u1090 ?");
            sb.Append(@" \b bold\b0  \i\fs28\f1 \u1090 ?\u1077 ?\u1089 ?\u1090 ?\i0\fs20\f0  \u12469 ?");
            sb.Append(@"\b \u12509 ?\u12540 ?\u12488 ?\u24773 ?\b0\ul \u22577 ?\ulnone  isn't\par 1\par 3\par 3\par end\par}");
            sheet[6, 1].Value = sb.ToString();
            sheet.MergedCells.Add(6, 1, 2, 4);
        }

        #endregion

        //-------------------------------------------------------------------
        #region ** formats (test sheet)

        internal void Formats()
        {
            // add the sheet with name
            XLSheet sheet = _book.Sheets.Add("formats");

            // column width in twips
            sheet.Columns[0].Width = 2300;
            sheet.Columns[1].Width = 2000;

            // add some styles
            XLStyle s1 = new XLStyle(_book);
            XLStyle s2 = new XLStyle(_book);
            XLStyle s3 = new XLStyle(_book);
            s1.Format = "#,##0.00000";
            s2.Format = "#,##0.00000";
            s2.Font = new XLFont("Courier New", 14);
            s3.Format = "dd-MMM-yy";

            // populate sheet with some random values
            Random r = new Random();
            for (int i = 0; i < 100; i++)
            {
#pragma warning disable CA5394 // Do not use insecure randomness
                sheet[i, 0].Value = r.NextDouble() * 100000;
#pragma warning restore CA5394 // Do not use insecure randomness
                sheet[i, 0].Style = (i % 13 == 0) ? s2 : s1;
            }
            for (int i = 0; i < 100; i++)
            {
#pragma warning disable CA5394 // Do not use insecure randomness
                sheet[i, 1].Value = new DateTime(2005, r.Next(1, 12), r.Next(1, 28));
#pragma warning restore CA5394 // Do not use insecure randomness
                sheet[i, 1].Style = s3;
            }
        }

        internal void Styles()
        {
            // add the sheet with name
            XLSheet sheet = _book.Sheets.Add("styles");

            // create styles for odd and even values
            XLStyle styleOdd = new XLStyle(_book);
            styleOdd.Font = new XLFont("Tahoma", 9, false, true);
            styleOdd.ForeColor = _Color.Blue;
            XLStyle styleEven = new XLStyle(_book);
            styleEven.Font = new XLFont("Tahoma", 9, true, false);
            styleEven.ForeColor = _Color.Red;

            // write content and format into some cells
            for (int i = 0; i < 10; i++)
            {
                XLCell cell = sheet[i, 0];
                cell.Value = i + 1;
                cell.Style = ((i + 1) % 2 == 0) ? styleEven : styleOdd;
            }
        }

        #endregion

        //-------------------------------------------------------------------
        #region ** formulas (test sheet)

        internal void Formulas()
        {
            // add the sheet with name
            XLSheet sheet = _book.Sheets.Add("formulas");

            // column width in twips
            sheet.Columns[0].Width = 2000;
            sheet.Columns[1].Width = 2200;

            // string formulas
            string s = "String:";
            sheet[0, 0].Value = s;
            sheet[1, 0].Value = s;
            sheet[2, 0].Value = s;

            sheet[0, 1].Value = "apples";
            sheet[1, 1].Value = "and";
            sheet[2, 1].Value = "oranges";

            s = "String formula:";
            sheet[4, 0].Value = s;
            sheet[5, 0].Value = s;

            sheet[4, 1].Value = "apples and oranges";
            sheet[5, 1].Value = "apples an";
            sheet[4, 1].Formula = "CONCATENATE(B1,\" \",B2, \" \",B3)";
            sheet[5, 1].Formula = "LEFT(B5,9)";

            // simple formulas
            sheet[7, 0].Value = "Formula: 5!";
            sheet[7, 1].Value = 120;
            sheet[7, 1].Formula = "1*2*3*4*5";

            sheet[8, 0].Value = "Formula: 12/0";
            sheet[8, 1].Value = 0;
            sheet[8, 1].Formula = "12/0";

            sheet[9, 0].Value = "Formula: 1 = 1";
            sheet[9, 1].Value = true;
            sheet[9, 1].Formula = "1=1";

            sheet[10, 0].Value = "Formula: 1 = 2";
            sheet[10, 1].Value = false;
            sheet[10, 1].Formula = "1 = 2";

            // now function
            sheet[12, 0].Value = "Formula: Now()";
            sheet[12, 1].Value = DateTime.Now;
            sheet[12, 1].Formula = "Now()";

            XLStyle style = new XLStyle(_book);
            DateTimeFormatInfo dtfi = CultureInfo.CurrentCulture.DateTimeFormat;
            style.Format = XLStyle.FormatDotNetToXL(dtfi.ShortDatePattern + " " + dtfi.ShortTimePattern);
            sheet[12, 1].Style = style;

            // copy of formulas
            int count = 33;
            for (int i = 0; i < count; i++)
            {
                sheet[i, 4].Value = i + 1;
                sheet[i, 5].Value = count - i;
            }
            sheet[0, 6].Formula = "$E1+F1";
            for (int i = 1; i < count; i++)
            {
                sheet.CopyFormula(0, 6, i, 6);
                sheet.CopyFormula(0, 6, i, 7);
                sheet.CopyFormula(0, 6, i, 8);
            }
            Debug.Assert(sheet[29, 7].Formula == "$E30+G30");
        }

        #endregion

        //-------------------------------------------------------------------
        #region ** named ranges (test sheet)

        internal void NamedRanges()
        {
            // add the sheet with name
            XLSheet sheet = _book.Sheets.Add("named ranges");

            // write content and format into some cells
            for (int i = 0; i < 10; i++)
            {
                XLCell cell = sheet[i, 0];
                cell.Value = i + 1;
            }

            _book.NamedRanges.Add("NRTest", sheet, 7, 0);

            sheet[12, 0].Value = 0;
            sheet[12, 0].Formula = "NRTest + 5";

            // add named ranges
            _book.NamedRanges.Add("FTest", sheet, 12, 2);
            _book.NamedRanges.Add("STest", sheet, 14, 2, 2, 3);

            // cell range test
            var cr1 = new XLCellRange(_book, "FTest:STest");
            cr1.Value = 10;
            var cr2 = new XLCellRange(_book, "STest");
            cr2.Value = 20;

            // Bernardo's test for named range
            var nrFirst = _book.NamedRanges.Add("Values.First", sheet, 1, 8);
            var nrSecond = _book.NamedRanges.Add("Values.Second", sheet, 2, 8);
            var nrSum = _book.NamedRanges.Add("SumOfThree", sheet, 1, 8, 3, 1);
            var crFirst = new XLCellRange(sheet, "Values.First");
            crFirst.Value = 300;
            var crSecond = new XLCellRange(sheet, "Values.Second");
            crSecond.Value = 200;
            sheet[3, 8].Value = 500;
            sheet[1, 9].Value = 500;
            sheet[1, 9].Formula = "Values.First + Values.Second";
            sheet[1, 10].Value = 1000;
            sheet[1, 10].Formula = "SUM(SumOfThree)";

            // Bernardo's test
            //_book.ReferenceMode = XLReferenceMode.A1;
            var range1 = new XLCellRange(_book, "Values.First");
            if (range1.RowFrom < 0 || range1.ColumnFrom > 1000)
                throw new Exception("bad range");
            var range2 = new XLCellRange(sheet, "Values.First");
            if (range2.RowFrom < 0 || range2.ColumnFrom > 1000)
                throw new Exception("bad range");
            range1 = new XLCellRange(_book, "named ranges!I1:I11");
            if (range1.RowFrom < 0 || range1.ColumnFrom > 1000)
                throw new Exception("bad range");
            range2 = new XLCellRange(_book.Sheets[0], "named ranges!I1:I11");
            if (range2.RowFrom < 0 || range2.ColumnFrom > 1000)
                throw new Exception("bad range");
            Debug.Assert(range2.ToString().Equals("'named ranges'!I1:I11"));

            // R1C1 test
            //_book.ReferenceMode = XLReferenceMode.R1C1;
            //range1 = new XLCellRange(_book, "named ranges!R1C9:R11C9");
            //if (range1.RowFrom < 0 || range1.ColumnFrom > 1000)
            //    throw new Exception("bad range");
            //range2 = new XLCellRange(_book.Sheets[0], "named ranges!R1C9:R11C9");
            //if (range2.RowFrom < 0 || range2.ColumnFrom > 1000)
            //    throw new Exception("bad range");
            //Debug.Assert(range2.ToString().Equals("'named ranges'!R1C9:R11C9"));
            //_book.ReferenceMode = XLReferenceMode.A1;

            sheet[17, 2].Value = "Used of a print areas in first and this worksheets.";
            _book.NamedRanges.Add("Print_Area", new XLCellRange(sheet, 12, 15, 2, 4));

            // conditional formatting
            var item = new XLConditionalFormatting();
            var rule = new XLConditionalFormattingRule();
            rule.Font = new XLFontFormatting();
            rule.Font.FontColor = _Color.FromArgb(255, 255, 0, 10);  // red
            rule.Font.Bold = true;
            rule.Pattern = new XLPatternFormatting();
            rule.Pattern.Pattern = XLPattern.Solid;
            rule.Pattern.BackColor = _Color.FromArgb(255, 200, 255, 200);
            rule.Operator = XLConditionalFormattingOperator.GreaterThanOrEqual;
            rule.FirstFormula = "7";
            item.Rules.Add(rule);
            item.Ranges.Add(new XLRange(0, 0, 9, 0));
            sheet.ConditionalFormattings.Add(item);
        }

        #endregion

        //-------------------------------------------------------------------
        #region ** helpers

        internal void Save(Stream stream, FileFormat format)
        {
            // save as stream
            _book.Save(stream, format);

            // load as stream
            C1XLBook wb = new C1XLBook();
            wb.Load(stream, format);
        }
        internal void Save(string fileName, bool show = false)
        {
            // save the file
            //string fileName = Application.StartupPath + @"\hello.xls";
            _book.Save(fileName);

            // show the file
            if (show)
            {
                //System.Diagnostics.Process.Start(fileName);
            }
        }

        internal void SaveClone(string fileName, bool show)
        {
            // save the file
            C1XLBook clone = _book.Clone();
            clone.Save(fileName);

            // show the file
            if (show)
            {
                //System.Diagnostics.Process.Start(fileName);
            }
        }

        internal static string GetFilter(bool write)
        {
            StringBuilder sb = new StringBuilder();
            if (write)
                sb.Append("Open XML MS Excel file (*.xlsx)|*.xlsx|");
            sb.Append("Binary MS Excel file (*.xls)|*.xls");
            return sb.ToString();
        }
        public static Stream GetManifestResource(string resource)
        {
            resource = resource.ToLower();
            Assembly a = Assembly.GetExecutingAssembly();
            foreach (string res in a.GetManifestResourceNames())
            {
                if (res.ToLower().EndsWith(resource))
                    return a.GetManifestResourceStream(res);
            }
            return null;
        }

        #endregion

        //--------------------------------------------------
        #region ** bitmap methods

        public static _Image GetImage(string path)
        {
            return _Image.FromFile(path);
        }
        public static _Image GetImage(Stream stream)
        {
            return _Image.FromStream(stream);
        }
        public static _Bitmap GetBitmap(string path)
        {
            var img = new _Bitmap();
            img.Load(File.ReadAllBytes(path));
            return img;
        }
        public static _Bitmap GetBitmap(Stream stream)
        {
            var img = new _Bitmap();
            img.Load(stream);
            return img;
        }
        public static _Bitmap GetBitmap(int pixelWidth, int pixelHeight, bool opaque)
        {
#if DOTNETCOREx
            var img = new _Bitmap();
            var bmp = new GcBitmap(pixelWidth, pixelHeight, opaque);
            using (var ms = new MemoryStream())
            {
                bmp.SaveAsPng(ms);
                ms.Seek(0, SeekOrigin.Begin);
                img.Load(ms);
            }
            return img;
#else
            return new _Bitmap(pixelWidth, pixelHeight, opaque);
#endif
        }

        #endregion
    }
}
