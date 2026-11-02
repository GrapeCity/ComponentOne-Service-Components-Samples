namespace ExportReport
{
    /// <summary>
    /// Supported export formats for the report.
    /// </summary>
    internal enum ExportFormat
    {
        Pdf,
        Excel,
        Html,
        Rtf,
        Docx
    }

    internal static class ExportFormatExtensions
    {
        public static string ToFileExtension(this ExportFormat format) => format switch
        {
            ExportFormat.Pdf => ".pdf",
            ExportFormat.Excel => ".xlsx",
            ExportFormat.Html => ".html",
            ExportFormat.Rtf => ".rtf",
            ExportFormat.Docx => ".docx",
            _ => throw new ArgumentOutOfRangeException(nameof(format))
        };
    }
}
