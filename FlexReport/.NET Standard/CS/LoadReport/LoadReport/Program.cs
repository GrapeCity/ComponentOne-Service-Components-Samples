using C1.Report;
using LoadReport;

try
{
    var flxrPath = new PathResolver().FlxrPath;
    var reports = LoadReports(flxrPath);
    ReadReports(reports);
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Error: {ex.Message}");
    Console.ResetColor();
}

/// <summary>
/// Loads all report definitions from the given .flxr file.
/// </summary>
IReadOnlyList<FlexReport> LoadReports(string flxrPath)
{
    Console.WriteLine("[1/2] Loading reports...");
    return new ReportLoader().Load(flxrPath);
}

/// <summary>
/// Reads and prints each loaded report to the console.
/// </summary>
void ReadReports(IReadOnlyList<FlexReport> reports)
{
    Console.WriteLine($"[2/2] Reading {reports.Count} report(s)...");
    Console.WriteLine();

    var reader = new ReportReader();
    foreach (var report in reports)
    {
        using (report)
        {
            reader.Read(report);
        }
    }
}
