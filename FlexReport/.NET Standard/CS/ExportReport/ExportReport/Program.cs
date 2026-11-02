using ExportReport;

try
{
    var format = PromptFormat();
    ExportReportFile(format);
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Error: {ex.Message}");
    Console.ResetColor();
}

ExportFormat PromptFormat() => new ExportFormatMenu().SelectExportFormat();

void ExportReportFile(ExportFormat format)
{
    Console.WriteLine();
    var paths = new PathResolver();

    Console.WriteLine("[1/2] Loading report...");
    var report = new ReportLoader().Load(paths.FlxrPath, paths.MdbPath);

    Console.WriteLine($"[2/2] Exporting as {format}...");
    var outputPath = paths.BuildOutputPath(report.ReportName, format);
    new ReportExporter().Export(report, outputPath, format);

    Console.WriteLine();
    Console.WriteLine($"Report saved as {format} at: {outputPath}");
    try
    {
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(outputPath) { UseShellExecute = true });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Warning: Could not open file with default application: {ex.Message}");
    }
}
