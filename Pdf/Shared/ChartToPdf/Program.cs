using C1.Chart.Standard;
using C1.Pdf;
using C1.Word;
using ChartToPdf;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

var charts = new FlexChartBase[] {
    ChartSamples.CreateChart(),
    ChartSamples.CreateLineChart(),
    ChartSamples.CreatePieChart(),
    ChartSamples.CreateRadarChart(),
    ChartSamples.CreatePolarChart(),
    ChartSamples.CreateTreeMap(),
    ChartSamples.CreateSunburst(),
    ChartSamples.CreateFlexDiagram()
};

ChartSamples.ChartsToHtml("BitmapCharts.html", charts, new SizeF(595.3f, 841.9f), false, false);
ChartSamples.Show("BitmapCharts.html");
ChartSamples.ChartsToHtml("SvgCharts.html", charts, new SizeF(595.3f, 841.9f), true, false);
ChartSamples.Show("SvgCharts.html");
ChartSamples.ChartsToHtml("FastSvgCharts.html", charts, new SizeF(595.3f, 841.9f), true, true);
ChartSamples.Show("FastSvgCharts.html");

using (var doc = new C1PdfDocument())
{
    ChartSamples.RenderCharts(doc, charts, doc.PageSize);
    ChartSamples.SaveAndShow(doc, "Charts.pdf");
}

using (var doc = new C1WordDocument())
{
    doc.TopMargin = doc.TopMargin / 2;
    doc.LeftMargin = doc.LeftMargin / 2;
    ChartSamples.RenderCharts(doc, charts, doc.PageSize);
    ChartSamples.SaveAndShow(doc, "Charts.docx");
    ChartSamples.SaveAndShow(doc, "Charts.rtf");
}

Console.WriteLine("Done");
