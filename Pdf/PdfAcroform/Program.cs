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

namespace PdfAcroform
{
    class Program
    {
        private int _textBoxCount;
        private int _checkBoxCount;
        private int _radioButtonCount;
        private int _pushButtonCount;
        private int _comboBoxCount;
        private int _listBoxCount;
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
            // name and security
            _c1pdf.DocumentInfo.Producer = "ComponentOne C1.Pdf";
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
            Save(Test(), preview);
        }

        private string Test()
        {
            // create PDF document
            _c1pdf.Clear();
            _c1pdf.DocumentInfo.Title = "PDF Acroform";
            Console.WriteLine("Creating pdf document...");

            // calculate page rectangle (discounting margins)
            _Rect rcPage = GetPageRect();
            _Rect rc = rcPage;

            // add title
            _Font titleFont = new("Tahoma", 24, _FontStyle.Bold);
            rc = RenderParagraph(_c1pdf.DocumentInfo.Title, titleFont, rcPage, rc, false);

            // render acroforms
            rc = rcPage;
            _Font fieldFont = new("Arial", 14, _FontStyle.Regular);

            // text box field
            rc = new RectangleF(rc.X, rc.Y + rc.Height / 10, rc.Width / 3, rc.Height / 30);
            PdfTextBox textBox1 = RenderTextBox("TextBox Sample", fieldFont, rc);
            textBox1.BorderWidth = FieldBorderWidth.Thick;
            textBox1.BorderStyle = FieldBorderStyle.Inset;
            textBox1.BorderColor = Color.Green;
            //textBox1.BackColor = Color.Yellow;

            // first check box field
            rc = new RectangleF(rc.X, rc.Y + 2 * rc.Height, rc.Width, rc.Height);
            RenderCheckBox(true, "CheckBox 1 Sample", fieldFont, rc);

            // first radio button group
            rc = new RectangleF(rc.X, rc.Y + 2 * rc.Height, rc.Width, rc.Height);
            RenderRadioButton(false, "RadioGroup1", "RadioButton 1 Sample Group 1", fieldFont, rc);
            rc = new RectangleF(rc.X, rc.Y + 2 * rc.Height, rc.Width, rc.Height);
            RenderRadioButton(true, "RadioGroup1", "RadioButton 2 Sample Group 1", fieldFont, rc);
            rc = new RectangleF(rc.X, rc.Y + 2 * rc.Height, rc.Width, rc.Height);
            RenderRadioButton(false, "RadioGroup1", "RadioButton 3 Sample Group 1", fieldFont, rc);

            // second check box field
            rc = new RectangleF(rc.X, rc.Y + 2 * rc.Height, rc.Width, rc.Height);
            RenderCheckBox(false, "CheckBox 2 Sample", fieldFont, rc);

            // second radio button group
            rc = new RectangleF(rc.X, rc.Y + 2 * rc.Height, rc.Width, rc.Height);
            RenderRadioButton(true, "RadioGroup2", "RadioButton 1 Sample Group 2", fieldFont, rc);
            rc = new RectangleF(rc.X, rc.Y + 2 * rc.Height, rc.Width, rc.Height);
            RenderRadioButton(false, "RadioGroup2", "RadioButton 2 Sample Group 2", fieldFont, rc);

            // first combo box field
            rc = new RectangleF(rc.X, rc.Y + 2 * rc.Height, rc.Width, rc.Height);
            RenderComboBox(new string[] { "First", "Second", "Third" }, 2, fieldFont, rc);

            // first list box field
            RectangleF rclb = new RectangleF(rc.X, rc.Y + 2 * rc.Height, rc.Width, 3 * rc.Height);
            RenderListBox(new string[] { "First", "Second", "Third", "Fourth", "Fifth" }, 5, fieldFont, rclb);

            // load first icon
            _Image icon = null;
            using (var stream = GetManifestResource("phoenix.png"))
            using (var ms = new MemoryStream((int)stream.Length))
            {
                stream.CopyTo(ms);
                icon = _Image.FromBytes(ms.ToArray());
            }

            // first push button field
            rc = new RectangleF(rc.X, rc.Y + 6 * rc.Height, rc.Width, rc.Height);
            PdfPushButton button1 = RenderPushButton("Submit", fieldFont, rc, icon, ButtonLayout.ImageLeftTextRight);
            button1.Actions.Released.Add(new PdfPushButton.Action(ButtonAction.CallMenu, "FullScreen"));
            button1.Actions.GotFocus.Add(new PdfPushButton.Action(ButtonAction.OpenFile, @"..\..\Program.cs"));
            button1.Actions.LostFocus.Add(new PdfPushButton.Action(ButtonAction.GotoPage, "2"));
            button1.Actions.Released.Add(new PdfPushButton.Action(ButtonAction.OpenUrl, "http://www.componentone.com/"));

            // load second icon
            using (var stream = GetManifestResource("download.png"))
            using (var ms = new MemoryStream((int)stream.Length))
            {
                stream.CopyTo(ms);
                icon = _Image.FromBytes(ms.ToArray());
            }

            // second push putton field
            rc = new RectangleF(rc.X, rc.Y + 2 * rc.Height, rc.Width, 2 * rc.Height);
            PdfPushButton button2 = RenderPushButton("Cancel", fieldFont, rc, icon, ButtonLayout.TextTopImageBottom);
            button2.Actions.Pressed.Add(new PdfPushButton.Action(ButtonAction.ClearFields));
            button2.Actions.Released.Add(new PdfPushButton.Action(ButtonAction.CallMenu, "Quit"));

            // load thread icon
            using (var stream = GetManifestResource("top100.png"))
            using (var ms = new MemoryStream((int)stream.Length))
            {
                stream.CopyTo(ms);
                icon = _Image.FromBytes(ms.ToArray());
            }

            // push button only icon field
            rc = new RectangleF(rc.X + 1.5f * rc.Width, rc.Y, rc.Width / 2, rc.Height);
            PdfPushButton button3 = RenderPushButton("", fieldFont, rc, icon, ButtonLayout.ImageOnly);
            button3.Actions.MouseEnter.Add(new PdfPushButton.Action(ButtonAction.HideField, button1.Name));
            button3.Actions.MouseLeave.Add(new PdfPushButton.Action(ButtonAction.ShowField, button1.Name));
            button3.Actions.Released.Add(new PdfPushButton.Action(ButtonAction.CallMenu, "ShowGrid"));
            button3.BorderWidth = FieldBorderWidth.Medium;
            button3.BorderStyle = FieldBorderStyle.Beveled;
            button3.BorderColor = Color.Gray;

            // next page
            _c1pdf.NewPage();

            // text for next page
            rc = rcPage;
            RenderParagraph("Second page as bookmark", titleFont, rcPage, rc, false);

            // text box field
            //rc = rcPage;
            //rc = new RectangleF(rc.X, rc.Y + rc.Height / 10, rc.Width / 3, rc.Height / 30);
            //PdfTextBox textBox2 = RenderTextBox("TextSample 2", fieldFont, rc, Color.Yellow, "In 2 page");

            // second pass to number pages
            AddFooters();

            // save the document
            return "acroform.pdf";
        }

