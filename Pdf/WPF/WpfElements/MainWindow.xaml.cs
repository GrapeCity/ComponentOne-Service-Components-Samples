using C1.Win.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfElements
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PDF_Click(object sender, RoutedEventArgs e)
        {
            C1PdfDocument pdf = new C1PdfDocument(PaperKind.A4, true);
            pdf.Compression = C1.Pdf.CompressionLevel.NoCompression;
            pdf.DrawPreferences = DrawElementPreferences.Vectors;

            var r = pdf.PageRectangle;
            var rect = new Rect(r.X, r.Y, r.Width, r.Height);

            var pts = new Point[5];
            pts[0] = new Point(rect.Left + 100, rect.Top + 100);
            pts[1] = new Point(rect.Right - 100, rect.Top + 100);
            pts[2] = new Point(rect.Right - 100, rect.Bottom - 100);
            pts[3] = new Point(rect.Left + 100, rect.Bottom - 100);
            pts[4] = new Point(rect.Left + 100, rect.Top + 100);
            var types = new byte[5];
            types[0] = 0x00;
            types[1] = 0x01;
            types[2] = 0x01;
            types[3] = 0x01;
            types[4] = 0x00;
            pdf.FillPath(Color.FromArgb(0xff, 0xe4, 0x93, 0x56), pts, types, true);

            //rect.in
            //pdf.DrawPreferences = DrawElementPreferences.Pixels;
            pdf.DrawElement(cnv, rect, C1.Util.ContentAlignment.MiddleCenter, Stretch.None);

            //string pdffile = @"c:\temp\test.pdf";
            string pdffile = GetResult("Results/TestDocument.pdf");

            try
            {
                //PDFファイルを保存
                pdf.Save(pdffile);
                //開く
                System.Diagnostics.Process.Start
                    (new System.Diagnostics.ProcessStartInfo(pdffile) { UseShellExecute = true });

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        string GetResult(string path)
        {
            var a = Assembly.GetExecutingAssembly();
            var dir = System.IO.Path.GetDirectoryName(a.Location);
            var folder = System.IO.Path.GetDirectoryName(a.Location);
            return System.IO.Path.Combine(dir ?? string.Empty, path);
        }
    }
}
