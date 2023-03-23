using System;
using System.IO;
using System.Drawing;
using System.Globalization;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Reflection;

using C1.Excel;
using Microsoft.Win32;

using _Size = System.Drawing.Size;
using _Image = GrapeCity.Documents.Drawing.Image;

namespace ExcelWpf
{
    /// <summary>
    /// Interaction logic for ExcelCreator.xaml
    /// </summary>
    public partial class ExcelCreator : UserControl
    {
        public ExcelCreator()
        {
            InitializeComponent();
        }

        private void btnHelloWorld_Click(object sender, RoutedEventArgs e)
        {
            SaveBook(book =>
            {
                book.Sheets[0][0, 0].Value = "Hello World";
            });
        }

        private void btnStyles_Click(object sender, RoutedEventArgs e)
        {
            SaveBook(book =>
            {
                // get the sheet that was created by default, give it a name
                var sheet = book.Sheets[0];

                // create styles for odd and even values
                var styleOdd = new XLStyle(book);
                styleOdd.Font = new XLFont("Tahoma", 9, false, true);
                styleOdd.ForeColor = Color.Blue;

                var styleEven = new XLStyle(book);
                styleEven.Font = new XLFont("Tahoma", 9, true, false);
                styleEven.ForeColor = Color.Red;
                styleEven.BackColor = Color.Yellow;

                // step 3: write content and format into some cells
                for (int i = 0; i < 30; i++)
                {
                    XLCell cell = sheet[i, 0];
                    cell.Value = i + 1;
                    cell.Style = ((i + 1) % 2 == 0) ? styleEven : styleOdd;
                }
            });
        }

        private void btnFormulas_Click(object sender, RoutedEventArgs e)
        {
            SaveBook(book =>
            {
                // first sheet
                var sheet = book.Sheets[0];

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

                var style = new XLStyle(book);
                var dtfi = CultureInfo.CurrentCulture.DateTimeFormat;
                style.Format = XLStyle.FormatDotNetToXL(dtfi.ShortDatePattern + " " + dtfi.ShortTimePattern);
                sheet[12, 1].Style = style;
            });
        }

