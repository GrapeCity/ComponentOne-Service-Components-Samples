using CreateReport;



var flxrPath = SetupOutputPath();
GenerateReport(flxrPath);
PrintResult(flxrPath);


/// <summary>
/// Creates the output directory and returns the full path to the .flxr file.
/// </summary>
string SetupOutputPath()
{
    var outputDir = Path.Combine(Path.GetTempPath(), "EmployeeReport");
    Directory.CreateDirectory(outputDir);
    return Path.Combine(outputDir, "EmployeeReport.flxr");
}

/// <summary>
/// generates data, builds report structure, writes .flxr file.
/// </summary>
void GenerateReport(string flxrPath)
{
    var xmlPath = Path.ChangeExtension(flxrPath, ".xml");

    Console.WriteLine("[1/3] Building employee data...");
    var data = new EmployeeData(xmlPath).Create();

    Console.WriteLine("[2/3] Building report sections...");
    var report = new ReportBuilder(data, xmlPath).Build();

    Console.WriteLine("[3/3] Saving report...");
    report.Save(flxrPath);
}

void PrintResult(string flxrPath)
{
    Console.WriteLine($"Report saved to: {flxrPath}");
    Console.WriteLine("Open it with FlexViewer or FlexReport Designer to view the report.");
}
