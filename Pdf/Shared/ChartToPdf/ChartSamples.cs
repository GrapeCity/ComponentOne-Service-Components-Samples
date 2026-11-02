using C1.Chart;
using C1.Chart.Standard;
using C1.Diagram;
using C1.Diagram.Standard;
using C1.Excel;
using C1.Pdf;
using C1.Word;
using GrapeCity.Documents.Common;
using GrapeCity.Documents.Drawing;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;

namespace ChartToPdf
{
    static class ChartSamples
    {
        static Random rnd = new Random(1);
        static int rand() => rnd.Next(10, 100);

        public static FlexChart CreateChart()
        {
            var chart = new FlexChart();
            SetHeader(chart);

            chart.AxisX.AxisLine = false;
            chart.Footer.Content = "GDP in Trillions of USD (Nominal, Current Prices)";
            chart.AxisY.MajorTickMarks = TickMark.None;

            chart.Legend.Position = Position.Bottom;

            chart.BindingX = "Year";
            chart.DataSource = GdpDataSource.Data;

            chart.Series.Add(new Series() { Binding = "USA", Name = "USA" });
            chart.Series.Add(new Series() { Binding = "China", Name = "China" });

            return chart;
        }

        public static FlexChart CreateLineChart()
        {
            var chart = new FlexChart() { ChartType = ChartType.Spline };
            SetHeader(chart);
            chart.Binding = "Y";
            chart.BindingX = "X";

            chart.AxisY.Min = -1;
            chart.AxisY.Max = 1;
            chart.AxisY.AxisLine = true;

            chart.AxisX.MajorGrid = true;
            chart.AxisX.Format = "0π";

            var pointsSin = new List<PointF>();
            var pointsCos = new List<PointF>();
            for (var i = 0; i < 41; i++)
            {
                pointsSin.Add(new PointF(0.1f * i, MathF.Sin(0.1f * i * MathF.PI)));
                pointsCos.Add(new PointF(0.1f * i, MathF.Cos(0.1f * i * MathF.PI)));
            }
            var series1 = new Series() { Name = "y = sin(x)", DataSource = pointsSin };
            var series2 = new Series() { Name = "y = cos(x)", DataSource = pointsCos };

            chart.Series.Add(series1);
            chart.Series.Add(series2);

            return chart;
        }

        public static FlexPie CreatePieChart()
        {
            var pieChart = new FlexPie();
            SetHeader(pieChart);

            var data = new List<object>()
            {
                new { Name = "USA", Value = 30.51m },
                new { Name = "China", Value = 19.23m },
                new { Name = "Rest", Value = 69.35m },
            };

            pieChart.DataSource = data;
            pieChart.Binding = "Value";
            pieChart.BindingName = "Name";

            pieChart.Footer.Content = "2025 GDP in Trillions of USD (IMF projection)";

            return pieChart;
        }

        public static FlexRadar CreateRadarChart()
        {
            var chart = new FlexRadar();
            SetHeader(chart);

            chart.ChartType = RadarChartType.LineSymbols;
            chart.Binding = "Value";
            chart.BindingX = "Name";

            chart.AxisY.Min = 60;
            chart.AxisY.Max = 100;
            chart.AxisY.AxisLine = true;

            var data1 = new List<object>()
            { 
                new { Name = "Camera", Value = 95 },
                new { Name = "Battery", Value = 82 },
                new { Name = "Display", Value = 94 },
                new { Name = "Performance", Value = 96 },
                new { Name = "Value", Value = 72 },
                new { Name = "Build Quality", Value = 95 }
            };

            var ser1 = new RadarSeries() { Name = "iPhone 16 Pro", DataSource = data1 };
            chart.Series.Add(ser1);

            var data2 = new List<object>()
            {
                new { Name = "Camera", Value = 96 },
                new { Name = "Battery", Value = 80 },
                new { Name = "Display", Value = 90 },
                new { Name = "Performance", Value = 88 },
                new { Name = "Value", Value = 85 },
                new { Name = "Build Quality", Value = 88 }
            };

            var ser2 = new RadarSeries() { Name = "Google Pixel 9 Pro", DataSource = data2 };
            chart.Series.Add(ser2);

            return chart;
        }

