using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Globalization;
using System.Diagnostics;

using C1.Excel;
using GrapeCity.Documents.Common;

namespace ExcelFormulas
{
    class Program
    {
        static void Save(C1XLBook wb, string fileName, bool preview = false)
        {
            if (wb != null)
            {
                var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var path = Path.Combine(dir, fileName);
                wb.Save(path);
                if (preview)
                {
                    Process.Start(new ProcessStartInfo { FileName = path, UseShellExecute = true });
                }
            }
        }

        // read print settings from sheet, show in form
        static void ShowPrintSettings(XLSheet sheet)
        {
            XLPrintSettings ps = sheet.PrintSettings;

            // paper size, orientation
            Console.Write("PAPER KIND: ");
            Console.WriteLine(((PaperKind)ps.PaperKind).ToString());
            Console.Write("LANDSCAPE: ");
            Console.WriteLine(ps.Landscape);

            // scaling
            Console.Write("SCALING: ");
            Console.Write("AUTO SCALE: ");
            Console.WriteLine(ps.AutoScale);
            Console.Write("FIT PAGE ACROSS: ");
            Console.WriteLine(ps.FitPagesAcross);
            Console.Write("FIT PAGE DOWN: ");
            Console.WriteLine(ps.FitPagesDown);
            Console.Write("SCALING FACTOR (%): ");
            Console.WriteLine(ps.ScalingFactor);

            // start page
            Console.Write("START PAGE: ");
            Console.WriteLine(ps.StartPage);

            // margins
            Console.Write("MARGINS: ");
            Console.Write("LEFT: ");
            Console.WriteLine(ps.MarginLeft);
            Console.Write("TOP: ");
            Console.WriteLine(ps.MarginTop);
            Console.Write("RIGHT: ");
            Console.WriteLine(ps.MarginRight);
            Console.Write("BOTTOM: ");
            Console.WriteLine(ps.MarginBottom);
            Console.Write("HEADER: ");
            Console.WriteLine(ps.MarginHeader);
            Console.Write("FOOTER: ");
            Console.WriteLine(ps.MarginFooter);

            // header/footer
            Console.Write("HEADER: ");
            Console.WriteLine(ps.Header);
            Console.Write("FOOTER: ");
            Console.WriteLine(ps.Footer);
        }

        static C1XLBook CreateSample()
        {
            // create C1XLBook
            C1XLBook wb = new C1XLBook();

            XLSheet ws = wb.Sheets[0];
            XLPrintSettings ps = ws.PrintSettings;

            // paper size, orientation
            ps.PaperKind = (short)PaperKind.B5;
            ps.Landscape = true;

            // scaling
            // ** note: 
            //    setting the FitPagesAcross, FitPagesDown, and ScalingFactor properties
            //    changes the value of AutoScale, so set AutoScale last.
            ps.FitPagesAcross = 1;
            ps.FitPagesDown = 1;
            ps.ScalingFactor = 100;
            ps.AutoScale = true;

            // start page
            ps.StartPage = 1;

            // margins
            ps.MarginLeft = 1.0;
            ps.MarginTop = 1.0;
            ps.MarginRight = 1.0;
            ps.MarginBottom = 1.0;
            ps.MarginHeader = 0.5;
            ps.MarginFooter = 0.5;

            // header/footer
            ps.Header = "Start";
            ps.Footer = "";

            // column width in twips
            ws.Columns[0].Width = 2000;

            // string
            ws[0, 0].Value = "Print setting test";

            // done
            return wb;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Excel print settings sample...");
            if (args.Length == 0)
            {
                var book = CreateSample();
                Save(book, "test.xls", false);
                book.Clear();
                book.Load("test.xls");
                ShowPrintSettings(book.Sheets[0]);
            }
            else
            {
                foreach (var item in args)
                {
                    if (File.Exists(item))
                    {
                        var book = new C1XLBook();
                        book.Load(item);
                        foreach (XLSheet sheet in book.Sheets)
                        {
                            ShowPrintSettings(sheet);
                        }
                        Process.Start(new ProcessStartInfo { FileName = item, UseShellExecute = true });
                    }
                }
            }
            Console.WriteLine("Excel test files created.");
        }
    }
}
