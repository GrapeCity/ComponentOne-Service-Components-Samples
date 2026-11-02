using C1.Report;
using C1.Document.Export;

namespace ExportReport
{
    /// <summary>
    /// Exports a loaded <see cref="FlexReport"/> to a file using the chosen format.
    /// </summary>
    internal class ReportExporter
    {
        /// <summary>
        /// Exports <paramref name="report"/> to <paramref name="outputPath"/> in the given <paramref name="format"/>.
        /// </summary>
        public void Export(FlexReport report, string outputPath, ExportFormat format)
        {
            var outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            var filter = CreateFilter(outputPath, format);
            report.RenderToFilter(filter);
        }

        /// <summary>
        /// Creates the appropriate <see cref="ExportFilter"/> for the given format.
        /// </summary>
        private ExportFilter CreateFilter(string outputPath, ExportFormat format)
        {
            return format switch
            {
                ExportFormat.Pdf => new PdfFilter { FileName = outputPath },
                ExportFormat.Excel => new XlsFilter { FileName = outputPath },
                ExportFormat.Html => new HtmlFilter { FileName = outputPath },
                ExportFormat.Rtf => new RtfFilter { FileName = outputPath },
                ExportFormat.Docx => new GsWordFilter { FileName = outputPath },
                _ => throw new ArgumentOutOfRangeException(nameof(format))
            };
        }
    }

}