        public static FlexRadar CreatePolarChart()
        {
            var chart = new FlexRadar();
            SetHeader(chart);

            chart.ChartType = RadarChartType.Area;
            chart.Binding = "Y";
            chart.BindingX = "X";

            chart.AxisY.Labels = false;
            chart.AxisY.MajorTickMarks = TickMark.None;
            chart.AxisX.MajorUnit = 90;
            chart.AxisX.Format = "0°";

            chart.AxisY.Labels = false;
            chart.AxisY.MajorTickMarks = TickMark.None;
            chart.AxisY.MajorUnit = 0.25;

            var points = new PointF[360];

            for (var i = 0; i < points.Length; i++)
                points[i] = new PointF(i, MathF.Cos(7 * i * MathF.PI / 180f));
            chart.Series.Add( new RadarSeries() { DataSource = points });
            chart.Footer.Content = "Polar Coordinates: r(θ) = cos(7θ)";

            return chart;
        }


        public static TreeMap CreateTreeMap()
        {
            var treeMap = new TreeMap() { Binding = "sales", BindingName = "type", ChildItemsPath = "items" };
            SetHeader(treeMap);
            treeMap.DataSource = CreateHierarchicalData();
            treeMap.DataLabel.Content = "{type}";

            return treeMap;
        }

        public static Sunburst CreateSunburst()
        {
            var sunburst = new Sunburst()
            {
                Binding = "sales",
                BindingName = "type",
                ChildItemsPath = "items"
            };
            SetHeader(sunburst);
            sunburst.DataLabel.Position = PieLabelPosition.Center;
            sunburst.DataLabel.Content = "{type}";
            sunburst.DataSource = CreateHierarchicalData();

            return sunburst;
        }

        public static FlexDiagram CreateFlexDiagram()
        {
            var diagram = new FlexDiagram();
            SetHeader(diagram);

            diagram.Nodes.Add(new Node() { Text = "🛢 Data", Content = "Nodes & Edges" });
            diagram.Nodes.Add(new Node() { Text = "✨ Layout", Content = "MSAGL - Microsoft Automatic Graph Layout" });
            diagram.Nodes.Add(new Node() { Text = "🎨 Graphics", Content = "Cross-platform graphics engine" });
            diagram.Nodes.Add(new Node() { Text = "⑆ FlexDiagram", Content = "Flow Chart, Organizational Chart, etc." });

            diagram.Edges.Add(new Edge() { Source = diagram.Nodes[0], Target = diagram.Nodes[1], TargetArrow = ArrowStyle.Normal });
            diagram.Edges.Add(new Edge() { Source = diagram.Nodes[1], Target = diagram.Nodes[2], SourceArrow = ArrowStyle.Normal, TargetArrow = ArrowStyle.Normal });
            diagram.Edges.Add(new Edge() { Source = diagram.Nodes[2], Target = diagram.Nodes[3], TargetArrow = ArrowStyle.Normal });

            var style = new ChartStyle() { Font = new C1.Chart.Standard.Font() { FontName = "Calibri", FontSize = 24 } };

            foreach (var node in diagram.Nodes)
            {
                node.TitleStyle = style;
                node.Shape = Shape.RoundedRectangle;
            }

            return diagram;
        }

        static void SetHeader(FlexChartBase chart)
        {
            chart.Header.Content = chart.GetType().Name;
            chart.Header.HorizontalAlignment = HorizontalAlignment.Left;
        }

        public record GdpData(int Year, decimal USA, decimal China, decimal RestOfWorld);

        static class GdpDataSource
        {
            // GDP in Trillions of USD (Nominal, Current Prices)
            public static readonly GdpData[] Data =
            [
                new(1980,  2.86m,  0.19m,  8.37m),
                new(1985,  4.34m,  0.31m,  8.37m),
                new(1990,  5.96m,  0.36m, 16.50m),
                new(1995,  7.64m,  0.74m, 22.76m),
                new(2000, 10.25m,  1.22m, 22.37m),
                new(2005, 13.04m,  2.32m, 32.40m),
                new(2010, 15.05m,  6.19m, 45.27m),
                new(2015, 18.30m, 11.28m, 45.78m),
                new(2020, 21.35m, 15.00m, 49.23m),
                new(2025, 30.51m, 19.23m, 69.35m),  // IMF projection
            ];