        // get the current page rectangle (depends on paper size)
        // and apply a 1" margin all around it.
        internal _Rect GetPageRect()
        {
            _Rect rcPage = _c1pdf.PageRectangle;
            rcPage.Inflate(-72, -72);
            return rcPage;
        }

        // add text box field for fields of the PDF document
        // with common parameters and default names.
        // 
        internal PdfTextBox RenderTextBox(string text, _Font font, _Rect rc, _Color back, string toolTip = null)
        {
            // create
            string name = string.Format("ACFTB{0}", _textBoxCount + 1);
            PdfTextBox textBox = new()
            {
                // default border
                //textBox.BorderWidth = 3f / 4;
                BorderStyle = FieldBorderStyle.Solid,
                BorderColor = SystemColors.ControlDarkDark,

                // parameters
                Font = font,
                Name = name,
                DefaultText = text,
                Text = text,
                ToolTip = string.IsNullOrEmpty(toolTip) ? string.Format("{0} ({1})", text, name) : toolTip
            };
            if (back != _Color.Transparent && !back.IsEmpty)
            {
                textBox.BackColor = back;
            }

            // add
            _c1pdf.AddField(textBox, rc);
            _textBoxCount++;

            // done
            return textBox;
        }
        internal PdfTextBox RenderTextBox(string text, _Font font, _Rect rc)
        {
            return RenderTextBox(text, font, rc, Color.Transparent, null);
        }