        private void btnPictures_Click(object sender, RoutedEventArgs e)
        {
            SaveBook(book =>
            {
                // three methods add images to Excel file
                XLPictureShape picture;

                // create images
                _Image spbImage = _Image.FromStream(GetManifestResource("spb.jpg"));
                _Image canadaImage = _Image.FromStream(GetManifestResource("canada.bmp"));
                _Image googleImage = _Image.FromStream(GetManifestResource("google.bmp"));
                _Image babyImage = _Image.FromStream(GetManifestResource("baby.png"));

                /////////////////////////////////////////////////////////
                // List "Images" -- three methods add images
                /////////////////////////////////////////////////////////
                XLSheet sheet = book.Sheets[0];
                sheet.Name = "Images";
                sheet.PrintSettings.Header = "&LCanada:&G&D&CHeader Center&R&P";
                sheet.PrintSettings.Footer = "&C&Z";

                // first method
                picture = new XLPictureShape(googleImage, 0, 0, 2200, 5000);
                picture.DashedLineStyle = XLShapeDashedLineStyle.Solid;
                picture.LineStyle = XLShapeLineStyle.Simple;
                picture.LineColor = Color.BlueViolet;
                picture.Rotation = 90.0f;
                picture.LineWidth = 10;
                sheet[1, 7].Value = picture;
                sheet[1, 1].Value = "C1Excel pictures";

                // second method
                picture = new XLPictureShape(spbImage, 100, 3000, 8000, 6000);
                picture.Brightness = .55f;
                picture.Contrast = .5f;
                sheet.Shapes.Add(picture);

                picture = new XLPictureShape(canadaImage);
                picture.LineColor = Color.Aqua;
                picture.LineWidth = 100;
                sheet.PrintSettings.HeaderPictureLeft = picture;

                // third method
                picture = new XLPictureShape(babyImage);
                picture.Rotation = 30.0f;
                picture.LineColor = Color.Aqua;
                picture.LineWidth = 100;
                sheet[15, 2].Value = picture;

                /////////////////////////////////////////////////////////
                // List "Types" -- support image types (bmp, png, jpg, emf)
                /////////////////////////////////////////////////////////
                sheet = book.Sheets.Add("Types");
                sheet[1, 0].Value = "Bmp:";
                sheet[1, 1].Value = googleImage;
                sheet[8, 0].Value = "Png:";
                sheet[8, 1].Value = babyImage;
                sheet[25, 0].Value = "Jpeg:";
                sheet[25, 1].Value = spbImage;

                /////////////////////////////////////////////////////////
                // List "Borders" -- various picture borders
                /////////////////////////////////////////////////////////
                sheet = book.Sheets.Add("Borders");
                int row = 1, col = 0;
                foreach (XLShapeLineStyle style in Enum.GetValues(typeof(XLShapeLineStyle)))
                {
                    col = 1;
                    sheet.Rows[row].Height = 3700;

                    foreach (XLShapeDashedLineStyle dashedStyle in Enum.GetValues(typeof(XLShapeDashedLineStyle)))
                    {
                        sheet.Columns[col].Width = 2300;
                        picture = new XLPictureShape(babyImage);
                        picture.LineWidth = 100;
                        picture.LineColor = Color.FromArgb(100, 200, Math.Min(col * 12, 255));
                        picture.DashedLineStyle = dashedStyle;
                        picture.LineStyle = style;
                        sheet[row, col].Value = picture;
                        sheet[row + 1, col].Value = "style: " + style.ToString();
                        sheet[row + 2, col].Value = "dashed: " + dashedStyle.ToString();

                        col += 2;
                    }

                    row += 4;
                }

                /////////////////////////////////////////////////////////
                // List "Alignment" -- position image using ContentAlignment
                /////////////////////////////////////////////////////////
                sheet = book.Sheets.Add("Alignment");
                row = 1;
                sheet.Columns[1].Width = sheet.Columns[4].Width = 6000;
                sheet.Columns[7].Width = sheet.Columns[10].Width = 2000;
                foreach (XLAlignVert alignVert in Enum.GetValues(typeof(XLAlignVert)))
                {
                    foreach (XLAlignHorz alignHorz in Enum.GetValues(typeof(XLAlignHorz)))
                    {
                        sheet.Rows[row].Height = 2400;

                        var cellSize = new _Size(sheet.Columns[1].Width, sheet.Rows[row].Height);
                        picture = new XLPictureShape(googleImage, cellSize, alignHorz, alignVert, ImageScaling.Clip);
                        sheet[row, 1].Value = picture;
                        sheet[row, 2].Value = $"clip: {alignHorz}-{alignVert}";

                        picture = new XLPictureShape(googleImage, cellSize, alignHorz, alignVert, ImageScaling.Scale);
                        sheet[row, 4].Value = picture;
                        sheet[row, 5].Value = $"scale: {alignHorz}-{alignVert}";

                        row += 4;
                    }
                }

                /////////////////////////////////////////////////////////
                // List "Properties" -- various picture properties
                /////////////////////////////////////////////////////////
                sheet = book.Sheets.Add("Properties");

                // associating hyperlink with the shape
                sheet.Rows[1].Height = 2000;
                sheet.Columns[1].Width = 3600;
                picture = new XLPictureShape(spbImage);
                picture.Hyperlink = "http://www.spb.ru/";
                sheet[1, 1].Value = picture;
                sheet[2, 1].Value = "hyperlink (click on the picture)";

                // others view type
                col = 1;
                sheet.Rows[4].Height = 2000;
                foreach (XLPictureViewType viewType in Enum.GetValues(typeof(XLPictureViewType)))
                {
                    sheet.Columns[col].Width = 3600;
                    picture = new XLPictureShape(spbImage);
                    picture.ViewType = viewType;
                    sheet[4, col].Value = picture;
                    sheet[5, col].Value = "view type: " + viewType.ToString();
                    ;

                    col += 2;
                }

                // brightness & contrast
                col = 1;
                sheet.Rows[7].Height = sheet.Rows[10].Height = 2000;
                for (int i = 0; i <= 100; i += 10)
                {
                    sheet.Columns[col].Width = 3600;
                    picture = new XLPictureShape(spbImage);
                    picture.Brightness = (float)i / 100;
                    sheet[7, col].Value = picture;
                    sheet[8, col].Value = string.Format("brightness: {0}", picture.Brightness);

                    picture = new XLPictureShape(spbImage);
                    picture.Contrast = (float)i / 100;
                    sheet[10, col].Value = picture;
                    sheet[11, col].Value = string.Format("contrast: {0}", picture.Contrast);

                    col += 2;
                }
            });
        }

        private void SaveBook(Action<C1XLBook> action)
        {
            if (!IsPermissionGranted(new FileIOPermission(PermissionState.Unrestricted)))
            {
                string msg = "There is no permission to save file.";
                if (Application.Current == null)
                {
                    MessageBox.Show(msg, "C1.Excel", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    MessageBox.Show(msg, Application.Current.MainWindow.Title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else
            {
                InternalSave(action);
            }
        }

        private static void InternalSave(Action<C1XLBook> action)
        {

            var dlg = new SaveFileDialog();
            dlg.Filter = "Open XML Excel Files (*.xlsx)|*.xlsx|Binary Excel Files (*.xls)|*.xls|Comma Separated Values Files (*.csv)|*.csv";
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    var book = new C1XLBook();
                    if (action != null)
                    {
                        action(book);
                    }
                    var fileName = dlg.FileName;
                    using (var stream = dlg.OpenFile())
                    {
                        book.Save(stream, GetFileFormat(fileName));
                    }
                    Process.Start(new ProcessStartInfo { FileName = fileName, UseShellExecute = true });
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                }
            }
        }

        internal static FileFormat GetFileFormat(string path)
        {
            switch (Path.GetExtension(path))
            {
                case ".csv":
                    return FileFormat.Csv;
                case ".xls":
                    return FileFormat.Biff8;
                case ".xlsx":
                case ".xlmx":
                    return FileFormat.OpenXml;
            }
            return FileFormat.OpenXml;
        }

        internal static bool IsPermissionGranted(CodeAccessPermission requestedPermission)
        {
            try
            {
                // Try and get this permission
                requestedPermission.Demand();
                return true;
            }
            catch
            {
                return false;
            }
        }

        internal static Stream GetManifestResource(string resource)
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
    }
}