            public static decimal GetWorldTotal(GdpData d) => d.USA + d.China + d.RestOfWorld;
        }


        public static object[] CreateHierarchicalData()
        {
            var data = new object[] {
                    new {
                        type = "Electronics",
                        items = new object[] {
                            new {
                                type = "Camera",
                                sales = rand()
                            },
                            new {
                                type = "Headphones",
                                sales = rand()
                            }
                        }
                    },
                    new {
                        type = "Computers",
                        items = new object[]
                        {
                            new
                            {
                                type = "Desktops",
                                sales = rand()
                            },
                            new
                            {
                                type = "Laptops",
                                sales = rand()
                            }
                        }
                    }
                };
            return data;
        }

        internal static void RenderCharts(C1.Util.IFlowDocument flow, FlexChartBase[] charts, SizeF rcPage)
        {
            // get ready to work
            DateTime bs = DateTime.Now;

            // initialization
            var offset = 72f;
            var chartWidth = rcPage.Width - 2 * offset;
            var chartHeight = (rcPage.Height - 2.5f * offset) / 2;
            var x = offset;

            // render charts
            foreach (var chart in charts)
            {
                if ((x + chartHeight) > rcPage.Height)
                {
                    // new page
                    flow.NewPage();
                    x = offset;
                }
                chart.Render(flow, offset, x, chartWidth, chartHeight);
                x += chartHeight + 0.5f * offset;
            }

            // done
            var txt = (flow is C1PdfDocument) ? "C1Pdf" : "C1Word";
            TimeSpan ts = DateTime.Now.Subtract(bs);
            Console.WriteLine("for {0} done in {1:f2}s", txt, ts.TotalSeconds);
        }