        // add check box field for fields of the PDF document
        // with common parameters and default names.
        // 
        internal PdfCheckBox RenderCheckBox(bool value, string text, _Font font, _Rect rc, _Color back, string toolTip = null)
        {
            // create
            string name = string.Format("ACFCB{0}", _checkBoxCount + 1);
            PdfCheckBox checkBox = new()
            {
                // default border
                BorderWidth = FieldBorderWidth.Thin,
                BorderStyle = FieldBorderStyle.Solid,
                BorderColor = SystemColors.ControlDarkDark,

                // parameters
                Name = name,
                DefaultValue = value,
                Value = value,
                ToolTip = string.IsNullOrEmpty(toolTip) ? string.Format("{0} ({1})", text, name) : toolTip
            };
            if (back != _Color.Transparent && !back.IsEmpty)
            {
                checkBox.BackColor = back;
            }

            // add
            float checkBoxSize = font.Size;
            float checkBoxTop = rc.Top + (rc.Height - checkBoxSize) / 2;
            _c1pdf.AddField(checkBox, new _Rect(rc.Left, checkBoxTop, checkBoxSize, checkBoxSize));
            _checkBoxCount++;

            // text for check box field
            float x = rc.Left + checkBoxSize + 1.0f;
            float y = rc.Top + (rc.Height - checkBoxSize - 1.0f) / 2;
            _c1pdf.DrawString(text, new _Font(font.Name, checkBoxSize, font.Style), _Color.Black, new _Point(x, y));

            // done
            return checkBox;
        }
        internal PdfCheckBox RenderCheckBox(bool value, string text, _Font font, _Rect rc)
        {
            return RenderCheckBox(value, text, font, rc, _Color.Transparent, null);
        }

        // add radio button box field for fields of the PDF document
        // with common parameters and default names.
        // 
        internal PdfRadioButton RenderRadioButton(bool value, string group, string text, _Font font, _Rect rc, Color back, string toolTip = null)
        {
            // create
            string name = string.IsNullOrEmpty(group) ? "ACFRGR" : group;
            PdfRadioButton radioButton = new()
            {
                // parameters
                Name = name,
                DefaultValue = value,
                Value = value,
                ToolTip = string.IsNullOrEmpty(toolTip) ? string.Format("{0} ({1})", text, name) : toolTip
            };
            if (back != _Color.Transparent && !back.IsEmpty)
            {
                radioButton.BackColor = back;
            }

            // add
            float radioSize = font.Size;
            float radioTop = rc.Top + (rc.Height - radioSize) / 2;
            _c1pdf.AddField(radioButton, new _Rect(rc.Left, radioTop, radioSize, radioSize));
            _radioButtonCount++;

            // text for radio button field
            float x = rc.Left + radioSize + 1.0f;
            float y = rc.Top + (rc.Height - radioSize - 1.0f) / 2;
            _c1pdf.DrawString(text, new _Font(font.Name, radioSize, font.Style), _Color.Black, new _Point(x, y));

            // done
            return radioButton;
        }
        internal PdfRadioButton RenderRadioButton(bool value, string group, string text, _Font font, _Rect rc)
        {
            return RenderRadioButton(value, group, text, font, rc, Color.Transparent, null);
        }

        // add combo box field for fields of the PDF document
        // with common parameters and default names.
        // 
        internal PdfComboBox RenderComboBox(string[] list, int activeIndex, _Font font, _Rect rc, _Color back, string toolTip = null)
        {
            // create
            string name = string.Format("ACFCLB{0}", _comboBoxCount + 1);
            PdfComboBox comboBox = new()
            {
                // default border
                BorderWidth = FieldBorderWidth.Thin,
                BorderStyle = FieldBorderStyle.Solid,
                BorderColor = SystemColors.ControlDarkDark,

                // parameters
                Font = font,
                Name = name,
                DefaultValue = activeIndex,
                Value = activeIndex
            };
            comboBox.ToolTip = string.IsNullOrEmpty(toolTip) ? string.Format("{0} ({1})", string.Format("Count = {0}", comboBox.Items.Count), name) : toolTip;
            if (back != _Color.Transparent && !back.IsEmpty)
            {
                comboBox.BackColor = back;
            }

            // array
            foreach (string text in list)
            {
                comboBox.Items.Add(text);
            }

            // add
            _c1pdf.AddField(comboBox, rc);
            _comboBoxCount++;

            // done
            return comboBox;
        }
        internal PdfComboBox RenderComboBox(string[] list, int activeIndex, _Font font, RectangleF rc)
        {
            return RenderComboBox(list, activeIndex, font, rc, Color.Transparent, null);
        }