        // save current document and show it in Adobe Acrobat
        internal static void SaveAndShow(C1.Util.IFlowDocument flow, string fileName)
        {
            try
            {
                flow.Save(fileName);
                var psi = new ProcessStartInfo(fileName)
                {
                    Verb = "open",
                    FileName = fileName,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch
            {
                Console.WriteLine("Can't save, make sure the document is not open.");
            }
        }

        // show it in Adobe Acrobat
        internal static void Show(string fileName)
        {
            try
            {
                var psi = new ProcessStartInfo(fileName)
                {
                    Verb = "open",
                    FileName = fileName,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch
            {
                Console.WriteLine("Can't save, make sure the document is not open.");
            }
        }

        internal static void ChartsToHtml(string path, FlexChartBase[] charts, SizeF paper, bool svg = true, bool fast = true)
        {
            // Get ready to work
            DateTime bs = DateTime.Now;

            // Convert to pixels
            int wpx = C1XLBook.TwipsToPixels(20 * (int)paper.Width);
            int hpx = C1XLBook.TwipsToPixels(20 * (int)paper.Height);

            // Output file stream
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = Encoding.UTF8;
                settings.Indent = true;
                var doc = XmlWriter.Create(fs, settings);

                // Create new XHTML document with body and head tags
                doc.WriteStartElement("html");
                doc.WriteStartElement("head");

                // Add title from file name
                doc.WriteElementString("title", "Charts");

                // Add UTF-8 encoding
                doc.WriteStartElement("meta");
                doc.WriteAttributeString("charset", "utf-8");
                doc.WriteEndElement();

                // Styles
                doc.WriteStartElement("style");
                doc.WriteAttributeString("type", "text/css");
                doc.WriteString(GetResourceText("Result.css")); // Make sure you have this method implemented
                doc.WriteEndElement();

                // Close head
                doc.WriteEndElement();

                // Add body
                doc.WriteStartElement("body");

                // Add content element
                doc.WriteStartElement("div");
                doc.WriteAttributeString("id", "content");

                // Initialization
                var offset = 72f;
                var chartWidth = paper.Width - 2 * offset;
                var chartHeight = (paper.Height - 2.5f * offset) / 2;
                var rc = new Rectangle(0, 0, (int)paper.Width, (int)paper.Height);

                // Render charts
                int count = 0;
                for (int i = 0; i < charts.Length; i += 2)
                {
                    // Render interface
                    var y = offset;
                    var r = new Rectangle(0, 0, wpx, hpx);
                    C1.Util.IRendering gr = svg
                        ? (fast ? new C1.Util.SvgRendering(wpx, hpx) : new C1.Util.SvgGraphicsRendering(wpx, hpx))
                        : new C1.Util.BitmapGraphicsRendering(wpx, hpx);

                    // Clear bitmaps
                    if (svg)
                    {
                        gr.SetFill(Color.White);
                        gr.DrawRect(r.X, r.Y, r.Width, r.Height);
                    }

                    // First chart
                    charts[i].Render(gr, offset, y, chartWidth, chartHeight);
                    y += chartHeight + 0.5f * offset;

                    // Second chart
                    if (i + 1 < charts.Length)
                    {
                        charts[i + 1].Render(gr, offset, y, chartWidth, chartHeight);
                        y += chartHeight + 0.5f * offset;
                    }

                    // Section is created for page content
                    doc.WriteStartElement("section");

                    // Set page ID
                    doc.WriteAttributeString("id", $"page{1 + count}");
                    count++;

                    // Make page-like view
                    doc.WriteAttributeString("style", $"width:{wpx}px;height:{hpx}px;");

                    // Save image
                    if (svg)
                    {
                        string txt;
                        if (fast)
                        {
                            txt = ((C1.Util.SvgRendering)gr).ToText();
                        }
                        else
                        {
                            var sb = new StringBuilder();
                            var svgDocument = ((C1.Util.SvgGraphicsRendering)gr).ToDocument();
                            svgDocument.Save(sb, settings);
                            txt = sb.ToString();
                        }
                        int idx = txt.IndexOf("<svg");
                        if (idx > 0)
                        {
                            txt = txt.Substring(idx);
                            txt = txt.Replace("id=\"clipPath", $"id=\"clipPath{count}_");
                            txt = txt.Replace("url(#clipPath", $"url(#clipPath{count}_");
                        }
                        doc.WriteRaw(txt);
                    }
                    else
                    {
                        doc.WriteStartElement("img");
                        using (var ms = new MemoryStream())
                        {
                            string result;
                            if (fast)
                            {
                                ((C1.Util.BitmapGraphicsRendering)gr).Bitmap.SaveAsJpeg(ms);
                                result = "jpeg";
                            }
                            else
                            {
                                ((C1.Util.BitmapGraphicsRendering)gr).Bitmap.SaveAsPng(ms);
                                result = "png";
                            }
                            doc.WriteAttributeString("src", $"data:image/{result};base64,{Convert.ToBase64String(ms.ToArray())}");
                        }
                        doc.WriteAttributeString("alt", $"Page {1 + count}");
                        doc.WriteEndElement();
                    }

                    // Disposing
                    ((IDisposable)gr).Dispose();

                    // Close tags
                    doc.WriteEndElement(); // Section
                }

                doc.WriteEndElement(); // Div#content

                doc.WriteEndElement(); // Body
                doc.WriteEndElement(); // Html

                // Save HTML document
                doc.Flush();
            }

            // Done
            TimeSpan ts = DateTime.Now.Subtract(bs);
            Console.WriteLine("for {0} done in {1:f2}s", path, ts.TotalSeconds);
        }

        /// <summary>
        /// Retrieves text from an embedded resource
        /// </summary>
        /// <param name="resource">The name of the resource</param>
        /// <returns>The text content of the resource</returns>
        internal static string GetResourceText(string resource)
        {
            resource = resource.ToLower();
            var assembly = Assembly.GetExecutingAssembly();
            foreach (string res in assembly.GetManifestResourceNames())
            {
                if (res.ToLower().EndsWith(resource))
                {
                    var stream = assembly.GetManifestResourceStream(res);
                    if (stream != null)
                    {
                        using (stream)
                        {
                            var reader = new StreamReader(stream, Encoding.UTF8, true);
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}