        // add list box field for fields of the PDF document
        // with common parameters and default names.
        // 
        internal PdfListBox RenderListBox(string[] list, int activeIndex, _Font font, _Rect rc, _Color back, string toolTip = null)
        {
            // create
            string name = string.Format("ACFLB{0}", _listBoxCount + 1);
            PdfListBox listBox = new()
            {
                // default border
                BorderWidth = FieldBorderWidth.Thin,
                BorderStyle = FieldBorderStyle.Solid,
                BorderColor = SystemColors.ControlDarkDark,

                // parameters
                Font = font,
                Name = name,
                DefaultValue = activeIndex,
                Value = activeIndex
            };
            listBox.ToolTip = string.IsNullOrEmpty(toolTip) ? string.Format("{0} ({1})", string.Format("Count = {0}", listBox.Items.Count), name) : toolTip;
            if (back != _Color.Transparent && !back.IsEmpty)
            {
                listBox.BackColor = back;
            }

            // array
            foreach (string text in list)
            {
                listBox.Items.Add(text);
            }

            // add
            _c1pdf.AddField(listBox, rc);
            _listBoxCount++;

            // done
            return listBox;
        }
        internal PdfListBox RenderListBox(string[] list, int activeIndex, _Font font, _Rect rc)
        {
            return RenderListBox(list, activeIndex, font, rc, _Color.Transparent, null);
        }

        // add push button box field for fields of the PDF document
        // with common parameters and default names.
        // 
        internal PdfPushButton RenderPushButton(string text, _Font font, _Rect rc, _Color back, _Color fore, string toolTip = null, _Image image = null, ButtonLayout layout = ButtonLayout.TextOnly)
        {
            // create
            string name = string.Format("ACFPB{0}", _pushButtonCount + 1);
            PdfPushButton pushButton = new PdfPushButton();

            // parameters
            pushButton.Name = name;
            pushButton.DefaultValue = text;
            pushButton.Value = text;
            pushButton.Font = font;
            pushButton.ToolTip = string.IsNullOrEmpty(toolTip) ? string.Format("{0} ({1})", text, name) : toolTip;
            if (back != _Color.Transparent && !back.IsEmpty)
            {
                pushButton.BackColor = back;
            }
            if (fore != _Color.Transparent && !fore.IsEmpty)
            {
                pushButton.ForeColor = fore;
            }

            // icon
            if (image != null)
            {
                pushButton.Image = image;
                pushButton.Layout = layout;
            }

            // add
            _c1pdf.AddField(pushButton, rc);
            _pushButtonCount++;

            // done
            return pushButton;
        }
        internal PdfPushButton RenderPushButton(string text, Font font, RectangleF rc, Color back)
        {
            return RenderPushButton(text, font, rc, back, _Color.Transparent);
        }
        internal PdfPushButton RenderPushButton(string text, Font font, RectangleF rc)
        {
            return RenderPushButton(text, font, rc, _Color.Transparent, _Color.Transparent);
        }
        internal PdfPushButton RenderPushButton(string text, _Font font, _Rect rc, _Image image, ButtonLayout layout = ButtonLayout.ImageOnly)
        {
            return RenderPushButton(text, font, rc, _Color.Transparent, _Color.Transparent, null, image, layout);
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
            _Font fontHorz = new _Font("Tahoma", 7, _FontStyle.Bold);
            _Font fontVert = new _Font("Viner Hand ITC", 14, _FontStyle.Bold);

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

        static Stream GetManifestResource(string resource)
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
